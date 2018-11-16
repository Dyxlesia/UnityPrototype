using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController3 : MonoBehaviour {

    [SerializeField] Vector3 vel;           //The velocity of the player

    [SerializeField] Camera cam;            //The main camera

    [SerializeField] GameObject cursorCube; //The cube used as a cursor that the player is always looking at
    [SerializeField] GameObject slash;      //The sprite that apears when you attack
    [SerializeField] GameObject stamina;    //The blue/green bar that decreases when you attack
    [SerializeField] GameObject fuel;       //The white bar that decreases when you dash
    [SerializeField] GameObject aggroPoint;

    [SerializeField] float movespeed;       //How fast the player accelerates
    [SerializeField] float movespeedCap;    //The player's maximum speed. Assigned in the region velocityCapCode. Yes, this can cause bugs
    [SerializeField] float slowspeed;       //How fast the player decelerates
    [SerializeField] float jumpVel;         //Velocity applied when jumping. Assigned when entering/exiting ladder hitboxes.
    [SerializeField] float dashStrength;    //The velocity the player imedietly accelerates to for about a frame when dashing
    [SerializeField] float invincibleTime;  //How long the player is invincible after being hit

    [SerializeField] bool dashAtCursor;     //If true, the player will dash at the cursor. If false, the player will dash in the direction held


    public static string equipedTool;
    public static float slashDamage;        //Set to 1 in Start(). The amount of damage dealt by your attack (public static because AI and AI2 need acsess to this)
    
    float slashTimer;                       //A timer used to determine how fast your attacks stay out
    float dropTimer;                        //A timer used to preventyou from falling through the world when pressing space on a pit
    float distToGround;                     //The distance from the center of the player to the floor
    float damageTimer;

    bool onGround;                          //Is the player on the ground?
    bool overPit;                           //Is the player over a pit?
    bool pressingW;                         //Is the player pressing W?
    bool pressingA;                         //Is the player pressing A?
    bool pressingS;                         //Is the player pressing S?
    bool pressingD;                         //Is the player pressing D?
    bool dashing;                           //Is the player dashing or holding down the dash button?
    bool attacking;                         //Is the player attacking or holding down the attack button?
    bool tookDamage;                        //Did the player just take damage?

    // Use this for initialization
    void Start()
    {
        //Assigning variables
        equipedTool = "wrench";
        slashDamage = 1;
        slashTimer = 0;
        dropTimer = 0;
        distToGround = 1.2f;
        onGround = true;
        overPit = false;
        pressingW = false;
        pressingA = false;
        pressingS = false;
        pressingD = false;
        dashing = false;
        attacking = false;
        tookDamage = false;
        
        //The cursor is now invisible and in locked in the center of the screen
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Regenerate stamina and fuel
        Regen(stamina, Color.green, Color.blue, 0.0075f);
        Regen(fuel, Color.white, Color.white, 0.0015f);
        
        //The velocity is assigned to vel here to make velocity application easier
        vel = gameObject.GetComponent<Rigidbody>().velocity;

        #region tookDamageCode
        if (tookDamage)
        {
            if (gameObject.GetComponent<MeshRenderer>().material.color == Color.blue)
            {
                gameObject.GetComponent<MeshRenderer>().material.color = Color.white;
            }
            else if (gameObject.GetComponent<MeshRenderer>().material.color == Color.white)
            {
                gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
            }

            damageTimer += Time.deltaTime;

            if (damageTimer > invincibleTime)
            {
                tookDamage = false;
                damageTimer = 0;
                gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
            }
        }
        #endregion

        //Modifies the cursor cube's position, then has the player face the cube
        #region faceCursorCode

        //The cursor cube's position is modified by the direction and speed of the mouse cursor, which is stuck at the center of the screen
        cursorCube.transform.position += new Vector3(Input.GetAxis("Mouse X"), 0, Input.GetAxis("Mouse Y"));

        //The direction the player is supposed to face is assigned
        Vector3 lookDirection = new Vector3(cursorCube.transform.position.x, gameObject.transform.position.y, cursorCube.transform.position.z);

        //The player turns to face the cube
        gameObject.transform.LookAt(lookDirection);

        #endregion

        //Increases/decreases player velocity, based on the keys being pressed/not pressed
        #region moveCode

        //Movement in the Z direction. Velocity is added/subtracted to the current velocity
        if (Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
        {
            pressingW = true;
            pressingS = false;
            vel.z += movespeed;
        }
        else if (Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W))
        {
            pressingS = true;
            pressingW = false;
            vel.z -= movespeed;
        }
        else
        {
            //If you don't press anything, you slow down and stop
            pressingW = false;
            pressingS = false;

            if (vel.z > 0)
            {
                if (vel.z - slowspeed > 0)
                {
                    vel.z -= slowspeed;
                }
                else
                {
                    vel.z = 0;
                }

            }
            else if (vel.z < 0)
            {
                if (vel.z + slowspeed < 0)
                {
                    vel.z += slowspeed;
                }
                else
                {
                    vel.z = 0;
                }
            }
        }

        //Movement in the X direction. Velocity is added/subtracted to the current velocity
        if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            pressingA = true;
            pressingD = false;
            vel.x -= movespeed;
        }
        else if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
        {
            pressingD = true;
            pressingA = false;
            vel.x += movespeed;
        }
        else
        {
            //If you don't press anything, you slow down and stop
            pressingA = false;
            pressingD = false;

            if (vel.x > 0)
            {
                if (vel.x - slowspeed > 0)
                {
                    vel.x -= slowspeed;
                }
                else
                {
                    vel.x = 0;
                }

            }
            else if (vel.x < 0)
            {
                if (vel.x + slowspeed < 0)
                {
                    vel.x += slowspeed;
                }
                else
                {
                    vel.x = 0;
                }
            }
        }

        #endregion

        //prevent the player form going faster than the designated speed. The movespeed cap is assigned in here, so if you are modifying that, you need to modify it here as well
        #region velocityCapCode

        //if you are moving diagonal, lower the movespeed cap to make diagonal movement more accurate
        if ((pressingW && pressingA) || (pressingW && pressingD) || (pressingS && pressingA) || (pressingS && pressingD))
        {
            movespeedCap = 6.6f;
        }
        else
        {
            movespeedCap = 10;
        }
        
        //Movement speed cap Z
        if (vel.z > movespeedCap)
        {
            vel.z -= movespeed;
        }
        else if (vel.z < -movespeedCap)
        {
            vel.z += movespeed;
        }

        //Movement speed cap X
        if (vel.x > movespeedCap)
        {
            vel.x -= slowspeed;
        }
        else if (vel.x < -movespeedCap)
        {
            vel.x += slowspeed;
        }

        #endregion

        //If the player is on the ground and they press space, they jump
        #region jumpCode
        /*
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (overPit)
            {
                gameObject.GetComponent<CapsuleCollider>().enabled = false;
                dropTimer += Time.deltaTime;
            }
            else if (onGround)
            {
                vel.y = jumpVel;
                onGround = false;
            }
        }

        //If the player is over a pit, only disable their collider for half a second, then re-enable it
        if (dropTimer > 0)
        {
            dropTimer += Time.deltaTime;

            if (dropTimer >= 0.5f)
            {
                gameObject.GetComponent<CapsuleCollider>().enabled = true;
                dropTimer = 0;
                overPit = false;
            }
        }
        */
        #endregion

        //If you right click and have enough fuel, dash
        #region dashCode
        
        //If the player has enough fuel, dash forward. I'm using GetMouseButton() instead of GetMouseButtonDown() to avoid any inputs getting eaten
        if (Input.GetMouseButton(1) && (dashing == false && cam.transform.position.x - fuel.transform.position.x < 0.56f))
        {
            dashing = true;

            //decrease fuel by a bit in the x direction. Janky, but it works
            fuel.transform.position = new Vector3(fuel.transform.position.x - 0.1f, fuel.transform.position.y, fuel.transform.position.z);

            if (dashAtCursor)
            {
                //you dash towards the cursor
                #region dashTowardsCursor

                //Quaternion bullshit. Velocity is applied in the direction you are facing, which is towards the cursorCube
                vel = gameObject.transform.localRotation * new Vector3(0, 0, dashStrength);

                //If you are pressing shift while dashing, you do a back dash
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    vel = gameObject.transform.localRotation * new Vector3(0, 0, -dashStrength);
                }

                #endregion
            }
            else if (!dashAtCursor)
            {
                //some really cancererous code, but it lets you dash in the direction you hold
                #region dashInDirection

                //if moving up left
                if ((pressingW && pressingA) && !(pressingW && pressingD) && !(pressingS && pressingA) && !(pressingS && pressingD))
                {
                    vel = new Vector3(-dashStrength, 0, dashStrength);
                }
                //if moving up right
                else if (!(pressingW && pressingA) && (pressingW && pressingD) && !(pressingS && pressingA) && !(pressingS && pressingD))
                {
                    vel = new Vector3(dashStrength, 0, dashStrength);
                }
                //if moving down left
                else if (!(pressingW && pressingA) && !(pressingW && pressingD) && (pressingS && pressingA) && !(pressingS && pressingD))
                {
                    vel = new Vector3(-dashStrength, 0, -dashStrength);
                }
                //if moving down right
                else if (!(pressingW && pressingA) && !(pressingW && pressingD) && !(pressingS && pressingA) && (pressingS && pressingD))
                {
                    vel = new Vector3(dashStrength, 0, -dashStrength);
                }
                //if moving up
                else if ((pressingW && !pressingA) && (!pressingS && !pressingD))
                {
                    vel = new Vector3(0, 0, dashStrength);
                }
                //if moving left
                else if ((!pressingW && pressingA) && (!pressingS && !pressingD))
                {
                    vel = new Vector3(-dashStrength, 0, 0);
                }
                //if moving down
                else if ((!pressingW && !pressingA) && (pressingS && !pressingD))
                {
                    vel = new Vector3(0, 0, -dashStrength);
                }
                //if moving right
                else if ((!pressingW && !pressingA) && (!pressingS && pressingD))
                {
                    vel = new Vector3(dashStrength, 0, 0);
                }
                else
                {
                    //vel = new Vector3(vel.x, jumpVel, vel.z);
                }

                #endregion
            }
        }
        else if (!Input.GetMouseButton(1))
        {
            //this is here to prevent players from holding down dash
            dashing = false;
        }

        #endregion

        //player velocity is applied
        gameObject.GetComponent<Rigidbody>().velocity = vel;

        //Check if on ground after velocity is applied. I don't remember why this code goes after velocity, but I do remember that it is supposed to be after
        #region checkIfOnGround
        RaycastHit hit_;
        int layerMask = LayerMask.GetMask("Default");

        if (Physics.Raycast(gameObject.transform.position, Vector3.down, out hit_, distToGround, layerMask))
        {
            onGround = true;
        }
        else
        {
            onGround = false;
        }
        #endregion

        //If you left click, attack
        #region attackCode

        //If you left click and the slash sprite is not visible, attack
        if ((Input.GetMouseButton(0) && attacking == false) && slashTimer == 0)
        {
            attacking = true;

            //deal extra damage if your stamina bar is full
            if (stamina.GetComponent<SpriteRenderer>().color == Color.blue)
            {
                slash.GetComponent<SpriteRenderer>().color = Color.cyan;
                slashDamage = 2;

                if (equipedTool == "wrench")
                {
                    slash.transform.localScale = new Vector3(1.9f, 3f, 1.9f);
                }
                else if (equipedTool == "screwdriver")
                {
                    slash.transform.localScale = new Vector3(1.7f, 3.2f, 1.7f);
                }
                else if (equipedTool == "hammer")
                {
                    slash.transform.localScale = new Vector3(6, 6, 6);
                    slashDamage = 1.5f;
                }
            }
            else
            {
                slash.GetComponent<SpriteRenderer>().color = Color.white;
                slashDamage = 1;

                if (equipedTool == "wrench")
                {
                    slash.transform.localScale = new Vector3(1.5f, 2.5f, 1.5f);
                }
                else if (equipedTool == "screwdriver")
                {
                    slash.transform.localScale = new Vector3(1.5f, 2.2f, 1.5f);
                }
                else if (equipedTool == "hammer")
                {
                    slash.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                    slashDamage = 0.5f;
                }
            }

            //decrease stamina bar
            stamina.transform.position = new Vector3(cam.transform.position.x - 0.65f, stamina.transform.position.y, stamina.transform.position.z);

            //make the slash sprite and hitbox appear
            slash.GetComponent<SpriteRenderer>().enabled = true;
            slash.GetComponent<BoxCollider>().enabled = true;

            //start increasing the slash timer to make this block of code only happen once
            slashTimer += Time.deltaTime;
        }
        else if (slash.GetComponent<SpriteRenderer>().enabled == true)
        {
            //increase the timer
            slashTimer += Time.deltaTime;

            float attacktime = 0;

            if (equipedTool == "wrench")
            {
                attacktime = 0.3f;
            }
            else if (equipedTool == "screwdriver")
            {
                attacktime = 0.2f;
            }
            else if (equipedTool == "hammer")
            {
                attacktime = 0.3f;
            }

            //after the attack has been out for an amount of time, make it diseapear and reset the timer
            if (slashTimer >= attacktime)
            {
                slash.GetComponent<SpriteRenderer>().enabled = false;
                slash.GetComponent<BoxCollider>().enabled = false;

                slashTimer = 0;
            }
        }
        else if (!Input.GetMouseButton(0))
        {
            //this exists to prevent players from holding down the attack button
            attacking = false;
        }

        #endregion
    }

    //Regenerates the Stamina and Fuel bars, but since they are super close to the camea, the values are really small.
    private void Regen(GameObject bar, Color rechargeColor, Color fullColor, float rechargeSpeed)
    {
        bar.transform.position = new Vector3(bar.transform.position.x + rechargeSpeed, bar.transform.position.y, bar.transform.position.z);
        bar.GetComponent<SpriteRenderer>().color = rechargeColor;

        //prevents the bars from going above 100% and going below 0%
        if (bar.transform.position.x > cam.transform.position.x - 0.365f)
        {
            bar.transform.position = new Vector3(cam.transform.position.x - 0.365f, bar.transform.position.y, bar.transform.position.z);
            bar.GetComponent<SpriteRenderer>().color = fullColor;
        }
        else if (bar.transform.position.x < cam.transform.position.x - 0.65f)
        {
            bar.transform.position = new Vector3(cam.transform.position.x - 0.65f, bar.transform.position.y, bar.transform.position.z);
        }
    }

    //if you are in the ladder hitbox you can jump high, if you are over a pit you can fall through, and if you are inside a room, the camera will lerp to the room coordinates
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "ladder")
        {
            jumpVel = 15;
        }

        if (other.gameObject.tag == "pit")
        {
            overPit = true;
        }

        if (other.gameObject.tag == "floor")
        {
            other.gameObject.GetComponent<RoomMovement>().lerpTo = true;

            if (other.gameObject.GetComponent<RoomMovement>().weirdRoom == true)
            {
                aggroPoint.transform.position = other.gameObject.transform.position;
            }
            else
            {
                aggroPoint.transform.position = other.gameObject.transform.position;
            }
        }

        if (other.tag == "item")
        {
            if (other.GetComponent<ItemData>().item == "health")
            {
                Health.playerHealth = 5;
            }
            else
            {
                equipedTool = other.GetComponent<ItemData>().item;
                print(equipedTool);
                slash.GetComponent<SpriteRenderer>().sprite = other.GetComponent<ItemData>().sprite;
                slash.GetComponent<Transform>().localScale = other.GetComponent<ItemData>().scale;
                slash.GetComponent<Transform>().localPosition = other.GetComponent<ItemData>().pos;
                slash.GetComponent<Transform>().localEulerAngles = other.GetComponent<ItemData>().rot;
                slash.GetComponent<BoxCollider>().center = other.GetComponent<ItemData>().colPos;
                slash.GetComponent<BoxCollider>().size = other.GetComponent<ItemData>().colScale;
            }
        }

        if (other.gameObject.tag == "projectile")
        {
            if (!tookDamage)
            {
                Health.playerHealth--;
                tookDamage = true;
            }
        }

        if (other.gameObject.tag == "smashDoor" && other.gameObject.GetComponent<MeshRenderer>().material.color == Color.red)
        {
            Health.playerHealth = 0;
        }
    }

    //if you leave a hit box, things return to normal
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "ladder")
        {
            jumpVel = 5;
        }

        if (other.gameObject.tag == "pit")
        {
            overPit = false;
        }

        if (other.gameObject.tag == "floor")
        {
            other.gameObject.GetComponent<RoomMovement>().lerpTo = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "enemy")
        {
            if (!tookDamage)
            {
                Health.playerHealth--;
                tookDamage = true;
            }
        }
    }   

    private void OnParticleCollision(GameObject other)
    {
        print(other.gameObject);

        if (other.gameObject.tag == "steam")
        {
            if (!tookDamage)
            {
                Health.playerHealth--;
                tookDamage = true;
            }
        }
    }
}

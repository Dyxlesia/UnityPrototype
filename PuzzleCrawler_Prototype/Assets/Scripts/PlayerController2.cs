using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2 : MonoBehaviour {
    [SerializeField] Vector3 vel;

    [SerializeField] float movespeed;
    [SerializeField] float movespeedCap;
    [SerializeField] float slowspeed;
    [SerializeField] float jumpVel;
    [SerializeField] float rollTime;
    [SerializeField] float gravityIncrease;
    [SerializeField] GameObject cam;
    [SerializeField] GameObject slash;
    [SerializeField] GameObject stamina;

    public static float slashDamage;

    Vector3 tempVel;
    float slashTimer;
    float rollTimer;
    float distToGround;
    bool onGround;
    bool pressingWS;
    bool pressingAD;
    bool rolling;
    bool rollRecharge;

    bool endRoll;
    bool endAttack;

    // Use this for initialization
    void Start()
    {
        endRoll = false;
        endAttack = false;
        slashDamage = 1;
        tempVel = new Vector3(0, 0, 0);
        slashTimer = 0;
        rollTimer = 0;
        onGround = true;
        distToGround = 1.2f;
        pressingAD = false;
        pressingWS = false;
        rolling = false;
        rollRecharge = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        regenStamina();

        vel = gameObject.GetComponent<Rigidbody>().velocity;

        if (!rolling)
        {

            //Movement in the Z direction
            if (Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
            {
                pressingWS = true;
                vel.z += movespeed;
                gameObject.transform.eulerAngles = new Vector3(0, 0, 0);
            }
            else if (Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W))
            {
                pressingWS = true;
                vel.z -= movespeed;
                gameObject.transform.eulerAngles = new Vector3(0, 180, 0);
            }
            else
            {
                pressingWS = false;

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

            //Movement in the X direction
            if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
            {
                pressingAD = true;
                vel.x -= movespeed;
                gameObject.transform.eulerAngles = new Vector3(0, 270, 0);
            }
            else if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
            {
                pressingAD = true;
                vel.x += movespeed;
                gameObject.transform.eulerAngles = new Vector3(0, 90, 0);
            }
            else
            {
                pressingAD = false;
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
        }

        //if pressing (w or s) and (a or d)
        //movespeedCap = 4
        if (pressingAD & pressingWS)
        {

            movespeedCap = 6;
        }
        else
        {
            movespeedCap = 8;
        }

        //Movement speed cap Z
        if (vel.z > movespeedCap)
        {
            vel.z -= slowspeed;
            //vel.z = movespeedCap;
        }
        else if (vel.z < -movespeedCap)
        {
            vel.z += slowspeed;
            //vel.z = -movespeedCap;
        }

        //Movement speed cap X
        if (vel.x > movespeedCap)
        {
            vel.x -= slowspeed;
            //vel.x = movespeedCap;
        }
        else if (vel.x < -movespeedCap)
        {
            vel.x += slowspeed;
            //vel.x = -movespeedCap;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (onGround)
            {
                vel.y = jumpVel;
                onGround = false;
            }
        }

        #region rollCode
        if ((Input.GetMouseButton(1) && endRoll == false) && (rolling == false && !rollRecharge))
        {
            endRoll = true;

            tempVel = vel;
            tempVel.x *= 2;
            tempVel.y = -4;
            //gravityIncrease = 1;
            tempVel.z *= 2;

            if (tempVel.x > movespeedCap * 2)
            {
                tempVel.x = movespeedCap * 2;
            }

            if (tempVel.z > movespeedCap * 2)
            {
                tempVel.z = movespeedCap * 2;
            }


            rolling = true;
            gameObject.transform.localScale = new Vector3(1, 0.5f, 1);
        }

        if (!onGround)
        {
            vel.y -= gravityIncrease;

        }

        if (rolling && !rollRecharge)
        {
            gameObject.GetComponent<Rigidbody>().velocity = tempVel;

            rollTimer += Time.deltaTime;

            if (rollTimer >= rollTime)
            {
                rolling = false;
                rollTimer = 0;
                gameObject.transform.localScale = new Vector3(1, 1, 1);
                rollRecharge = true;
            }

        }
        else
        {
            //Velocity is applied
            gameObject.GetComponent<Rigidbody>().velocity = vel;
            gameObject.GetComponent<Renderer>().material.color = Color.blue;
        }

        if (rollRecharge)
        {
            rollTimer += Time.deltaTime;

            if (rollTimer >= 1)
            {
                rollTimer = 0;
                rollRecharge = false;
                gameObject.GetComponent<Renderer>().material.color = Color.green;
            }
        }

        if (!Input.GetMouseButton(1))
        {
            endRoll = false;
        }
        #endregion


        //Check if on ground
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


        //Attack Code
        if ((Input.GetMouseButton(0) && endAttack == false) && slashTimer == 0)
        {
            endAttack = true;
            if (stamina.GetComponent<SpriteRenderer>().color == Color.blue)
            {
                slashDamage = 2;
            }
            else
            {
                slashDamage = 1;
            }

            stamina.transform.position = new Vector3(cam.transform.position.x - 0.65f, stamina.transform.position.y, stamina.transform.position.z);

            //disable control for a bit, add a bit of velocity
            slash.GetComponent<SpriteRenderer>().enabled = true;
            slash.GetComponent<BoxCollider>().enabled = true;
            //print(ComboSlash.comboPoints);
            slashTimer += Time.deltaTime;
        }
        else if (slash.GetComponent<SpriteRenderer>().enabled == true)
        {
            slashTimer += Time.deltaTime;

            if (slashTimer >= 0.3f)
            {
                slash.GetComponent<SpriteRenderer>().enabled = false;
                slash.GetComponent<BoxCollider>().enabled = false;

                slashTimer = 0;
                //if (ComboSlash.hitflag == true)
                //{
                    //ComboSlash.comboPoints += 1;
                //}
            
               // ComboSlash.hitflag = false;
            }
            
            //if (slashTimer >= attackDelay)
            //{
                //slashTimer = 0;
            //}
        }

        if (!Input.GetMouseButton(0))
        {
            endAttack = false;
        }
    }

    private void regenStamina()
    {
        stamina.transform.position = new Vector3(stamina.transform.position.x + 0.0075f, stamina.transform.position.y, stamina.transform.position.z);
        stamina.GetComponent<SpriteRenderer>().color = Color.green;

        if (stamina.transform.position.x > cam.transform.position.x - 0.365f)
        {
            stamina.transform.position = new Vector3(cam.transform.position.x - 0.365f, stamina.transform.position.y, stamina.transform.position.z);
            stamina.GetComponent<SpriteRenderer>().color = Color.blue;
        }
        else if (stamina.transform.position.x < cam.transform.position.x - 0.65f)
        {
            stamina.transform.position = new Vector3(cam.transform.position.x - 0.65f, stamina.transform.position.y, stamina.transform.position.z);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "ladder")
        {
            jumpVel = 17;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "ladder")
        {
            jumpVel = 5;
        }
    }
}

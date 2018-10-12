using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField] Vector3 vel;

    [SerializeField] float movespeed;
    [SerializeField] float movespeedCap;
    [SerializeField] float slowspeed;
    [SerializeField] float jumpVel;
    [SerializeField] float gravityIncrease;
    [SerializeField] GameObject slash;

    float slashTimer;
    float distToGround;
    bool attackBuffer;
    bool onGround;
    bool pressingWS;
    bool pressingAD;
    bool attacking;

    // Use this for initialization
    void Start()
    {
        slashTimer = 0;
        onGround = true;
        distToGround = 1.2f;
        pressingAD = false;
        pressingWS = false;
        attacking = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        vel = gameObject.GetComponent<Rigidbody>().velocity;

        if (attacking == true)
        {
            vel.x *= 1.2f;
            vel.z *= 1.2f;
        }

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
            print("1");
            if (onGround)
            {
                print("2");
                vel.y = jumpVel;
                onGround = false;
            }
        }

        if (!onGround)
        {
            vel.y -= gravityIncrease;

        }

        //Velocity is applied
        gameObject.GetComponent<Rigidbody>().velocity = vel;


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

        if (vel == new Vector3(0, 0, 0))
        {
            attackBuffer = false;
        }

        //Attack Code
        if (Input.GetKeyDown(KeyCode.Return) && attackBuffer == false)
        {
            //disable control for a bit, add a bit of velocity
            attackBuffer = true;
            attacking = true;
            slash.GetComponent<SpriteRenderer>().enabled = true;
            slash.GetComponent<BoxCollider>().enabled = true;
            print(ComboSlash.comboPoints);
        }
        else if (slash.GetComponent<SpriteRenderer>().enabled == true)
        {
            slashTimer += Time.deltaTime;

            if (slashTimer >= 0.07f)
            {
                attacking = false;
            }

            if (slashTimer >= 0.2f)
            {
                
                slashTimer = 0;
                slash.GetComponent<SpriteRenderer>().enabled = false;
                slash.GetComponent<BoxCollider>().enabled = false;

                if (ComboSlash.hitflag == true)
                {
                  ComboSlash.comboPoints += 1;
                }

                ComboSlash.hitflag = false;
            }
        }

       
        //press enter
        //dash a bit in the direction your going while attacking
        //little bit of a pause
        //


    }
}

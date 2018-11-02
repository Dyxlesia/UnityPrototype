using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Crawler : MonoBehaviour {

    [SerializeField] GameObject player;
    [SerializeField] Camera cam;            //The main camera
    [SerializeField] GameObject aggroPoint; //An object that goes to the center of the room whenever the player enters a room

    [SerializeField] float health;
    [SerializeField] float movespeed;
    [SerializeField] float knockback;       //The distance the enemy fets knocked back when you hit it
    [SerializeField] float stunTime;        //The time the enemy is stunned for
    [SerializeField] float ignoreChance;    //The chance that the enemay ignores the player and follows it's path

    [SerializeField] GameObject pointA;
    [SerializeField] GameObject pointB;
    [SerializeField] GameObject pointC;
    [SerializeField] GameObject pointD;

    [SerializeField] int pointTo;

    Rigidbody ridbod;

    GameObject startRoom;

    //float timer;
    float randomNumber;
    float stun;                             //The timer used when the enemy is stuned

    bool hit;                               //Is the enemy hit/stunned?
    bool aggroed;                           //Has the enemy seen the player? AKA Has the player entered the same room as the enemy?

    // Use this for initialization
    void Start()
    {
        //pointTo = 1;
        ridbod = gameObject.GetComponent<Rigidbody>();
        //ridbod.useGravity = false;
        //timer = 1;
        randomNumber = Random.value;
        stun = 0;
        hit = false;
        aggroed = false;
    }

    // Update is called once per frame
    void Update()
    {
        nearAggroPoint();

        if (!aggroed)
        {
            ignoreChance = 1;
        }
        else
        {
            ignoreChance = 0.8f;
        }
        

        if (hit)
        {
            stun += Time.deltaTime;

            GetComponent<MeshRenderer>().material.color = Color.magenta;

            if (stun >= stunTime)
            {
                hit = false;
                GetComponent<MeshRenderer>().material.color = Color.white;
                stun = 0;

                float temp = randomNumber;

                randomNumber = Random.value;

                if (randomNumber <= ignoreChance)
                {
                    randomNumber = temp;
                }
            }

        }
        else
        {
            if (randomNumber > ignoreChance)
            {
                //move towards player
                gameObject.transform.LookAt(player.transform);
            }
            else if (randomNumber <= ignoreChance)
            {
                //folow path

                if (pointTo == 1)
                {
                    gameObject.transform.LookAt(pointA.transform);

                    if (Vector3.Distance(gameObject.transform.position, pointA.transform.position) < 2)
                    {
                        pointTo = 2;
                    }
                }
                else if (pointTo == 2)
                {
                    gameObject.transform.LookAt(pointB.transform);

                    if (Vector3.Distance(gameObject.transform.position, pointB.transform.position) < 2)
                    {
                        pointTo = 3;
                    }
                }
                else if (pointTo == 3)
                {
                    gameObject.transform.LookAt(pointC.transform);

                    if (Vector3.Distance(gameObject.transform.position, pointC.transform.position) < 2)
                    {
                        pointTo = 4;
                    }
                }
                else if (pointTo == 4)
                {
                    gameObject.transform.LookAt(pointD.transform);

                    if (Vector3.Distance(gameObject.transform.position, pointD.transform.position) < 2)
                    {
                        pointTo = 1;
                    }
                }

                
            }

            gameObject.transform.eulerAngles = new Vector3(0, gameObject.transform.eulerAngles.y, 0);

            ridbod.velocity = gameObject.transform.localRotation * new Vector3(0, 0, movespeed);
        }

        if (health <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //If the enemy is hit by the player attack, get knocked back in a direction and lose health
        if (other.tag == "slash")
        {
            //The enemy loses health
            health -= PlayerController3.slashDamage;

            //The stun timer is set to start at 0, and the enemy is maked as hit
            stun = 0;
            hit = true;

            //The enemy is knocked back in the direction opposite of the player. This code block is a complicated mess of angle calculation and if statements,
            //and I am not going to bother commenting most of it. It shouldn't be nessesary to change enything in here, so all should be well.
            #region knockbackCode

            //calculates the angle the enemy is from the player
            float tempX = player.transform.position.x - gameObject.transform.position.x;
            float tempZ = player.transform.position.z - gameObject.transform.position.z;

            float theta = (Mathf.Atan(tempZ / tempX)) * (180 / Mathf.PI);

            Vector3 tempVel = new Vector3();

            if ((gameObject.transform.position.z > player.transform.position.z) && (gameObject.transform.position.x > player.transform.position.x))
            {
                if (theta > 67.5f)
                {
                    tempVel = new Vector3(0, 0, knockback);//U
                }
                else if (theta < 22.5f)
                {
                    tempVel = new Vector3(knockback, 0, 0);//R
                }
                else if (theta <= 67.5f && theta >= 22.5)
                {
                    tempVel = new Vector3(knockback * 0.66f, 0, knockback * 0.66f);//UR
                }

            }
            else if ((gameObject.transform.position.z < player.transform.position.z) && (gameObject.transform.position.x < player.transform.position.x))
            {
                if (theta > 67.5f)
                {
                    tempVel = new Vector3(0, 0, -knockback);//D
                }
                else if (theta < 22.5f)
                {
                    tempVel = new Vector3(-knockback, 0, 0);//L
                }
                else if (theta <= 67.5f && theta >= 22.5)
                {
                    tempVel = new Vector3(-knockback * 0.66f, 0, -knockback * 0.66f);//DL
                }
            }
            else if ((gameObject.transform.position.z > player.transform.position.z) && (gameObject.transform.position.x < player.transform.position.x))
            {
                if (theta < -67.5f)
                {
                    tempVel = new Vector3(0, 0, knockback);//U
                }
                else if (theta > -22.5f)
                {
                    tempVel = new Vector3(-knockback, 0, 0);//L
                }
                else if (theta >= -67.5f && theta <= -22.5)
                {
                    tempVel = new Vector3(-knockback * 0.66f, 0, knockback * 0.66f);//UL
                }
            }
            else if ((gameObject.transform.position.z < player.transform.position.z) && (gameObject.transform.position.x > player.transform.position.x))
            {
                if (theta < -67.5f)
                {
                    tempVel = new Vector3(0, 0, -knockback);//D
                }
                else if (theta > -22.5f)
                {
                    tempVel = new Vector3(knockback, 0, 0);//R
                }
                else if (theta >= -67.5f && theta <= -22.5)
                {
                    tempVel = new Vector3(knockback * 0.66f, 0, -knockback * 0.66f);//DR
                }
            }

            //velocity is applied
            gameObject.GetComponent<Rigidbody>().velocity = tempVel;

            #endregion
        }

        if (other.tag == "floor")
        {
            startRoom = other.gameObject;
        }

        if (other.gameObject.tag == "smashDoor")
        {
            health = 0;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "zap")
        {
            if (other.GetComponent<ParticleSystem>().emission.enabled)
            {
                health -= Time.deltaTime / 4;
            }
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.tag == "steam")
        {
            health -= Time.deltaTime / 4;
        }
    }

    //Workaround because raycasts didn't work. If the player moves inside a room, the aggro point moves into that room, aggroing the enemies
    private void nearAggroPoint()
    {
        if (startRoom.transform.position == aggroPoint.transform.position)
        {
            float tempY = aggroPoint.transform.position.y - gameObject.transform.position.y;

            if (Mathf.Abs(tempY) < 2)
            {
                aggroed = true;
            }
        }
    }
}

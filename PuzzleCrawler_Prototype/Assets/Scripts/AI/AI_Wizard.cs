using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Wizard : MonoBehaviour {

    [SerializeField] GameObject player;
    [SerializeField] Camera cam;            //The main camera
    [SerializeField] GameObject aggroPoint; //An object that goes to the center of the room whenever the player enters a room

    [SerializeField] GameObject orb;
    [SerializeField] GameObject ball1;
    [SerializeField] GameObject ball2;
    [SerializeField] GameObject ball3;
    [SerializeField] GameObject shot;

    [SerializeField] float health;
    [SerializeField] float movespeed;
    [SerializeField] float knockback;       //The distance the enemy fets knocked back when you hit it
    [SerializeField] float stunTime;        //The time the enemy is stunned for
    [SerializeField] float teleportDist;
    [SerializeField] float shotSpeed;

    Rigidbody ridbod;

    GameObject startRoom;

    Vector3 startpos;
    Vector3 orbstartpos;

    float timer;
    float stun;                             //The timer used when the enemy is stuned

    bool playerCursed;
    bool hit;                               //Is the enemy hit/stunned?
    bool aggroed;                           //Has the enemy seen the player? AKA Has the player entered the same room as the enemy?

    // Use this for initialization
    void Start()
    {
        orbstartpos = gameObject.transform.position;
        startpos = gameObject.transform.position;
        //pointTo = 1;
        ridbod = gameObject.GetComponent<Rigidbody>();
        ridbod.useGravity = false;
        timer = 0;
        //randomNumber = Random.value;
        stun = 0;
        hit = false;
        aggroed = false;
        playerCursed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (startRoom != null)
        {
            if (startRoom.GetComponent<MeshRenderer>().enabled == false)
            {
                ridbod.useGravity = false;
                ridbod.velocity = new Vector3(0, 0, 0);
            }
        }

        if (playerCursed)
        {
            orb.transform.localEulerAngles = new Vector3(0, orb.transform.localEulerAngles.y + 1, 0);

            orb.transform.position = Vector3.Lerp(orb.transform.position, player.transform.position, 2 * Time.deltaTime);
        }

        nearAggroPoint();

        if (!aggroed)
        {
            return;
        }

        ridbod.useGravity = true;

        if (hit)
        {
            stun += Time.deltaTime;

            GetComponent<MeshRenderer>().material.color = Color.magenta;

            if (stun >= stunTime)
            {
                hit = false;
                GetComponent<MeshRenderer>().material.color = Color.white;
                stun = 0;

                teleport();
            }

        }

        shot.transform.LookAt(player.transform);

        if (shot.transform.parent != null)
        {
            shot.transform.position = gameObject.transform.position;
        }

        if (orb.transform.parent != null)
        {
            orbstartpos = gameObject.transform.position;
            
        }

        timer += Time.deltaTime;

        if (!playerCursed)
        {
            if (timer > 0.5f && timer <= 1)
            {
                orb.transform.position = new Vector3(orbstartpos.x, Mathf.Lerp(orb.transform.position.y, orbstartpos.y + 2, 0.2f), orbstartpos.z);
            }

            if (timer > 1 && timer <= 2)
            {
                ball1.transform.position = new Vector3(orbstartpos.x, orbstartpos.y + 2, Mathf.Lerp(ball1.transform.position.z, orbstartpos.z + 4, 0.2f));
                ball2.transform.position = new Vector3(Mathf.Lerp(ball2.transform.position.x, orbstartpos.x + 3.48f, 0.2f), orbstartpos.y + 2, Mathf.Lerp(ball2.transform.position.z, orbstartpos.z - 2, 0.2f));
                ball3.transform.position = new Vector3(Mathf.Lerp(ball3.transform.position.x, orbstartpos.x - 3.48f, 0.2f), orbstartpos.y + 2, Mathf.Lerp(ball3.transform.position.z, orbstartpos.z - 2, 0.2f));
            }

            if (timer > 2)
            {
                playerCursed = true;
                orb.transform.parent = null;
                timer += 2;
            }
        }

        if (playerCursed)
        {
            //orb.transform.localEulerAngles = new Vector3(0, orb.transform.localEulerAngles.y + 1, 0);

            //orb.transform.position = Vector3.Lerp(orb.transform.position, player.transform.position, 2 * Time.deltaTime);

            if (timer > 3 && timer <= 3.5f)
            {
                shot.transform.position = gameObject.transform.position;
                shot.GetComponent<Rigidbody>().velocity = shot.transform.rotation * new Vector3(0, 0, shotSpeed);
                shot.transform.parent = null;
                shot.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
            }
            
        }

        if (timer >= 5)
        { 
            teleport();
            shot.transform.position = gameObject.transform.position;
            shot.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            shot.transform.parent = gameObject.transform;
            shot.transform.localScale = new Vector3(0.95f, 0.95f, 0.95f);
            timer = 0;
        }


        if (health <= 0)
        {
            gameObject.SetActive(false);
            orb.SetActive(false);
            playerCursed = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //If the enemy is hit by the player attack, get knocked back in a direction and lose health
        if (other.tag == "slash" && gameObject.tag == "enemy")
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
        if (aggroed && aggroPoint.transform.position.y != startRoom.transform.position.y)
        {
            aggroed = false;
            ridbod.useGravity = false;
        }

        if (startRoom.transform.position == aggroPoint.transform.position)
        {
            float tempY = aggroPoint.transform.position.y - gameObject.transform.position.y;

            if (Mathf.Abs(tempY) < 2)
            {
                aggroed = true;
            }
        }
    }

    private void teleport()
    {
        float randx = Random.Range(-teleportDist, teleportDist);
        float randz = Random.Range(-teleportDist, teleportDist);

        gameObject.transform.position = new Vector3(startpos.x + randx, startpos.y, startpos.z + randz);
    }
}


﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Squid : MonoBehaviour {

    [SerializeField] GameObject player;
    [SerializeField] Camera cam;            //The main camera
    [SerializeField] GameObject body;

    [SerializeField] float dashStrenth;
    [SerializeField] float dashRate;
    [SerializeField] float attackChance;
    [SerializeField] float health;
    [SerializeField] float knockback;       //The distance the enemy fets knocked back when you hit it
    [SerializeField] float stunTime;        //The time the enemy is stunned for

    Rigidbody ridbod;

    float timer;
    float randomNumber;
    float stun;                             //The timer used when the enemy is stuned

    bool hit;                               //Is the enemy hit/stunned?
    //bool aggroed;                           //Has the enemy seen the player? AKA Has the player entered the same room as the enemy?

    // Use this for initialization
    void Start ()
    {
        ridbod = gameObject.GetComponent<Rigidbody>();
        timer = 1;
        randomNumber = 0;
        stun = 0;
        hit = false;
        //aggroed = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
        /*
        InCameraVeiw();

        if (!aggroed)
        {
            return;
        }
        */

        if (hit)
        {
            body.transform.localScale = new Vector3(1, 1, 1);

            stun += Time.deltaTime;

            //The enemy turns red
            body.GetComponent<Renderer>().material.color = Color.magenta;

            if (stun >= stunTime)
            {
                hit = false;
                stun = 0;
                body.GetComponent<Renderer>().material.color = Color.white;
            }
        }
        else
        {
            timer += Time.deltaTime;

            if (timer >= dashRate - 0.5f && timer < dashRate)
            {
                body.GetComponent<Renderer>().material.color = Color.white;
                body.transform.localScale += new Vector3(0.02f, 0.02f, 0.02f);
            }

            if (timer >= dashRate)
            {
                body.transform.localScale = new Vector3(1, 1, 1);
                randomNumber = Random.value;

                //attack
                if (attackChance >= randomNumber)
                {
                    gameObject.transform.LookAt(player.transform.position);
                    ridbod.velocity = gameObject.transform.localRotation * new Vector3(0, 0, dashStrenth * 1.5f);
                    body.GetComponent<Renderer>().material.color = Color.red;
                }
                else // move randomly
                {
                    randomNumber = Random.Range(-180, 180);
                    gameObject.transform.localEulerAngles = new Vector3(0, randomNumber, 0);

                    ridbod.velocity = gameObject.transform.localRotation * new Vector3(0, 0, dashStrenth);
                    body.GetComponent<Renderer>().material.color = Color.white;
                }

                timer = 0;
            }
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
    }

    //Raycasts to see if the enemy is in veiw of the camera. Please don't edit this code. Also, if you disable upper floors, bad things happen. 
    //This also means the top floor of the dungeon should have higer walls than normal
    private void InCameraVeiw()
    {
        RaycastHit rayhit;
        Debug.DrawRay(cam.transform.position, gameObject.transform.position - cam.transform.position, Color.yellow);

        Physics.Raycast(cam.transform.position, gameObject.transform.position - cam.transform.position, out rayhit);

        //If the raycast hits the enemy, the player is in the same room as the enemy, and the enemy is aggroed
        if (rayhit.collider == gameObject.GetComponent<Collider>())
        {
            //aggroed = true;
            print(rayhit.collider);  //Just some debug code
        }
    }

}

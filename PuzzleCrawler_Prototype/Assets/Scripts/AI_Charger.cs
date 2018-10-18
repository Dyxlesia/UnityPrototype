﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Charger : MonoBehaviour
{

    [SerializeField] GameObject player;
    [SerializeField] Camera cam;            //The main camera
    [SerializeField] GameObject aggroPoint; //An object that goes to the center of the room whenever the player enters a room

    [SerializeField] GameObject leftArm;
    [SerializeField] GameObject rightArm;

    [SerializeField] float health;
    [SerializeField] float movespeed;
    [SerializeField] float knockback;       //The distance the enemy fets knocked back when you hit it
    [SerializeField] float stunTime;        //The time the enemy is stunned for

    Rigidbody ridbod;

    GameObject startRoom;

    float timer;
    float stun;                             //The timer used when the enemy is stuned
    float Yfix;

    bool hit;                               //Is the enemy hit/stunned?
    bool aggroed;                           //Has the enemy seen the player? AKA Has the player entered the same room as the enemy?

    // Use this for initialization
    void Start()
    {
        Yfix = gameObject.transform.position.y;
        //pointTo = 1;
        ridbod = gameObject.GetComponent<Rigidbody>();
        ridbod.useGravity = false;
        timer = 0;
        //randomNumber = Random.value;
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
            }

        }
        else
        {
            if (timer < 3)
            {
                gameObject.transform.LookAt(player.transform);
                gameObject.transform.eulerAngles = new Vector3(0, gameObject.transform.eulerAngles.y, 0);
            }

            timer += Time.deltaTime;

            if (timer >= 2.5)
            {
                GetComponent<MeshRenderer>().material.color = Color.red;
                leftArm.GetComponent<MeshRenderer>().material.color = Color.red;
                rightArm.GetComponent<MeshRenderer>().material.color = Color.red;
            }

            if (timer >= 3)
            {
                ridbod.velocity = gameObject.transform.localRotation * new Vector3(0, 0, movespeed - (timer * timer));
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation((player.transform.position - transform.position).normalized), 0.03f);
                gameObject.transform.eulerAngles = new Vector3(0, gameObject.transform.eulerAngles.y, 0);
            }

            if (timer >= 6)
            {
                ridbod.velocity = new Vector3(0, 0, 0);
                timer = 0;
                GetComponent<MeshRenderer>().material.color = Color.white;
                leftArm.GetComponent<MeshRenderer>().material.color = Color.white;
                rightArm.GetComponent<MeshRenderer>().material.color = Color.white;
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
    }

    //Workaround because raycasts didn't work. If the player moves inside a room, the aggro point moves into that room, aggroing the enemies
    private void nearAggroPoint()
    {
        if (aggroed && aggroPoint.transform.position.y != startRoom.transform.position.y)
        {
            aggroed = false;
            ridbod.useGravity = false;
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, Yfix, gameObject.transform.position.z);
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI2 : MonoBehaviour {

    //This enemy walks wowars

    [SerializeField] GameObject player;
    [SerializeField] GameObject shockwave;
    [SerializeField] GameObject cam;
    [SerializeField] float lerpSpeed;
    [SerializeField] float knockback;
    [SerializeField] float stunTime;
    [SerializeField] float health;

    bool hit;
    bool attack;
    bool aggroed;
    float stun;
    float cycleTimer;

    // Use this for initialization
    void Start()
    {
        hit = false;
        attack = false;
        aggroed = false;
        stun = 0;
        cycleTimer = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        InCameraVeiw();

        if (aggroed == false)
        {
            return;
        }

        if (!attack)
        {
            cycleTimer += Time.deltaTime;

            if (cycleTimer > 2.5 && cycleTimer < 3.8)
            {
                gameObject.transform.position += new Vector3(0, 0.1f, 0);
            }
            else if (cycleTimer >= 3.8f && cycleTimer < 4)
            {
                gameObject.transform.position -= new Vector3(0, 0.4f, 0);
            }
            else if (cycleTimer >= 4)
            {
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, 1, gameObject.transform.position.z);
                gameObject.GetComponent<Renderer>().material.color = Color.white;
                attack = true;
                cycleTimer = 0;
            }
        }
        else if (attack)
        {
            shockwave.transform.localScale = new Vector3(shockwave.transform.localScale.x + 0.1f, 1, shockwave.transform.localScale.z + 0.1f);

            cycleTimer += Time.deltaTime;

            if (cycleTimer >= 4)
            {
                attack = false;
                cycleTimer = 0;
                shockwave.transform.localScale = new Vector3(0.2f, 1, 0.2f);
            }
        }


        if (hit == false)
        {
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, player.transform.position, lerpSpeed);
        }
        else if (hit == true)
        {
            stun += Time.deltaTime;

            gameObject.GetComponent<Renderer>().material.color = Color.red;

            if (stun > stunTime)
            {
                hit = false;
                stun = 0;
                gameObject.GetComponent<Renderer>().material.color = Color.white;
                gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            }
        }

        if (health <= 0)
        {
            gameObject.SetActive(false);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "slash")
        {
            ComboSlash.hitflag = true;
            health -= PlayerController2.slashDamage;
            stun = 0;
            hit = true;

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

            gameObject.GetComponent<Rigidbody>().velocity = tempVel;
        }
    }

    private void InCameraVeiw()
    {
        //Vector3 temp = (camera.transform.position - gameObject.transform.position) / (camera.transform.position - gameObject.transform.position).magnitude;
        RaycastHit rayhit;
        Debug.DrawRay(cam.transform.position, gameObject.transform.position - cam.transform.position, Color.yellow);

        Physics.Raycast(cam.transform.position, gameObject.transform.position - cam.transform.position, out rayhit);



        if (rayhit.collider == gameObject.GetComponent<Collider>())
        {
            aggroed = true;
            print(rayhit.collider);
        }
    }

}

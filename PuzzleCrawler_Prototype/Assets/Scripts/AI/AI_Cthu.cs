using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Cthu : MonoBehaviour {

    [SerializeField] GameObject player;
    [SerializeField] GameObject cam;
    [SerializeField] GameObject shot;
    [SerializeField] float aggroTime;

    [SerializeField] AudioSource teleport;
    [SerializeField] AudioSource shoot;


    bool aggroed;
    float timer;

	// Use this for initialization
	void Start ()
    {
        aggroed = false;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (Timer.GlobalTimerValue > aggroTime || (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.F)))
        {
            aggroed = true;
        }


        if (aggroed)
        {
            if (Vector3.Distance(gameObject.transform.position, player.transform.position) > 20)
            {
                teleport.Play();

                float rx = Random.Range(2, 5);
                float rz = Random.Range(2, 5);

                if (Random.value > 0.5f)
                {
                    rx *= -1;
                }

                if (Random.value > 0.5f)
                {
                    rz *= -1;
                }


                gameObject.transform.position = new Vector3(player.transform.position.x + rx, 2.5f, player.transform.position.z + rz);
            }

            //transform.parent = null;
            transform.LookAt(player.transform);
            transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y + 180, 0);
            //transform.parent = cam.transform;

            timer += Time.deltaTime;

            if (timer > 1)
            {
                float randomNumberX = Random.Range(-10, 10);
                float randomNumberZ = Random.Range(-10, 10);

                GetComponent<Rigidbody>().velocity = 2 * new Vector3(randomNumberX, 0, randomNumberZ);

                shoot.Play();
                GameObject energyBall = Instantiate(shot, shot.transform.position, shot.transform.rotation, null);
                energyBall.transform.LookAt(player.transform);
                energyBall.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
                energyBall.GetComponent<Rigidbody>().velocity = energyBall.transform.rotation * new Vector3(0, 0, 30);

                timer = 0;
            }
        }
	}
}

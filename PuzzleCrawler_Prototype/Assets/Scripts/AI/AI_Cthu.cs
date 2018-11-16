using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Cthu : MonoBehaviour {

    [SerializeField] GameObject player;
    [SerializeField] GameObject cam;
    [SerializeField] GameObject shot;

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
        if (Timer.GlobalTimerValue > 100)
        {
            aggroed = true;
        }


        if (aggroed)
        {
            if (Vector3.Distance(gameObject.transform.position, player.transform.position) > 20)
            {
                gameObject.transform.position = new Vector3(player.transform.position.x, 2.5f, player.transform.position.z);
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

                //
                GameObject energyBall = Instantiate(shot, shot.transform.position, shot.transform.rotation, null);
                energyBall.transform.LookAt(player.transform);
                energyBall.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
                energyBall.GetComponent<Rigidbody>().velocity = energyBall.transform.rotation * new Vector3(0, 0, 30);

                timer = 0;
            }
        }
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateTech : MonoBehaviour {

    [SerializeField] GameObject player;
    [SerializeField] string toolType;
    [SerializeField] GameObject sign;

    float timer;

    // Use this for initialization
    void Start ()
    {
        timer = 0;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Vector3.Distance(gameObject.transform.position, player.transform.position) < 4)
        {
            if (Input.GetKey(KeyCode.Q) && toolType == PlayerController3.equipedTool)
            {
                sign.GetComponent<PressQSign>().gettingFixed = true;

                timer += Time.deltaTime;

                if (timer > 3)
                {
                    gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
                }
            }
            else
            {
                sign.GetComponent<PressQSign>().gettingFixed = false;
            }

        }
        else
        {
            sign.GetComponent<PressQSign>().gettingFixed = false;
        }
    }
}

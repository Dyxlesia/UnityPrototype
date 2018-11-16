using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechDoor : MonoBehaviour {

    [SerializeField] GameObject player;
    [SerializeField] float riseSpeed;
    [SerializeField] float smashSpeed;

    float timer;
    bool powered;
    bool moveUp;
    bool moveDown;

	// Use this for initialization
	void Start () {
        timer = 0;
        powered = false;
        moveUp = false;
        moveDown = true;
	}
	
	// Update is called once per frame
	void Update ()
    {

        if (GetComponent<MeshRenderer>().material.color == Color.red)
        {
            gameObject.GetComponent<ActivateTech>().enabled = false;
            gameObject.GetComponent<BoxCollider>().isTrigger = true;
            GetComponent<MeshRenderer>().material.color = Color.red;
            powered = false;

            if (moveDown)
            {
                gameObject.GetComponent<BoxCollider>().isTrigger = true;
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, Mathf.Lerp(gameObject.transform.position.y, 2.5f, smashSpeed), gameObject.transform.position.z);

                if (gameObject.transform.position.y < 2.55f)
                {
                    moveDown = false;
                    moveUp = true;
                }
            }
            else if (moveUp)
            {
                gameObject.GetComponent<BoxCollider>().isTrigger = false;
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, Mathf.Lerp(gameObject.transform.position.y, 6, riseSpeed), gameObject.transform.position.z);

                if (gameObject.transform.position.y > 5.9f)
                {
                    moveDown = true;
                    moveUp = false;
                }
            }

        }
        else if (GetComponent<MeshRenderer>().material.color == Color.green)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, Mathf.Lerp(gameObject.transform.position.y, 6, 0.2f), gameObject.transform.position.z);

            if (gameObject.transform.position.y > 5.9f)
            {
                powered = true;
            }
            
        }

        if (Input.GetKey(KeyCode.Q) && Vector3.Distance(gameObject.transform.position, player.transform.position) < 7)
        {
            if (powered == true)
            {
                timer += Time.deltaTime;

                Color col = GetComponent<MeshRenderer>().material.color;

                col = Color.Lerp(col, Color.red, timer * 0.1f);

                GetComponent<MeshRenderer>().material.color = col;
            }
        }

        
    }
}

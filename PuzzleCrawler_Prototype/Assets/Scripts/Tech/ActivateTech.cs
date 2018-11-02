using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateTech : MonoBehaviour {

    [SerializeField] GameObject player;
    [SerializeField] string toolType;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Vector3.Distance(gameObject.transform.position, player.transform.position) < 3)
        {
            if (Input.GetKey(KeyCode.Q) && toolType == PlayerController3.equipedTool)
            {
                gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
            }
        }
	}
}

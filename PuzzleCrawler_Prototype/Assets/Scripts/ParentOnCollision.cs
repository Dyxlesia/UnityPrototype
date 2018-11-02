using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentOnCollision : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.transform.parent.gameObject.transform.parent == null && collision.gameObject.transform.parent.gameObject.name == "Room(Clone)")
        {
            collision.gameObject.transform.parent.gameObject.transform.parent = gameObject.transform.parent;

            if (collision.gameObject.GetComponent<ParentOnCollision>() != null)
            {
                foreach (ParentOnCollision parentScript in collision.gameObject.transform.parent.gameObject.GetComponentsInChildren<ParentOnCollision>())
                {
                    parentScript.enabled = true;
                }
            }
        }
    }

    //Attached to bigroomDoors

    //onCollision
    //Parent the collision to the big room
    //activate the ParentOnCollision script on all children with it

    //after a second, stop parenting
    //delete all room(clone)s not parented to big room
}

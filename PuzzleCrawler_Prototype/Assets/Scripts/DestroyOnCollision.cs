using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnCollision : MonoBehaviour {

    [SerializeField] bool bigroom;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        foreach (GameObject door in GameObject.FindGameObjectsWithTag("door"))
        {
            
        }
	}

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.name == "BigRoom(Clone)" && bigroom == true)
        {
            Destroy(gameObject);
            RoomRandomization.bigroomCounter--;
        }

        if (collision.gameObject.name == "BigRoom(Clone)" && bigroom == false)
        {
            Destroy(gameObject);
            RoomRandomization.smallroomCounter--;
        }
    }
}

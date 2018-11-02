using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomMovement : MonoBehaviour {

    [SerializeField] GameObject player;     //The player
    [SerializeField] GameObject cam;        //The main camera
    [SerializeField] Vector3 camCoor;       //The coordinates the camera moves to when the player enters a room
    
    Vector3 roomCoor;                       //The coordinates of the room

    float lerptime = 0.05f;                 //The time it takes for the camera to lerp to a room
    public bool lerpTo;                     //Should the camera lerp towards the room coordinates?
    public bool weirdRoom;                  //Is the room any shape or size other than the standard square room?

    [SerializeField] Vector3 wrConstraints; //Camera constraints for weird rooms, values are added and subtracted from the room's center
 
    // Use this for initialization
    void Start()
    {
        //roomCoor is, surprisingly, the coordinates of the room this script is attached to
        roomCoor = gameObject.transform.position;

        //defaults to false
        lerpTo = false; 
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //If the player is inside the trigger of the room, the camera will lerp towards the room coordinates
        if (lerpTo == true)
        {
            //if the room requires the camera to follow the player
            if (weirdRoom)
            {
                //this code calculates if the camera should folow the player or stop before revealing something in another room. Don't change anything here please
                #region constraintCalc
                float lx = player.transform.position.x;

                if (player.transform.position.x > roomCoor.x + wrConstraints.x)
                {
                    lx = roomCoor.x + wrConstraints.x;
                }
                else if (player.transform.position.x < roomCoor.x - wrConstraints.x)
                {
                    lx = roomCoor.x - wrConstraints.x;
                }

                float lz = player.transform.position.z;

                if (player.transform.position.z > roomCoor.z + wrConstraints.z)
                {
                    lz = roomCoor.z + wrConstraints.z;
                }
                else if (player.transform.position.z < roomCoor.z - wrConstraints.z) 
                {
                    lz = roomCoor.z - wrConstraints.z;
                }
                #endregion

                //The camera lerps towards the coordinates it is supposed to go to. Don't change anything here please
                cam.transform.position = new Vector3(Mathf.Lerp(cam.transform.position.x, lx, lerptime), Mathf.Lerp(cam.transform.position.y, camCoor.y, lerptime), Mathf.Lerp(cam.transform.position.z, camCoor.z + lz, lerptime));
            }
            else
            {
                //The camera lerps towards the coordinates it is supposed to go to
                cam.transform.position = new Vector3(Mathf.Lerp(cam.transform.position.x, camCoor.x, lerptime), Mathf.Lerp(cam.transform.position.y, camCoor.y, lerptime), Mathf.Lerp(cam.transform.position.z, camCoor.z, lerptime));

                //if the camera is close enough to the coordinates it is supposed to lerp to, snap to them instead
                float dist = Vector3.Distance(cam.transform.position, camCoor);

                if (dist <= 0.05f)
                {
                    cam.transform.position = camCoor;
                }
            }




            
            
        }
    
        //If the room is above the player, make it invisible
        if (player.transform.position.y < gameObject.transform.position.y)
        {
            foreach (MeshRenderer m in gameObject.GetComponentsInChildren<MeshRenderer>())
            {
                m.enabled = false;

                if (m.gameObject.GetComponent<MeshCollider>() && (m.gameObject.tag == "floor") || m.gameObject.tag == "vanishingWall")
                {
                    m.gameObject.GetComponent<MeshCollider>().enabled = false;
                }
                
            }
        }
        else
        {
            //If the player is above or in the room, make it visible. DO NOT TOUCH THIS CODE. BAD THINGS HAPPEN IF YOU CHANGE THIS CODE.
            foreach (MeshRenderer m in gameObject.GetComponentsInChildren<MeshRenderer>())
            {
                if (m.gameObject.tag == "vanishingWall")
                {
                    if (!m.gameObject.GetComponent<WorkAroundWalls>().cameraInside)
                    {
                        m.enabled = true;

                        if (m.gameObject.GetComponent<MeshCollider>() && (m.gameObject.tag == "floor") || m.gameObject.tag == "vanishingWall")
                        {
                            m.gameObject.GetComponent<MeshCollider>().enabled = true;
                        } 
                    }
                }
                else
                {
                    m.enabled = true;

                    if (m.gameObject.GetComponent<MeshCollider>() && (m.gameObject.tag == "floor") || m.gameObject.tag == "vanishingWall")
                    {
                        m.gameObject.GetComponent<MeshCollider>().enabled = true;
                    }
                }
            }
        }

    }


}

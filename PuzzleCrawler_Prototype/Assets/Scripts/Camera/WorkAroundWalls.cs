using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkAroundWalls : MonoBehaviour {

    //This code is important for enemies to not activate unitl you enter the same room as them. Don't change anything

    public bool cameraInside;       

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "MainCamera")
        {
            cameraInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "MainCamera")
        {
            cameraInside = false;
        }
    }
}

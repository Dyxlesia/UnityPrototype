using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VeiwThruWalls : MonoBehaviour {

    //This code is important to make certain walls disapear when the camera hitbox touches them

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "vanishingWall")
        {
            foreach (MeshRenderer m in other.gameObject.GetComponentsInChildren<MeshRenderer>())
            {
                m.enabled = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "vanishingWall")
        {
            foreach (MeshRenderer m in other.gameObject.GetComponentsInChildren<MeshRenderer>())
            {
                m.enabled = true;
            }
        }
    }
}

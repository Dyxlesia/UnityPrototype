using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKey(KeyCode.LeftShift) & Input.GetKeyDown(KeyCode.Alpha1))
        {
            SceneManager.LoadScene("Combat_QuickStun");
        }
        else if (Input.GetKey(KeyCode.LeftShift) & Input.GetKeyDown(KeyCode.Alpha2))
        {
            SceneManager.LoadScene("Combat_demo");
        }
    }
}

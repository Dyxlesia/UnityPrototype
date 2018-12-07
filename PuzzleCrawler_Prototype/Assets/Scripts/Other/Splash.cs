using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Splash : MonoBehaviour {

    float timer;

	// Use this for initialization
	void Start () {
        timer = 0;
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;

        if (timer > 3)
        {
            gameObject.transform.position = new Vector3(0, 0, -10);

            if (timer > 6)
            {
                SceneManager.LoadScene("Title");
            }
        }
	}
}

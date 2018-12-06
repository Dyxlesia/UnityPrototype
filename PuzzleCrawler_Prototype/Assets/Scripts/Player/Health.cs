using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour {

    public static float playerHealth;
    [SerializeField] GameObject cam;
    [SerializeField] GameObject hp1;
    [SerializeField] GameObject hp2;
    [SerializeField] GameObject hp3;
    [SerializeField] GameObject hp4;
    [SerializeField] GameObject hp5;
    [SerializeField] Text text;

    // Use this for initialization
    void Start () {
        playerHealth = 5;
	}
	
	// Update is called once per frame
	void Update ()
    {
        /*
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            playerHealth = 5;
            hp5.SetActive(true);
            hp4.SetActive(true);
            hp3.SetActive(true);
            hp2.SetActive(true);
            hp1.SetActive(true);
        }*/

        if (playerHealth >= 5)
        {
            hp5.SetActive(true);
            hp4.SetActive(true);
            hp3.SetActive(true);
            hp2.SetActive(true);
            hp1.SetActive(true);
        }
        else if (playerHealth == 4)
        {
            hp5.SetActive(false);
            hp4.SetActive(true);
            hp3.SetActive(true);
            hp2.SetActive(true);
            hp1.SetActive(true);
        }
        else if (playerHealth == 3)
        {
            hp5.SetActive(false);
            hp4.SetActive(false);
            hp3.SetActive(true);
            hp2.SetActive(true);
            hp1.SetActive(true);
        }
        else if (playerHealth == 2)
        {
            hp5.SetActive(false);
            hp4.SetActive(false);
            hp3.SetActive(false);
            hp2.SetActive(true);
            hp1.SetActive(true);
        }
        else if (playerHealth == 1)
        {
            hp5.SetActive(false);
            hp4.SetActive(false);
            hp3.SetActive(false);
            hp2.SetActive(false);
            hp1.SetActive(true);
        }
        else if (playerHealth <= 0)
        {
            hp1.SetActive(false);
            cam.GetComponent<AudioListener>().enabled = true;
            gameObject.SetActive(false);
            playerHealth = 5;
            text.text = "Press R to reload";
        }

        
    }
}

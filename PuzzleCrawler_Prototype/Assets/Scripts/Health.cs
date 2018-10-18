using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

    public static float playerHealth;
    [SerializeField] GameObject hp1;
    [SerializeField] GameObject hp2;
    [SerializeField] GameObject hp3;
    [SerializeField] GameObject hp4;
    [SerializeField] GameObject hp5;

    // Use this for initialization
    void Start () {
        playerHealth = 5;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            playerHealth = 5;
            hp5.SetActive(true);
            hp4.SetActive(true);
            hp3.SetActive(true);
            hp2.SetActive(true);
            hp1.SetActive(true);
        }

        if (playerHealth < 5)
        {
            hp5.SetActive(false);
        }

        if (playerHealth < 4)
        {
            hp4.SetActive(false);
        }

        if (playerHealth < 3)
        {
            hp3.SetActive(false);
        }

        if (playerHealth < 2)
        {
            hp2.SetActive(false);
        }

        if (playerHealth <= 0)
        {
            hp1.SetActive(false);
            gameObject.SetActive(false);
        }

        
    }
}

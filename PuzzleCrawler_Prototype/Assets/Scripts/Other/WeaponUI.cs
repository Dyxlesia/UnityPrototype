using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponUI : MonoBehaviour {

    [SerializeField] Sprite Wrench;
    [SerializeField] Sprite Screwdriver;
    [SerializeField] Sprite Hammer;


    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (PlayerController3.equipedTool == "wrench")
        {
            GetComponent<SpriteRenderer>().sprite = Wrench;
        }
        else if (PlayerController3.equipedTool == "screwdriver")
        {
            GetComponent<SpriteRenderer>().sprite = Screwdriver;
        }
        else if (PlayerController3.equipedTool == "hammer")
        {
            GetComponent<SpriteRenderer>().sprite = Hammer;
        }


    }
}

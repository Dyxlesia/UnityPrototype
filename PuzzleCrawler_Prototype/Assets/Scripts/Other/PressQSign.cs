using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressQSign : MonoBehaviour {

    [SerializeField] Sprite icon;
    [SerializeField] Sprite Q;
    [SerializeField] Sprite empty;
    [SerializeField] GameObject fillBox;

    public bool gettingFixed;

    float timer;

	// Use this for initialization
	void Start ()
    {
        timer = 0;
        gettingFixed = false;
        GetComponent<SpriteRenderer>().sprite = icon;

    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (gettingFixed)
        {
            if (!GetComponent<AudioSource>().isPlaying)
            {
                GetComponent<AudioSource>().Play();
            }

            GetComponent<SpriteRenderer>().sprite = empty;

            if (fillBox.transform.localScale.x < 1)
            {
                fillBox.transform.localScale += new Vector3(Time.deltaTime / 3, Time.deltaTime / 3, Time.deltaTime / 3);
            }

            return;
        }

        timer += Time.deltaTime;

        if (timer > 1)
        {
            if (GetComponent<SpriteRenderer>().sprite == icon)
            {
                GetComponent<SpriteRenderer>().sprite = Q;
            }
            else if (GetComponent<SpriteRenderer>().sprite == Q)
            {
                GetComponent<SpriteRenderer>().sprite = icon;
            }

            timer = 0;
        }

	}
}

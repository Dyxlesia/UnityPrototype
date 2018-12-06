using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour {

    //[SerializeField] AudioSource asAmbience;
    [SerializeField] AudioSource asAttack;
    [SerializeField] AudioSource asHit;
    [SerializeField] AudioSource asDash;

    [SerializeField] AudioClip dash1;
    [SerializeField] AudioClip dash2;
    [SerializeField] AudioClip dash3;

    [SerializeField] AudioClip wrench1;
    [SerializeField] AudioClip wrench2;
    [SerializeField] AudioClip wrench3;

    [SerializeField] AudioClip screw1;
    [SerializeField] AudioClip screw2;
    [SerializeField] AudioClip screw3;

    [SerializeField] AudioClip hammer1;
    [SerializeField] AudioClip hammer2;
    [SerializeField] AudioClip hammer3;


    //float rand;

    // Use this for initialization
    void Start () {
        //rand = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void playAttackSound(string tooltype)
    {
        float rand = Random.value;

        if (tooltype == "wrench")
        {
            if (rand < 0.33f)
            {
                asAttack.clip = wrench1;
                asAttack.Play();
            }
            else if (rand > 0.66f)
            {
                asAttack.clip = wrench2;
                asAttack.Play();
            }
            else
            {
                asAttack.clip = wrench3;
                asAttack.Play();
            }
        }
        else if (tooltype == "screwdriver")
        {
            if (rand < 0.33f)
            {
                asAttack.clip = screw1;
                asAttack.Play();
            }
            else if (rand > 0.66f)
            {
                asAttack.clip = screw2;
                asAttack.Play();
            }
            else
            {
                asAttack.clip = screw3;
                asAttack.Play();
            }
        }
        else if (tooltype == "hammer")
        {
            if (rand < 0.33f)
            {
                asAttack.clip = hammer1;
                asAttack.Play();
            }
            else if (rand > 0.66f)
            {
                asAttack.clip = hammer2;
                asAttack.Play();
            }
            else
            {
                asAttack.clip = hammer3;
                asAttack.Play();
            }
        }
    }

    public void playDashSound()
    {
        float rand = Random.value;

        if (rand < 0.5f)
        {
            //PlaySound.
            asDash.clip = dash1;
            asDash.Play();
}
        else if (rand >= 0.5f)
        {
            asDash.clip = dash2;
            asDash.Play();
        }
        else
        {
            //asDash.clip = dash3;
            //asDash.Play();
        }
    }

    public void playHitSound()
    {
        asHit.Play();
    }
}

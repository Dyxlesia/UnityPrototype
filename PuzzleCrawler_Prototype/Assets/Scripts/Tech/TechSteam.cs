using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechSteam : MonoBehaviour {

    [SerializeField] AudioClip start;
    [SerializeField] AudioClip loop;

    AudioSource audioSource;

    // Use this for initialization
    void Start () {
        audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {

        if (gameObject.GetComponent<MeshRenderer>().material.color == Color.green)
        {
            if (!audioSource.isPlaying && audioSource.clip == null)
            {
                audioSource.clip = start;
                audioSource.Play();
            }
            else if (!audioSource.isPlaying && audioSource.clip == start)
            {
                audioSource.clip = loop;
                audioSource.loop = true;
                audioSource.Play();
            }




            ParticleSystem ps = gameObject.GetComponent<ParticleSystem>();
            var em = ps.emission;
            em.enabled = true;

        }
	}
}

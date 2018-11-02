using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechSteam : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (gameObject.GetComponent<MeshRenderer>().material.color == Color.green)
        {
            ParticleSystem ps = gameObject.GetComponent<ParticleSystem>();
            var em = ps.emission;
            em.enabled = true;
        }
	}
}

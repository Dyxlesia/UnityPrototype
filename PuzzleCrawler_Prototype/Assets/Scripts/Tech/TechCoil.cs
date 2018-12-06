using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechCoil : MonoBehaviour {

    GameObject enemyToAttack;
    float closestDist;
    
	// Use this for initialization
	void Start () {
        closestDist = 10;
	}

    // Update is called once per frame
    void Update()
    {
        if (GetComponentInParent<MeshRenderer>().material.color != Color.green)
        {
            return;
        }

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");

        foreach (GameObject target in enemies)
        {
            float distance = Vector3.Distance(target.transform.position, transform.position);

            if (distance < closestDist)
            {
                enemyToAttack = target;

                closestDist = distance;
            }
        }

        if (enemyToAttack != null)
        {
            if (!GetComponent<AudioSource>().isPlaying)
            {
                GetComponent<AudioSource>().Play();
            }

            transform.LookAt(enemyToAttack.transform);
            ParticleSystem ps = gameObject.GetComponent<ParticleSystem>();

            ParticleSystem.MainModule psmain = ps.main;
            psmain.startLifetime = Vector3.Distance(enemyToAttack.transform.position, gameObject.transform.position) / 10;

            var em = ps.emission;
            em.enabled = true;

            if (Vector3.Distance(enemyToAttack.transform.position, gameObject.transform.position) > 10 || enemyToAttack.activeSelf == false)
            {
                em.enabled = false;
                GetComponent<AudioSource>().Stop();
                closestDist = 10;
            }
        }
        else
        {
            GetComponent<AudioSource>().Stop();
        }
    }
}

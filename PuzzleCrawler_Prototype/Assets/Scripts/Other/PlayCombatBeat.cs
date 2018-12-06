using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayCombatBeat : MonoBehaviour
{

    bool playBeat;
    AudioSource beat;

	// Use this for initialization
	void Start () {
        playBeat = false;
        beat = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        int numEnemiesNear = 0;

        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("enemy"))
        {
            if (Vector3.Distance(enemy.transform.position, gameObject.transform.position) < 19)
            {
                numEnemiesNear++;
                //print(enemy);
            }
        }

        print(numEnemiesNear);

        if (numEnemiesNear > 0)
        {
            playBeat = true;
        }
        else if (numEnemiesNear == 0)
        {
            playBeat = false;
        }

        if (playBeat)
        {

            if (beat.volume < 0.7f)
            {
                beat.volume += Time.deltaTime;
            }
            else if (beat.volume >= 0.7f)
            {
                beat.volume = 0.7f;
            }
        }
        else
        {
            if (beat.volume > 0)
            {
                beat.volume -= Time.deltaTime;
            }
            else if (beat.volume <= 0)
            {
                beat.volume = 0;
            }
        }

	}
}

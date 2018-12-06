using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

    [SerializeField] Text timer;

    public static int GlobalTimerValue;
    float timerIncrementing;

	// Use this for initialization
	void Start () {
        timerIncrementing = 0;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        timerIncrementing += Time.deltaTime;

        GlobalTimerValue = (int)timerIncrementing;
        timer.text = "" + (int)timerIncrementing;
	}
}

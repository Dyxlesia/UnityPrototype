using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboSlash : MonoBehaviour {

	public static int comboPoints;
	public static bool hitflag;
	[SerializeField] float comboTimer;
	[SerializeField] float slashMaxWidth;
	[SerializeField] float slahsMaxLength;

	float timer;

	// Use this for initialization
	void Start ()
	{
		comboPoints = 0;
		hitflag = false;
		timer = 0;
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		if (comboPoints != 0)
		{
			timer += Time.deltaTime;

			if (timer >= comboTimer)
			{
				comboPoints -= 1;
				timer = 0;
			}
		}

		if (comboPoints > 5)
		{
			comboPoints = 5;
		}

			gameObject.transform.localScale = new Vector3(0.9f + (ComboSlash.comboPoints * (slashMaxWidth / 5)), 0.9f + (ComboSlash.comboPoints * (slahsMaxLength / 5)), 1);


	}


}

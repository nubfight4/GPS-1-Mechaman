using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour 
{
	public LifeObject target;
	public Image[] healthDots;
	private Image bar;

	// For Mathf.Lerp
	private float apparentHPPercentage;
	private float actualHPPercentage;

	void Start ()
	{
		bar = GetComponent<Image> ();
		//Image = GameObject.FindGameObjectWithTag; 
		apparentHPPercentage = target.GetRemainingHPPercentage ();
	}

	void Update () 
	{
		actualHPPercentage = target.GetRemainingHPPercentage ();
		if (apparentHPPercentage != actualHPPercentage)
		{
			apparentHPPercentage = Mathf.Lerp (actualHPPercentage, apparentHPPercentage, 0.9f); // 100 -> 0, (0.9f) 100 -> 90 -> 81 -> 72.9 -> ... -> 0 ???
		}
			
		if(target != null)
		{
			bar.fillAmount = apparentHPPercentage / 100 ;
		}

		else
		{
			bar.fillAmount = 0;
		}
	}
}

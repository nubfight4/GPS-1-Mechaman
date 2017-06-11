using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoBar : MonoBehaviour {

	private Mecha target;
	private Image bar;

	void Start () {
		target = GameObject.FindGameObjectWithTag ("Player").GetComponent<Mecha> ();
		bar = GetComponent <Image> ();
	}

	void Update () {
		if (target != null)
			bar.fillAmount = target.GetAmmoAmountByPercentage ();
		else
			bar.fillAmount = 0;
	}
}

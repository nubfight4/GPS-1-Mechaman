﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP_Crate : MonoBehaviour {

	public int recovery = 50;
	private float lifeTime = 1.5f;

	void Start () {
		Destroy (this.gameObject, lifeTime);
	}

	void OnCollisionEnter2D (Collision2D target)
	{
		if (target.gameObject.CompareTag ("Player")) {
			target.gameObject.GetComponent<Mecha> ().RecoverHP (recovery);
			Destroy (this.gameObject);
		} else if (target.gameObject.CompareTag ("Enemy")) {
			target.gameObject.GetComponent<Goatzilla> ().RecoverHP (recovery);
			Destroy (this.gameObject);
		}
	}
}
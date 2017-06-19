using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage_Crate : MonoBehaviour {

	public int damage = 50;
	private float lifeTime = 1.5f;

	void Start () {
		Destroy (this.gameObject, lifeTime);
	}

	void OnCollisionEnter2D (Collision2D target)
	{
		if (target.gameObject.CompareTag ("Player")) {
			target.gameObject.GetComponent<Mecha> ().ReceiveDamage (damage);
			Destroy (this.gameObject);
		} else if (target.gameObject.CompareTag ("Enemy")) {
			target.gameObject.GetComponent<Goatzilla> ().ReceiveDamage (damage);
			Destroy (this.gameObject);
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mecha_Melee_Collider : MonoBehaviour {
	private int damage = 25;
	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.CompareTag ("Enemy")) {
			col.gameObject.GetComponent<Goatzilla> ().ReceiveDamage (damage);
		} 
	}
}

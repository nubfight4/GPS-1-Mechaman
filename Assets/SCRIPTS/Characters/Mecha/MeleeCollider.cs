﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeCollider : MonoBehaviour {
	public int damage;
	public Mecha_NEW mechaScript;

	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.gameObject.CompareTag("Enemy"))
		{
			damage = mechaScript.dMG;
			col.gameObject.GetComponent<LifeObject>().ReceiveDamage(damage);
		}
	}
}

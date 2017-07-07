using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeObject : MonoBehaviour {
	protected int maxHP;
	protected int HP;
	protected bool isAlive = true;

	public void CheckDeath ()
	{
		if (HP <= 0) {
			Destroy (this.gameObject);
			isAlive = false;
		}
	}

	public int GetMaxHP ()
	{
		return this.maxHP;
	}

	public void SetMaxHP (int value)
	{
		this.maxHP = value;
	}

	public int GetHP ()
	{
		return this.HP;
	}

	public void SetHP (int value)
	{
		this.HP = value;
	}

	public float GetRemainingHPPercentage ()
	{
		return GetHP () * 100f / GetMaxHP ();
	}

	public void ReceiveDamage (int value)
	{
		this.HP -= value;
	}

	public void Knockback(Vector3 knockbackDir, float knockbackPower)
	{
		Vector2 v2 = new Vector2 (knockbackDir.x, knockbackDir.y);
		Rigidbody2D rb = GetComponent<Rigidbody2D> ();
		rb.velocity = v2 * knockbackPower;
	}
}

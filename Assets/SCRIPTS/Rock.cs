using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour {

	public float knockbackPower = 8.0f;
	private Vector3 dir;
	//private float speed;
	private int damage;
	private float lifeTime = 1.5f;

	public void Initialize (LifeObject target, int damage, float speed) {
		//this.speed = speed;
		this.damage = damage;
		//dir = target.transform.position.normalized;
		lifeTime = 3.0f;
	}

	void Start () {
		Destroy (this.gameObject, lifeTime);
	}

	void Update () {
		//transform.Translate (dir * Time.deltaTime * speed);
	}

	void OnCollisionEnter2D (Collision2D target)
	{
		if (target.gameObject.CompareTag ("Player")) {
			Destroy (this.gameObject);
			target.gameObject.GetComponent<Mecha> ().ReceiveDamage (damage);
			//target.gameObject.GetComponent<Mecha> ().Knockback (dir, knockbackPower);
			dir = new Vector3(target.transform.position.x - transform.position.x, 1);
			target.gameObject.GetComponent<Mecha> ().Knockback (dir, knockbackPower);
		} else if (target.gameObject.CompareTag ("Enemy")) {
			Physics2D.IgnoreCollision (target.gameObject.GetComponent<Collider2D> (), GetComponent<Collider2D> ());
		}
	}
}

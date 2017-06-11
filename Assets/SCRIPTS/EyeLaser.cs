using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeLaser : MonoBehaviour {

	public float knockbackPower = 3.0f;
	private float radius;
	private float lifeTime = 3f;
	private float angularSpeed;
	private float pi = 3.1416f;
	private bool isDirectionFromLeft;
	private int damage = 100;

	void Start ()
	{
		radius = transform.localScale.x / 2;
		Destroy (this.gameObject, lifeTime);
		angularSpeed = 50 * Time.deltaTime / lifeTime;
	}

	void Update ()
	{
		transform.Translate (0, DegreeToRad (-angularSpeed * radius) + 0.3f * Time.deltaTime, 0);
		if (isDirectionFromLeft) {
			transform.Rotate (0, 0, angularSpeed);
		} else {
			transform.Rotate (0, 0, -angularSpeed);
		}
	}

	public void SetIsDirectionFromLeft (bool isDirectionFromLeft)
	{
		this.isDirectionFromLeft = isDirectionFromLeft;
	}

	private float DegreeToRad (float deg)
	{
		return deg * pi / 180f;
	}

	void OnCollisionEnter2D (Collision2D target)
	{
		if (target.gameObject.CompareTag ("Player")) {
			target.gameObject.GetComponent<Mecha> ().ReceiveDamage (damage);
			float knockbackDirX = (isDirectionFromLeft) ? -1 : 1;
			Vector3 dir = new Vector3 (knockbackDirX, 1);
			Physics2D.IgnoreCollision (target.gameObject.GetComponent<Collider2D> (), GetComponent<Collider2D> ());
			target.gameObject.GetComponent<Mecha> ().Knockback (dir, knockbackPower);
		} else if (target.gameObject.CompareTag ("Enemy")) {
			Physics2D.IgnoreCollision (target.gameObject.GetComponent<Collider2D> (), GetComponent<Collider2D> ());
		}
	}
}

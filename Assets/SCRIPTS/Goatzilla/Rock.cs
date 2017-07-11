//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//
//public class Rock : MonoBehaviour {
//
//	public int damage = 50;
//	public float throwSpeedFactor = 1.0f; // 0.0000 ~ 1.4999, higher the faster the speed of rock
//	private float lifeTime = 1.5f;
//	private Mecha target;
//	private Vector3 destination;
//	private Vector3 distance;
//	private Rigidbody2D rb2d;
//	private float timer;
//
//
//	private Vector3 totalDistanceFromTarget;
//	private Vector3 currentDistanceFromTarget;
//	private float speed;
//
//	void Start ()
//	{
//		Destroy (this.gameObject, lifeTime);
//		target = GameObject.FindGameObjectWithTag ("Player").GetComponent<Mecha> ();
//		destination = target.transform.position;
//		distance = GetDistance ();
//		// rb2d = GetComponent<Rigidbody2D> ();
//		// timer = 0;
//		totalDistanceFromTarget = GetDistanceFromTarget ();
//		speed = totalDistanceFromTarget.x / (lifeTime - throwSpeedFactor);
//	}
//
//	void Update()
//	{
//		// Vector2 dir = new Vector2 (distance.x * 1500, distance.y + 0.01f * distance.x);
//		// rb2d.AddForce (dir * 3);
//		currentDistanceFromTarget = GetDistanceFromTarget ();
//		transform.Translate ( Vector3.up * Time.deltaTime * ArcSpeedYFactor () );
//		transform.Translate ( Vector3.right * Time.deltaTime * speed );
//	}
//
//	void OnCollisionEnter2D (Collision2D target)
//	{
//		if (target.gameObject.CompareTag ("Player")) {
//			target.gameObject.GetComponent<Mecha> ().ReceiveDamage (damage);
//			Destroy (this.gameObject);
//		} else if (target.gameObject.CompareTag ("Enemy") || target.gameObject.CompareTag ("Ground")) {
//			Physics2D.IgnoreCollision (target.gameObject.GetComponent<Collider2D> (), GetComponent<Collider2D> ());
//			// damage = 0;
//		}
//	}
//
//	private Vector3 GetDistanceFromTarget ()
//	{
//		Vector3 d = new Vector3 (target.transform.position.x - this.transform.position.x, target.transform.position.y - this.transform.position.y);
//		return d;
//	}
//
//	private Vector3 GetDistance ()
//	{
//		return destination - transform.position;
//	}
//
//	private float ArcSpeedYFactor ()
//	{
//		float temp = currentDistanceFromTarget.x - totalDistanceFromTarget.x / 2;
//		return temp / totalDistanceFromTarget.x;
//	}
//}

	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class Rock : MonoBehaviour 
	{

		public int damage = 20;
		private float lifeTime = 1.5f;

		void Start ()
		{
			Destroy (this.gameObject, lifeTime);
		}

		void OnCollisionEnter2D (Collision2D target)
		{
			if (target.gameObject.CompareTag ("Player")) {
				target.gameObject.GetComponent<Mecha> ().ReceiveDamage (damage);
				Destroy (this.gameObject);
			} else if (target.gameObject.CompareTag ("Enemy") || target.gameObject.CompareTag ("Ground")) {
				Physics2D.IgnoreCollision (target.gameObject.GetComponent<Collider2D> (), GetComponent<Collider2D> ());
				damage = 0;
			}
		}
	}


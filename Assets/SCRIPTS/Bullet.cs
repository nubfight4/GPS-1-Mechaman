using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	public Vector3 direction;
    [SerializeField]
	float speed = 8.0f;
	float aliveTimer = 0.0f;
    [SerializeField]
    float aliveDuration = 1.5f;
    [SerializeField]
    public int damage = 200;

	public void Initialize (LifeObject target, int damage) {
		this.damage = damage;
	}

	void bulletMovement()
	{
		transform.Translate(direction * Time.deltaTime * speed);

		aliveTimer += Time.deltaTime;
		if(aliveTimer > aliveDuration)
		{
			Destroy(gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		bulletMovement ();
	}

	void OnCollisionEnter2D (Collision2D target)
	{
		if (target.gameObject.CompareTag ("Enemy")) {
			Destroy (this.gameObject);
			target.gameObject.GetComponent<Goatzilla> ().ReceiveDamage (damage);
		} 

		else if (target.gameObject.CompareTag ("Player")) {
			Physics2D.IgnoreCollision (target.gameObject.GetComponent<Collider2D> (), GetComponent<Collider2D> ());
		}
	}
}

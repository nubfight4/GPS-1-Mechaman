using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmbraceScript : MonoBehaviour {

	Goatzilla goatzilla;
	Mecha mecha;
	Sync_Attack syncAttack;

	//when collide and tag is "enemy"
	//find gameobject mech and goatzilla and use its transform for reference
	//stick goatzilla to mech transform adjusted value

	void Start () 
	{
		syncAttack = FindObjectOfType<Sync_Attack> (); //find object with this script
		mecha = FindObjectOfType<Mecha> (); //object
		goatzilla = FindObjectOfType<Goatzilla> ();
	}

	void Update () 
	{
		HugGoatzilla ();
	}

	public bool hugFromLeft;
	public bool hugFromRight;
	public bool stopMovement;

	void HugGoatzilla ()
	{
		if (isHugEnemy) {
			if (syncAttack.hasLoveEmbrace) {
				syncAttack.box2D.isTrigger = true;
				//goatzilla.box2d.isTrigger = true; //turn off collision
				goatzilla.rb2d.constraints = RigidbodyConstraints2D.FreezeAll; //immobilizes monster movement, can still attack
				if (!mecha.srender.flipX && !hugFromLeft) {
					mecha.transform.position = new Vector2 (goatzilla.transform.position.x - 1, goatzilla.transform.position.y);
					hugFromRight = true;
					stopMovement = true;
				}
				if (mecha.srender.flipX && !hugFromRight) {
					mecha.transform.position = new Vector2 (goatzilla.transform.position.x + 1, goatzilla.transform.position.y);
					hugFromLeft = true;
					stopMovement = true;
				}
				if (!takeEmbraceDamage) {
					mecha.ReceiveDamage (embraceSelfDamage);
					goatzilla.ReceiveDamage (embraceDamage);
					takeEmbraceDamage = true;
				}
			}
		}

		if (!syncAttack.hasLoveEmbrace || !isHugEnemy) {
			syncAttack.box2D.isTrigger = false;
			goatzilla.rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
		}
	}

	public bool isHugEnemy;

	public bool takeEmbraceDamage;
	public int embraceDamage = 1;
	public int embraceSelfDamage = 250; //find current hp and half it (UNSTABLE)

	void OnTriggerEnter2D (Collider2D target)
	{
		if (this.gameObject.tag == "Embrace Attack" && target.gameObject.tag == "Enemy") {
			isHugEnemy = true;
		}
	}
	void OnTriggerStay2D (Collider2D target)
	{
		if (this.gameObject.tag == "Embrace Attack" && target.gameObject.tag == "Enemy") {
			isHugEnemy = true;
		}
	}
	void OnTriggerExit2D (Collider2D target)
	{
		if (this.gameObject.tag == "Embrace Attack" && target.gameObject.tag == "Enemy") {
			isHugEnemy = false;
			hugFromLeft = false;
			hugFromRight = false;
		}
	}
}

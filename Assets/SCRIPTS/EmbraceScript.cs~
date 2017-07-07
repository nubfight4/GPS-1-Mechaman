using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmbraceScript : MonoBehaviour
{

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
		if (goatzilla != null) {
			HugGoatzilla ();
		}
	}

	public bool hugFromLeft;
	public bool hugFromRight;
	public bool stopMovement;

	//mech is looking at enemy
	//mech is at the right side looking at enemy

	void HugGoatzilla ()
	{
		if (syncAttack.hasLoveEmbrace && isHugEnemy) { //combo is active and player is within range
			syncAttack.box2D.isTrigger = true;
			mecha.tag = "Embrace Attack";
			//goatzilla.box2d.isTrigger = true; //turn off collision
			goatzilla.rb2d.constraints = RigidbodyConstraints2D.FreezeAll; //immobilizes monster movement, can still attack
			if (syncAttack.leftLoveEmbrace && !hugFromLeft && mecha.transform.position.x > goatzilla.transform.position.x) {
				mecha.transform.localScale = new Vector2 (-1f, 1f);
				mecha.transform.position = new Vector2 (goatzilla.transform.position.x + 1, goatzilla.transform.position.y); // LOOK LEFT FROM RIGHT
				hugFromRight = true;
				stopMovement = true;
			}
			if (syncAttack.rightLoveEmbrace && !hugFromRight && mecha.transform.position.x < goatzilla.transform.position.x) {
				mecha.transform.localScale = new Vector2 (1f, 1f);
				mecha.transform.position = new Vector2 (goatzilla.transform.position.x - 1, goatzilla.transform.position.y); // LOOK RIGHT FROM LEFT
				hugFromLeft = true;
				stopMovement = true;
			}
		}

		if (!syncAttack.hasLoveEmbrace) {
			syncAttack.box2D.isTrigger = false;
			stopMovement = false;
			hugFromLeft = false;
			hugFromRight = false;
			goatzilla.rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
		}
	}

	public bool isHugEnemy;

	void OnTriggerEnter2D (Collider2D target)
	{
		if (this.gameObject.tag == "Embrace Collider" && target.gameObject.tag == "Enemy") { //allows the embrace collider to detect collision between enemy
			isHugEnemy = true;
		}
	}

	void OnTriggerStay2D (Collider2D target)
	{
		if (this.gameObject.tag == "Embrace Collider" && target.gameObject.tag == "Enemy") {
			isHugEnemy = true;
		}
	}

	void OnTriggerExit2D (Collider2D target)
	{
		if (this.gameObject.tag == "Embrace Collider" && target.gameObject.tag == "Enemy") {
			isHugEnemy = false;
			hugFromLeft = false;
			hugFromRight = false;
		}
	}
}

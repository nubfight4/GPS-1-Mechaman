using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamageScript : MonoBehaviour
{

	//this is a general script that allows enemies to take damage from different types of prefabs with box colliders
	//this script is TAG specific in which it detects the tag in a prefab before activation
	//enemies will also be subjected to attack effects such as knockback

	Goatzilla goatzilla;
	GameObject goatzillaObject;
	Mecha mecha;
	GameObject mechaObject;

	bool canTakeDamage = false;
	public float hurtDuration = 100f;
	float hurtDurationCounter = 0f;

	//damage for attacks
	public int slashAttackDamage = 100;
	public int theElevatorDamage = 100;
	public int loveEmbraceDamage = 100;
	public int lowKickDamage = 100;
	public int specialRangeDamage = 100;
	public int shadowlessStrikeDamage = 100;
	public int meSmashDamage = 100;
	public int letsDanceDamage = 100;

	//time before enemy able to take damage again
	public float slashDurationGrace = 1000f;
	public float elevatorDurationGrace = 1000f;
	public float loveEmbraceDurationGrace = 3000f;
	public float lowKickDurationGrace = 1000f;
	public float specialRangeDurationGrace = 1000f;
	public float shadowlessStrikeDurationGrace = 50f;
	public float meSmashDurationGrace = 1000f;
	public float letsDanceDurationGrace = 3000f;

	//status for low kick
	public float lowKickKnockbackDuration = 100f;
	float lowKickKnockbackDurationCounter = 0f;
	public float knockbackForce = 10f;

	//time before the enemy stands back up
	public float timeToGetUp = 1000f;
	float timeToGetUpCounter = 0f;
	public bool isKnockback = false;
	public bool isTrip = false;
	public float lowKickKnockbackDamping = 0.8f;

	//the elevator status
	public bool isUpperCut = false;
	public bool isElevatorKnockback = false;
	public float elevatorKnockbackForce = 10f;
	public float elevatorKnockbackDuration = 100f;
	float elevatorKnockbackDurationCounter = 0f;
	public float elevatorKnockbackDamping = 0.5f;

	//special fist status
	public bool isSpecialFist = false;
	public bool isFistKnockback = false;
	public float fistKnockbackX = 100f;
	public float fistKnockbackY = 100f;
	public float fistKnockbackDamp = 0.5f;
	public float fistKnockbackDuration = 100f;
	float fistKnockbackDurationCounter = 0f;
	public float onGroundDuration = 1000f;
	float onGroundDurationCounter = 0f;

	//time for goatzilla to get up
	public float timeOnGround = 3000f;
	public float timeOnGroundCounter = 0f;
	public bool isToss = false;

	void Start ()
	{
		goatzilla = gameObject.GetComponent<Goatzilla> ();
		goatzillaObject = GameObject.Find ("Goatzilla");
		mecha = gameObject.GetComponent<Mecha> ();
		mechaObject = GameObject.Find ("Mecha 1");
	}

	void Update ()
	{
		if (canTakeDamage) {
			hurtDurationCounter += Time.deltaTime * 1000f;
			if (hurtDurationCounter > hurtDuration) {
				hurtDurationCounter = 0f;
				canTakeDamage = false;
			}
		}

		if (isTrip) {
			TripStatus ();
		}

		if (isUpperCut) {
			UpperCutStatus ();
		}

		if (isSpecialFist) {
			SpecialFistStatus ();
		}

		if (isToss) {
			LetsDanceStatus ();
		}
	}

	void TripStatus () //occurs from low kick, pushes and trips the enemy
	{
		if (!isKnockback) {
			if (goatzillaObject.transform.position.x > mechaObject.transform.position.x) { //enemy on right
				goatzilla.rb2d.velocity = new Vector2 ((knockbackForce) * Time.deltaTime, goatzilla.rb2d.velocity.y);
				//goatzilla.transform.Rotate (0f, 0f, 90f); //causes entity to rotate relative to existing rotation
				goatzilla.transform.eulerAngles = new Vector3 (0f, 0f, 90f);
			} else if (goatzillaObject.transform.position.x < mechaObject.transform.position.x) { //enemy on left
				goatzilla.rb2d.velocity = new Vector2 (-(knockbackForce) * Time.deltaTime, goatzilla.rb2d.velocity.y);
				goatzilla.transform.eulerAngles = new Vector3 (0f, 0f, -90f);
			}
			if (lowKickKnockbackDurationCounter <= lowKickKnockbackDuration) {
				lowKickKnockbackDurationCounter += Time.deltaTime * 1000f;
			} else {
				lowKickKnockbackDurationCounter = 0f;
				isKnockback = true;
			}
		} else if (isKnockback) {
			goatzilla.rb2d.velocity = new Vector2 (goatzilla.rb2d.velocity.x * lowKickKnockbackDamping, goatzilla.rb2d.velocity.y);
			if (timeToGetUpCounter <= timeToGetUp) {
				timeToGetUpCounter += Time.deltaTime * 1000f;
			} else {
				goatzilla.transform.position = new Vector2 (goatzilla.transform.position.x, goatzilla.transform.position.y + 1f); //prevents the enemy from glitching through floor
				goatzilla.transform.eulerAngles = new Vector3 (0f, 0f, 0f);
				timeToGetUpCounter = 0f;
				isKnockback = false;
				isTrip = false;
			}
		}
	}

	void UpperCutStatus () //knockback upwards
	{
		if (!isElevatorKnockback) {
			//goatzilla.rb2d.constraints = RigidbodyConstraints2D.FreezeRotation; //enemy fly up
			goatzilla.rb2d.velocity = new Vector2 (goatzilla.rb2d.velocity.x, elevatorKnockbackForce * Time.deltaTime);
			if (elevatorKnockbackDurationCounter <= elevatorKnockbackDuration) {
				elevatorKnockbackDurationCounter += Time.deltaTime * 1000f;
			} else {
				elevatorKnockbackDurationCounter = 0f;
				isElevatorKnockback = true;
			}
		} else if (isElevatorKnockback) {
			goatzilla.rb2d.velocity = new Vector2 (0f, goatzilla.rb2d.velocity.y * elevatorKnockbackDamping);
			isUpperCut = false;
			isElevatorKnockback = false;
		}
	}

	void SpecialFistStatus () //huge knockback
	{
		if (!isFistKnockback) {
			if (goatzillaObject.transform.position.x > mechaObject.transform.position.x) { //enemy on right
				goatzilla.rb2d.velocity = new Vector2 (fistKnockbackX * Time.deltaTime, fistKnockbackY * Time.deltaTime);
				goatzilla.transform.eulerAngles = new Vector3 (0f, 0f, 90f);
			} else if (goatzillaObject.transform.position.x < mechaObject.transform.position.x) { //enemy on left
				goatzilla.rb2d.velocity = new Vector2 (-fistKnockbackX * Time.deltaTime, fistKnockbackY * Time.deltaTime);
				goatzilla.transform.eulerAngles = new Vector3 (0f, 0f, -90f);
			}
			if (fistKnockbackDurationCounter <= fistKnockbackDuration) {
				fistKnockbackDurationCounter += Time.deltaTime * 1000f;
			} else {
				fistKnockbackDurationCounter = 0f;
				isFistKnockback = true;
			}
		} else if (isFistKnockback) {
			goatzilla.rb2d.velocity = new Vector2 (goatzilla.rb2d.velocity.x * fistKnockbackDamp, goatzilla.rb2d.velocity.y);
			if (onGroundDurationCounter <= onGroundDuration) {
				onGroundDurationCounter += Time.deltaTime * 1000f;
			} else {
				goatzilla.transform.position = new Vector2 (goatzilla.transform.position.x, goatzilla.transform.position.y + 1f);
				goatzilla.transform.eulerAngles = new Vector3 (0f, 0f, 0f);
				onGroundDurationCounter = 0f;
				isFistKnockback = false;
				isSpecialFist = false;
			}
		}
	}

	void LetsDanceStatus ()
	{
		if (timeOnGroundCounter <= timeOnGround) {
			timeOnGroundCounter += Time.deltaTime * 1000f;
		} else {
			goatzilla.transform.position = new Vector2 (goatzilla.transform.position.x, goatzilla.transform.position.y + 1f);
			goatzilla.transform.eulerAngles = new Vector3 (0f, 0f, 0f);
			timeOnGroundCounter = 0f;
			isToss = false;
		}
	}

	void OnCollisionEnter2D (Collision2D target) //check collision once
	{

	}

	void OnCollisionStay2D (Collision2D target) //constant check for collision
	{
		
	}

	void OnTriggerEnter2D (Collider2D target) //check trigger once
	{
		
	}

	void OnTriggerStay2D (Collider2D target) //constant check for collision
	{
		if (target.gameObject.tag == "Slash Attack" && this.gameObject.tag == "Enemy" && !canTakeDamage) {
			goatzilla.ReceiveDamage (slashAttackDamage);
			hurtDuration = slashDurationGrace;
			canTakeDamage = true;
		}
		if (target.gameObject.tag == "Elevator Attack" && this.gameObject.tag == "Enemy" && !canTakeDamage) {
			isUpperCut = true;
			goatzilla.ReceiveDamage (theElevatorDamage);
			hurtDuration = elevatorDurationGrace;
			canTakeDamage = true;
		}
		if (target.gameObject.tag == "Special Fist Attack" && this.gameObject.tag == "Enemy" && !canTakeDamage) {
			isSpecialFist = true;
			goatzilla.ReceiveDamage (specialRangeDamage);
			hurtDuration = specialRangeDurationGrace;
			canTakeDamage = true;
		}
		if (target.gameObject.tag == "Embrace Attack" && this.gameObject.tag == "Enemy" && !canTakeDamage) {
			goatzilla.ReceiveDamage (loveEmbraceDamage);
			hurtDuration = loveEmbraceDurationGrace;
			canTakeDamage = true;
		}
		if (target.gameObject.tag == "Low Kick Attack" && this.gameObject.tag == "Enemy" && !canTakeDamage) {
			isTrip = true;
			goatzilla.ReceiveDamage (lowKickDamage);
			hurtDuration = lowKickDurationGrace;
			canTakeDamage = true;
		}
		if (target.gameObject.tag == "Shadowless Attack" && this.gameObject.tag == "Enemy" && !canTakeDamage) {
			goatzilla.ReceiveDamage (shadowlessStrikeDamage);
			hurtDuration = shadowlessStrikeDurationGrace;
			canTakeDamage = true;
		}
		if (target.gameObject.tag == "Me Smash Attack" && this.gameObject.tag == "Enemy" && !canTakeDamage) {
			goatzilla.ReceiveDamage (meSmashDamage);
			hurtDuration = meSmashDurationGrace;
			canTakeDamage = true;
		}
		if (target.gameObject.tag == "Lets Dance Collider" && this.gameObject.tag == "Enemy" && !canTakeDamage) {
			isToss = true;
			goatzilla.ReceiveDamage (letsDanceDamage);
			hurtDuration = letsDanceDurationGrace;
			canTakeDamage = true;
		}
	}
}

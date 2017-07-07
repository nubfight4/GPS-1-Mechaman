using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetsDanceScript : MonoBehaviour
{

	Goatzilla goatzilla;
	Mecha mecha;
	Sync_Attack syncAttack;

	public bool isTossOnce = false;
	public bool isLetsDance = false;
	public float letsDanceColliderDuration = 1000f;
	public float letsDanceColliderDurationCounter = 0f;
	public float letsDanceCarryHeight = 3f;

	public float isDestroyColliderDuration = 1000f;
	public float isDestroyColliderDurationCounter = 0f;

	public float throwForceX = 300f;
	public float throwForceY = 300f;

	void Start ()
	{
		goatzilla = FindObjectOfType<Goatzilla> ();
		mecha = FindObjectOfType<Mecha> ();
		syncAttack = FindObjectOfType<Sync_Attack> ();
	}

	void Update ()
	{
		if (isLetsDance) {
			goatzilla.transform.position = new Vector2 (mecha.transform.position.x, mecha.transform.position.y + letsDanceCarryHeight);
			goatzilla.transform.eulerAngles = new Vector3 (0f, 0f, 90f);
			if (letsDanceColliderDurationCounter <= letsDanceColliderDuration) { //how long the enemy is held
				letsDanceColliderDurationCounter += Time.deltaTime * 1000f;
			} else {
				if (mecha.transform.localScale.x == 1) {
					goatzilla.rb2d.velocity = new Vector2 (throwForceX * Time.deltaTime, throwForceY * Time.deltaTime);
				} else if (mecha.transform.localScale.x == -1) {
					goatzilla.rb2d.velocity = new Vector2 (-throwForceX * Time.deltaTime, throwForceY * Time.deltaTime);
				}
				letsDanceColliderDurationCounter = 0f;
				isLetsDance = false;
			}
		}
		
		if (!isLetsDance) { //destroy collider after it has finished its function first!
			if (isDestroyColliderDurationCounter <= isDestroyColliderDuration) {
				isDestroyColliderDurationCounter += Time.deltaTime * 1000f;
			} else {
				isDestroyColliderDurationCounter = 0f;
				isTossOnce = false;
				Destroy (gameObject);
			}
		}
	}

	void OnCollisionStay2D (Collision2D target)
	{

	}

	void OnTriggerStay2D (Collider2D target)
	{
		if (target.gameObject.tag == "Enemy" && this.gameObject.tag == "Lets Dance Collider" && !isLetsDance && !isTossOnce) {
			Debug.Log ("LETS DANCE!");
			isLetsDance = true;
			isTossOnce = true;
		}
	}
}

/*
 * on collide, check if tag is "enemy"
 * find the goatzilla and mecha reference
 * use reference to change the transform and rotation of goatzilla relative to mecha
 * possibly change goatzilla into trigger collider to avoid glitches
 * after a duration add velocity to goatzilla based on where mecha is facing to simulate throwing
 * goatzilla returns to collision collider and lands on the ground for a duration
 * take damage upon landing
 * after a while the goatzilla stands up
 */

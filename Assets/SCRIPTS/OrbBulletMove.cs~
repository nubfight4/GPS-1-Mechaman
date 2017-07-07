using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbBulletMove : MonoBehaviour
{

	OrbShootingScript orbShoot;
	Sync_Attack syncAttack;

	public float bulletMoveSpeed = 5f;
	public float bulletDuration = 1000f;
	public float bulletDurationCounter = 0f;

	public bool reflectOnce = false;
	public float reflectDelay = 1000f;
	public float reflectDelayCounter = 0f;

	void Start ()
	{
		syncAttack = FindObjectOfType<Sync_Attack> ();
		orbShoot = gameObject.GetComponent<OrbShootingScript> ();
	}

	void Update ()
	{
		transform.Translate (Vector2.up * bulletMoveSpeed * Time.deltaTime);
		if (bulletDurationCounter <= bulletDuration) {
			bulletDurationCounter += Time.deltaTime * 1000f;
		} else {
			bulletDurationCounter = 0f;
			Destroy (gameObject);
		}

		if (reflectOnce) {
			if (reflectDelayCounter <= reflectDelay) {
				reflectDelayCounter += Time.deltaTime * 1000f;
			} else {
				reflectDelayCounter = 0f;
				reflectOnce = false;
			}
		}
	}

	void OnTriggerStay2D (Collider2D target)
	{
		if (target.gameObject.tag == "Shield Collider" && this.gameObject.tag == "Orb Bullet") {
			if (syncAttack.isReflectUpwards && !reflectOnce) {
				transform.eulerAngles = new Vector3 (0f, 0f, 0f);
				reflectOnce = true;
			} else if (syncAttack.isReflectDownwards && !reflectOnce) {
				transform.eulerAngles = new Vector3 (0f, 0f, 180f);
				reflectOnce = true;
			}
		}
	}
}

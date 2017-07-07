using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbShootingScript : MonoBehaviour
{

	public GameObject orbBullet;
	GameObject orbBulletClone;

	GameObject mechaObject;

	public bool canShoot = false;
	public float shootDuration = 5000f;
	public float shootDurationCounter = 0f;

	void Start ()
	{
		mechaObject = GameObject.Find ("Mecha 1");
	}

	void Update ()
	{
		if (!canShoot) {
			if (mechaObject.transform.position.x < transform.position.x) {
				orbBulletClone = Instantiate (orbBullet, transform.position, Quaternion.Euler (0f, 0f, 90f));
			} else if (mechaObject.transform.position.x > transform.position.x) {
				orbBulletClone = Instantiate (orbBullet, transform.position, Quaternion.Euler (0f, 0f, -90f));
			}
			canShoot = true;
		} else if (canShoot) {
			if (shootDurationCounter <= shootDuration) {
				shootDurationCounter += Time.deltaTime * 1000f;
			} else {
				shootDurationCounter = 0f;
				canShoot = false;
			}
		}
	}
}

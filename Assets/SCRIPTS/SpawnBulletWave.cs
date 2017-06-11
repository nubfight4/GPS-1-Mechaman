using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBulletWave : MonoBehaviour {

	public GameObject Bullet;
	GameObject bulletClone;

	void Start () {

		for (int i = 0; i <= 5; i++)
		{
			bulletClone = Instantiate (Bullet,transform.position,Quaternion.identity) as GameObject;
		}
	}

	void Update () {
		
	}
}

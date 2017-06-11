﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformScript : MonoBehaviour {

	private Mecha player;
	private bool jumpDown;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Mecha> ();
//		jumpDown = player.canJumpDownPlatform;
	}

	// Update is called once per frame
	void Update () {
		Vector3 playerDir = player.transform.position - transform.position;
		if (playerDir.y -1 > 0) {
			GetComponent<BoxCollider2D> ().isTrigger = false;
		} 

		else if (playerDir.y -1 < 0 || jumpDown) {
			GetComponent<BoxCollider2D> ().isTrigger = true;
		}

	}


}


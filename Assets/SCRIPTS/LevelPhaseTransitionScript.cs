using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPhaseTransition : MonoBehaviour {

	private Player_Camera camera;
	private Goatzilla enemy;


	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if (enemy.isEnraged == true) {
			camera.Phase2 = true;
			Destroy (this.gameObject);
		}
	}
}

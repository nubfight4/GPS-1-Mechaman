using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Camera : MonoBehaviour {

    public Transform mecha;
    public float yOffset;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (mecha != null) {
			transform.position = new Vector3 (mecha.position.x, mecha.position.y + yOffset, transform.position.z);
		}
	}//syntax/level size border
}
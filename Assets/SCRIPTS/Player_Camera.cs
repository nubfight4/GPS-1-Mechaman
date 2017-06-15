using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Camera : MonoBehaviour {

	public Transform mecha;

	public bool CameraBoundary;
	public bool Phase2 = false;

	public Vector2 minCameraPos, maxCameraPos;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void LateUpdate() {

		if (Phase2 == true) {
			if (mecha != null) {
				transform.position = new Vector3(mecha.position.x, transform.position.y, transform.position.z);
			}

			if (CameraBoundary)
			{
				transform.position = 
					new Vector3(Mathf.Clamp(mecha.position.x, minCameraPos.x, maxCameraPos.x),
						Mathf.Clamp(transform.position.y, minCameraPos.y, maxCameraPos.y),
						transform.position.z);
			}
		}
		}

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class something
{
	public int number;
	public string name;
	public float stuff;
}

public class Mecha_NEW : MonoBehaviour {

	Vector3 gamepadPos;
	public List<something> check = new List<something>();
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		gamepadPos.x = Input.GetAxis ("Horizontal");
		gamepadPos.y = Input.GetAxis ("Vertical");
		transform.position = gamepadPos + transform.position;
	
		if (Input.GetKeyDown (KeyCode.Joystick1Button2)) 
		{ 
			Debug.Log ("Normal Attack!");
		}

		if (Input.GetKeyDown (KeyCode.Joystick1Button0)) 
		{
			Debug.Log ("Heavy Attack!");
		}

		if (Input.GetKeyDown (KeyCode.Joystick1Button1)) 
		{
			Debug.Log ("Block!");
		}
	}
}

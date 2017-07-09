using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Mecha_NEW : MonoBehaviour {
	Vector3 gamepadPos;
	Vector3 minPos;
	Vector3 maxPos;

	//Temporary
	private Animator anim;
	bool isJumping;
	bool isStop;

	// Use this for initialization
	void Start () {
		isJumping = false; 
		isStop = false;
		maxPos.x = 2.55f;
		maxPos.y = -1.0f;
		minPos.x = -8.85f;
		minPos.y = -2.25f;
		anim = GetComponent<Animator> ();
		
	}
	
	// Update is called once per frame
	void Update () {
		Boundary ();
		UpdateAnimator ();
		gamepadPos.x = Input.GetAxis ("Horizontal");
		gamepadPos.y = Input.GetAxis ("Vertical");
		transform.position = gamepadPos + transform.position;

		if (gamepadPos.x == 0) {
			isStop = true;
		} else {
			isStop = false;
		}
	
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
		/*
		if (Input.GetKeyDown (KeyCode.Joystick1Button3) && !isJumping) 
		{
			//supposed to put a timer here on how long player can jump
			isJumping = true;
			Debug.Log ("Jump!");
		}
*/
		if (Input.GetKeyUp (KeyCode.Joystick1Button3) && isJumping) 
		{
			isJumping = false;
		}
	}

	void Boundary() {

		if (transform.position.x > maxPos.x) 
		{
			transform.position = new Vector3 (maxPos.x,transform.position.y);
		}

		if (transform.position.x < minPos.x) 
		{
			transform.position = new Vector3 (minPos.x,transform.position.y);
		}

		if (transform.position.y > maxPos.y) 
		{
			transform.position = new Vector3 (transform.position.x,maxPos.y);
		}

		if (transform.position.y < minPos.y) 
		{
			transform.position = new Vector3 (transform.position.x,minPos.y);
		}
	}

	void UpdateAnimator(){
		anim.SetFloat ("Speed", gamepadPos.x);
		anim.SetBool ("isJumping", isJumping);
		anim.SetBool ("isStop", isStop);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Mecha_NEW : MonoBehaviour {
	public Vector3 gamepadPos;
	Vector3 minPos;
	Vector3 maxPos;


	private Animator anim;
	private SpriteRenderer sprite;
	bool isJumping;
	bool isStop;
	//! Combo and Syncgronise attack
	const int MAX_COMBOCOUNT = 5;
	bool[] attacks;
	bool p1Pressed;
	bool p2Pressed;
	int timePressedNormal;
	int timePressedHeavy;
		//reset
	bool startReset;
	float resetTimer;
	float resetDuration;
		//prevent
	bool isOtherCombo;
	bool isJumpPunching;

	// Use this for initialization
	void Start () {
		isJumping = false; 
		isStop = false;
		maxPos.x = 1.4f;
		//maxPos.y = -1.0f;
		minPos.x = -7.8f;
		//minPos.y = -2.25f;
		anim = GetComponent<Animator> ();
		attacks = new bool[MAX_COMBOCOUNT];
		isOtherCombo = false;
		isJumpPunching = false;
		startReset = false;
		p1Pressed = false;
		p2Pressed = false;
		timePressedNormal = 0;
		timePressedHeavy = 0;
		resetTimer = 0f;
		resetDuration = 0.5f;
	}
	
	// Update is called once per frame
	void Update () {
		Boundary ();
		UpdateAnimator ();
		gamepadPos.x = Input.GetAxis ("Horizontal");
		//gamepadPos.y = Input.GetAxis ("Vertical");
		transform.position = gamepadPos + transform.position;
		Movement();
		Combo();

		/* check left+right together
		if (Input.GetButtonDown("Bumper_Right_P1")) 
		{
			Debug.Log ("P1 RightBumper!");
		}
		if (Input.GetButtonDown("Bumper_Left_P1")) 
		{
			Debug.Log ("P1 LeftBumper!");
		}
		if (Input.GetButtonDown("Bumper_Right_P2")) 
		{
			Debug.Log ("P2 RightBumper!");
		}
		if (Input.GetButtonDown("Bumper_Left_P2")) 
		{
			Debug.Log ("P2 LeftBumper!");
		}*/

//		if (Input.GetKeyDown (KeyCode.Joystick1Button1)) 
//		{
//			Debug.Log ("Block!");
//		}
//
//		if (Input.GetKeyDown (KeyCode.Joystick1Button3) && !isJumping) 
//		{
//			//supposed to put a timer here on how long player can jump
//			isJumping = true;
//			Debug.Log ("Jump!");
//		}
//
//		if (Input.GetKeyUp (KeyCode.Joystick1Button3) && isJumping) 
//		{
//			isJumping = false;
//		}
	}

	void Movement()
	{
		gamepadPos.x = Input.GetAxis ("Horizontal");
		//gamepadPos.y = Input.GetAxis ("Vertical");
		transform.position = gamepadPos + transform.position;

		if (gamepadPos.x < -0.05) 
		{
			transform.localScale = new Vector3 (-1, transform.localScale.y);
		}
		if (gamepadPos.x > 0.05) 
		{
			transform.localScale = new Vector3 (1, transform.localScale.y);	
		}
	}

	void Boundary() 
	{

		if (transform.position.x > maxPos.x) 
		{
			transform.position = new Vector3 (maxPos.x,transform.position.y);
		}

		if (transform.position.x < minPos.x) 
		{
			transform.position = new Vector3 (minPos.x,transform.position.y);
		}

//		if (transform.position.y > maxPos.y) 
//		{
//			transform.position = new Vector3 (transform.position.x,maxPos.y);
//		}
//
//		if (transform.position.y < minPos.y) 
//		{
//			transform.position = new Vector3 (transform.position.x,minPos.y);
//		}
	}
	void UpdateAnimator(){
		anim.SetFloat ("Speed", gamepadPos.x);
		anim.SetBool ("isJumping", isJumping);
		//anim.SetBool ("isStop", isStop);
	}

	void Combo()
	{
		if (gamepadPos.x == 0) {
			isStop = true;
		} else {
			isStop = false;
		}
		//delay
		if(startReset)
		{
			resetTimer += Time.deltaTime;
			if(resetTimer >= resetDuration)
			{
				resetTimer = 0;
				timePressedNormal = 0;
				timePressedHeavy = 0;
				startReset = false;
				isOtherCombo = false;
				isJumpPunching = false;
				p1Pressed = false;
				p2Pressed = false;
			}
		}
		//Jump Punch
		if(Input.GetAxis("Vertical") > 0)
		{
			if(Input.GetButtonDown("Normal Attack") && timePressedNormal == 0)
			{
				Debug.Log("JumpPunch");
				timePressedNormal++;
				startReset = true;
				isOtherCombo = true;
				isJumpPunching = true;
			}
		}

		//Dash punch
		if(gamepadPos.x > 0)
		{
			if(Input.GetButtonDown("Heavy Attack") && isJumpPunching == false && timePressedHeavy == 0)
			{
				Debug.Log("Dash attack right");
				timePressedHeavy++;
				startReset = true;
				isOtherCombo = true;
			}
		}
		else if(gamepadPos.x < 0)
		{
			if(Input.GetButtonDown("Heavy Attack") && isJumpPunching == false && timePressedHeavy == 0)
			{
				Debug.Log("Dash attack left");
				timePressedHeavy++;
				startReset = true;
				isOtherCombo = true;
			}
		}

		//normal combo
		if (Input.GetButtonDown("Normal Attack")) 
		{ 
			if(!isOtherCombo)
			{
				startReset = true;
				timePressedHeavy = 0;
				if(timePressedNormal == 0)
				{
					Debug.Log ("Normal Attack!");
					resetTimer = 0;
				}
				else if(timePressedNormal == 1)
				{
					Debug.Log ("Normal normal Attack!");
					resetTimer = 0;
				}
				else if(timePressedNormal == 2)
				{
					Debug.Log ("Normal normal normal Attack!");
					resetTimer = 0;
				}
				timePressedNormal++;
				if(timePressedNormal >= 3)
				{
					timePressedNormal = 0;
				}	
			}
		}
		//heavy combo
		if (Input.GetButtonDown("Heavy Attack")) 
		{
			if(!isOtherCombo)
			{
				startReset = true;
				timePressedNormal = 0;
				if(timePressedHeavy == 0)
				{
					Debug.Log ("Heavy Attack!");
					resetTimer = 0;
				}
				else if(timePressedHeavy == 1)
				{
					Debug.Log ("HEAAAAAVYYYY Attack!");
					resetTimer = 0;
				}
				timePressedHeavy++;
				if(timePressedHeavy >= 2)
				{
					timePressedNormal = 0;
				}
			}
		}

		//ULtimate
		if(Input.GetButtonDown("Bumper_Left_P1") && Input.GetButtonDown("Bumper_Right_P1"))
		{
			startReset = true;
			p1Pressed = true;
			Debug.Log("HAHAHAHA P1");
		}

		if(Input.GetButtonDown("Bumper_Left_P2") && Input.GetButtonDown("Bumper_Right_P2"))
		{
			startReset = true;
			p2Pressed = true;
			Debug.Log("HAHAHAHA P2");
		}

		if(p1Pressed == true && p2Pressed == true)
		{
			Debug.Log("UltimateGG");
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mecha_NEW : MonoBehaviour {
	public enum STATE
	{
		IDLE = 0,
		WALKING,
		PUNCH,
		WHATSUP,
		SHADOW,
		HEAVY,
		DOUBLETROUBLE,
		TOTAL
	};
	public Vector3 gamepadPos;
	Vector3 minPos;
	Vector3 maxPos;
	private Animator anim;
	private SpriteRenderer sprite;
	int state;
	bool isJumping;
	bool isStop;
	//! Combo and Syncgronise attack
	bool p1LPressed;
	bool p1RPressed;
	bool p2LPressed;
	bool p2RPressed;
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
		anim = GetComponent<Animator>();
		state = 0;
		isJumping = false; 
		isStop = false;
		maxPos.x = 1.4f;
		//maxPos.y = -1.0f;
		minPos.x = -7.8f;
		//minPos.y = -2.25f;
		anim = GetComponent<Animator> ();
		isOtherCombo = false;
		isJumpPunching = false;
		startReset = false;
		p1LPressed = false;
		p1RPressed = false;
		p2LPressed = false;
		p2RPressed = false;
		timePressedNormal = 0;
		timePressedHeavy = 0;
		resetTimer = 0f;
		resetDuration = 0.7f;
	}

	// Update is called once per frame
	void Update () {
		Debug.Log(state);
		Boundary ();
		gamepadPos.x = Input.GetAxis ("Horizontal");
		//gamepadPos.y = Input.GetAxis ("Vertical");
		transform.position = gamepadPos + transform.position;
		Movement();
		Combo();
		UpdateAnimator();

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
		//anim.SetFloat ("Speed", gamepadPos.x);
		//anim.SetBool ("isJumping", isJumping);
		//anim.SetBool ("isStop", isStop);
		anim.SetInteger("State", state);
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
				p1LPressed = false;
				p1RPressed = false;
				p2LPressed = false;
				p2RPressed = false;
				resetDuration = 0.7f;
				state = (int)STATE.IDLE;
			}
		}
		//Jump Punch
		/*if(Input.GetAxis("Vertical") > 0)
		{
			if(Input.GetButtonDown("Normal Attack") && timePressedNormal == 0)
			{
				Debug.Log("JumpPunch");
				timePressedNormal++;
				startReset = true;
				isOtherCombo = true;
				isJumpPunching = true;
			}
		}*/

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
		if(timePressedNormal < 4)
		{
			if (Input.GetButtonDown("Normal Attack")) 
			{ 
				if(!isOtherCombo)
				{
					startReset = true;
					timePressedHeavy = 0;
					if(timePressedNormal == 0)
					{
						state = (int)STATE.PUNCH;
						resetTimer = 0;
						timePressedNormal++;
					}
					else if(timePressedNormal == 1)
					{
						state = (int)STATE.WHATSUP;
						resetTimer = 0;
						timePressedNormal++;
					}
					else if(timePressedNormal == 2)
					{
						//resetTimer = 0;
						state = (int)STATE.SHADOW;
						Debug.Log("WTF");
						Debug.Log(timePressedNormal);
					}
				}
			}
		}

		if(timePressedNormal >= 3)
		{
			resetTimer = resetDuration;
			Debug.Log("OVER =3");
			timePressedNormal = 0;
			state = (int)STATE.IDLE;
		}

		//heavy combo
		if (Input.GetButtonDown("Heavy Attack")) 
		{
			if(!isOtherCombo)
			{
				timePressedNormal = 0;
				startReset = true;
				if(timePressedHeavy < 2)
				{
					if(timePressedHeavy == 0)
					{
						state = (int)STATE.HEAVY;
					}
					else if(timePressedHeavy >= 1)
					{
						state = (int)STATE.DOUBLETROUBLE;
					}
					resetTimer = 0;
					timePressedHeavy++;
				}
				else
				{
					resetTimer = resetDuration;
					timePressedHeavy = 0;
					state = (int)STATE.IDLE;
				}
			}
		}

		//ULtimate
		if(Input.GetButtonDown("Bumper_Left_P1"))
		{
			startReset = true;
			p1LPressed = true;
		}
		if(Input.GetButtonDown("Bumper_Right_P1"))
		{
			startReset = true;
			p1RPressed = true;
		}
		if(Input.GetButtonDown("Bumper_Left_P2"))
		{
			startReset = true;
			p2LPressed = true;
		}
		if(Input.GetButtonDown("Bumper_Right_P2"))
		{
			startReset = true;
			p2RPressed = true;
		}

		if(p1LPressed == true && p1RPressed == true && p2LPressed == true && p2RPressed == true)
		{
			Debug.Log("UltimateGG");
		}
	}
}

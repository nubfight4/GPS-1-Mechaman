using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms;
using UnityEngine.Networking.Match;

public class Sync_Attack : MonoBehaviour {

	public Rigidbody2D rb2d;
	public SpriteRenderer srender;
	public Image image;
	public Mecha mecha;
	public BoxCollider2D box2D;

	public GameObject comboGUI; //to show the button press
	public GameObject slashEffectPrefab;
	public GameObject Bullet;
	public GameObject cyanEffect;
	public GameObject specialAttackEmpty;
	public GameObject lowKickPrefab;

	public GameObject imageW;
	public GameObject imageA;
	public GameObject imageS;
	public GameObject imageD;
	public GameObject imageLMB;
	public GameObject imageRMB;
	public GameObject imageExplode;

	public GameObject specialAttackEmptyClone;
	public GameObject bulletClone;
	public GameObject cyanEffectClone;
	public GameObject slashEffectPrefabClone;
	public GameObject lowKickPrefabClone;

	//use time.deltaTime * 1000, 1000ms = 1s, 500ms = 0.5s

	public float comboDuration = 1000f; //time taken before the combo resets
	public float comboTimer; //countdown timer for combo

	//all possible combo buttons based on player controls, checks how many times the buttons has been pressed
	public int isUp;
	public int isDown;
	public int isLeft;
	public int isRight;
	public int isMelee;
	public int isShoot;
	public int isBlock;
	public float isHoldUp;
	public float isHoldDown;
	public float isHoldLeft;
	public float isHoldRight;
	public float isHoldMelee;
	public float isHoldShoot;
	public float isHoldBlock;

	public string ComboAttackIndicator = ""; //use this to compare with other combos
	public string pressUp = "up"; //string allows sequential combos easier
	public string pressDown = "down";
	public string pressLeft = "left";
	public string pressRight = "right";
	public string pressShoot = "shoot";
	public string pressBlock = "block";
	public string pressMelee = "melee";

	public bool isKeyPress; //checks whether player has pressed a key before activating timer
	public bool isUltimateMode;
	public bool isSpecialAttack;
	public bool createCyan;
	public bool createSlashRightEffect;
	public bool createSlashLeftEffect;

	public float dashPower = 50f; //dash slash attack
	public float dashDuration = 200f;
	public float dashCounter;
	public float dashCooldown = 1000f;
	public float dashCooldownCounter;
	public float dashHoldDuration = 100f;
	public float dashGracePeriod = 200f;
	public bool canSlashDash;
	public bool isRightSlash;
	public bool isLeftSlash;
	public bool canGainCharge;

	public float specialAttackTimer = 3000f; //time before special attack expires
	public float specialAttackCounter;
	public float specialDelayPress = 300f; //delay between button presses
	public float specialDelayCounter;
	public bool activateSpecialCounter;
	public int correctSpecialPress;

	public int randomN; //random number generator
	public bool isRandom;

	public float currentCharge = 0f;
	public float maxCharge = 100f;
	public float ultimateCharge = 40f;
	public float specialCharge = 60f;

	public bool isHitEnemy; //check to hit enemy

	void Start () 
	{
		rb2d = gameObject.GetComponent<Rigidbody2D> ();
		srender = gameObject.GetComponent<SpriteRenderer> ();
		mecha = gameObject.GetComponent<Mecha> ();
		box2D = gameObject.GetComponent<BoxCollider2D> ();
		image = gameObject.GetComponent<Image> ();

		HideSpecialButtons ();
		//bulletClone = Instantiate (Bullet,transform.position,Quaternion.identity) as GameObject;
	}

	void Update () 
	{
		KeyPress ();
	}

	void FixedUpdate ()
	{
		SpecialAttack ();
		UltimateMode ();
		SyncCombos ();
		ResetComboTimer ();
		DashCooldown ();

		//StringInputRegister ();
		//StringAttackFunction ();
	}

	/*
	public string StringAttack = "updownleftright"; //possible combinations for string attack
	public string StringAttack2 = "downupleftright";
	*/

	/*
	void StringInputRegister ()
	{
		if(Input.GetButtonDown ("Vertical") && Input.GetAxis ("Vertical") > 0)
		{
			ComboAttackIndicator += pressUp;
		}
		if(Input.GetButtonDown ("Vertical") && Input.GetAxis ("Vertical") < 0)
		{
			ComboAttackIndicator += pressDown;
		}
		if(Input.GetButtonDown ("Horizontal") && Input.GetAxis ("Horizontal") < 0)
		{
			ComboAttackIndicator += pressLeft;
		}
		if(Input.GetButtonDown ("Horizontal") && Input.GetAxis ("Horizontal") > 0)
		{
			ComboAttackIndicator += pressRight;
		}
	}

	void StringAttackFunction ()
	{
		if (ComboAttackIndicator == "updown" || ComboAttackIndicator == "downup") {
			ComboAttackIndicator +
		}

		if(ComboAttackIndicator == StringAttack || ComboAttackIndicator == StringAttack2)
		{
			Debug.Log ("STRING ATTACK!");
			ComboAttackIndicator = "";
		}
	}
	*/

	void HideSpecialButtons()
	{
		imageW.SetActive (false); //make them invisible on game start, make them visible when required
		imageA.SetActive (false);
		imageS.SetActive (false);
		imageD.SetActive (false);
		imageLMB.SetActive (false);
		imageRMB.SetActive (false);
		imageExplode.SetActive (false);
	}

	void DashCooldown()
	{
		if (dashCounter > dashDuration) { //dash is executed
			dashCounter = dashDuration;
			dashCooldownCounter += Time.deltaTime * 1000;
			if (!canGainCharge) {
				currentCharge += 30f; //mech gain charge after executing skill
				canGainCharge = true;
			}
		} 
		if (dashCooldownCounter > dashCooldown) {
			Destroy (slashEffectPrefabClone);
			dashCounter = 0f;
			dashCooldownCounter = 0f;
			canSlashDash = false;
			isRightSlash = false;
			isLeftSlash = false;
			canGainCharge = false;
		}
	}

	void KeyPress()
	{
		//tap button input
		if (Input.GetButtonDown ("Vertical") && Input.GetAxis ("Vertical") > 0) {
			ComboAttackIndicator += pressUp;
			isKeyPress = true;
			isUp++;
			if (isSpecialAttack && randomN == 0) {
				imageExplode.SetActive (true); //special fx
				imageW.SetActive (false);
				activateSpecialCounter = true;
				correctSpecialPress++;
			}
		}
		if (Input.GetButtonDown ("Horizontal") && Input.GetAxis ("Horizontal") < 0) {
			ComboAttackIndicator += pressLeft;
			isKeyPress = true;
			isLeft++;
			if (isSpecialAttack && randomN == 1) {
				imageExplode.SetActive (true);
				imageA.SetActive (false);
				activateSpecialCounter = true;
				correctSpecialPress++;
			}
		}
		if (Input.GetButtonDown ("Vertical") && Input.GetAxis ("Vertical") < 0) {
			ComboAttackIndicator += pressDown;
			isKeyPress = true;
			isDown++;
			if (isSpecialAttack && randomN == 2) {
				imageExplode.SetActive (true);
				imageS.SetActive (false);
				activateSpecialCounter = true;
				correctSpecialPress++;
			}
		}
		if (Input.GetButtonDown ("Horizontal") && Input.GetAxis ("Horizontal") > 0) {
			ComboAttackIndicator += pressRight;
			isKeyPress = true;
			isRight++;
			if (isSpecialAttack && randomN == 3) {
				imageExplode.SetActive (true);
				imageD.SetActive (false);
				activateSpecialCounter = true;
				correctSpecialPress++;
			}
		}
		if (Input.GetButtonDown ("Melee")) {
			ComboAttackIndicator += pressMelee;
			isKeyPress = true;
			isMelee++;
		}
		if (Input.GetMouseButtonDown (0)) {
			ComboAttackIndicator += pressShoot;
			isKeyPress = true;
			isShoot++;
			if (isSpecialAttack && randomN == 4) {
				imageExplode.SetActive (true);
				imageLMB.SetActive (false);
				activateSpecialCounter = true;
				correctSpecialPress++;
			}
		}
		if (Input.GetMouseButtonDown (1)) {
			ComboAttackIndicator += pressBlock;
			isKeyPress = true;
			isBlock++;
			if (isSpecialAttack && randomN == 5) {
				imageExplode.SetActive (true);
				imageRMB.SetActive (false);
				activateSpecialCounter = true;
				correctSpecialPress++;
			}
		}

		//hold button input
		if (Input.GetButton ("Vertical") && Input.GetAxis ("Vertical") > 0) {
			isKeyPress = true;
			isHoldUp += Time.deltaTime * 1000;
		} else {
			isHoldUp = 0f; //counter resets if not holding down
		}
		if (Input.GetButton ("Horizontal") && Input.GetAxis ("Horizontal") < 0) {
			isKeyPress = true;
			isHoldLeft += Time.deltaTime * 1000;
		} else {
			isHoldLeft = 0f;
		}
		if (Input.GetButton ("Vertical") && Input.GetAxis ("Vertical") < 0) {
			isKeyPress = true;
			isHoldDown += Time.deltaTime * 1000;
		} else {
			isHoldDown = 0f;
		}
		if (Input.GetButton ("Horizontal") && Input.GetAxis ("Horizontal") > 0) {
			isKeyPress = true;
			isHoldRight += Time.deltaTime * 1000;
		} else {
			isHoldRight = 0f;
		}
		if (Input.GetButtonDown ("Melee")) {
			isKeyPress = true;
			isHoldMelee += Time.deltaTime * 1000;
		} else {
			isHoldMelee = 0f;
		}
		if (Input.GetMouseButton (0)) {
			isKeyPress = true;
			isHoldShoot += Time.deltaTime * 1000;
		} else {
			isHoldShoot = 0f;
		}
		if (Input.GetMouseButton (1)) {
			isKeyPress = true;
			isHoldBlock += Time.deltaTime * 1000;
		} else {
			isHoldBlock = 0f;
		}
	}

	void SyncCombos()
	{
		SlashDash ();
		LowKick ();
		ShadowlessStrike ();
		//SequenceAttack ();

		if (currentCharge >= maxCharge) { //charge limit
			currentCharge = maxCharge;
		}
	}

	void UltimateMode()
	{
		 //achieve ultimate mode first, then use special attack (NOT DONE)
		if (currentCharge >= ultimateCharge) {
			isUltimateMode = true;
		}
		if (currentCharge < ultimateCharge) {
			isUltimateMode = false;
		}

		if (isUltimateMode) {
			srender.color = Color.green; //something cool happens in ultimate mode, colour green for now

			if (!createCyan) { //special FX
				cyanEffectClone = Instantiate (cyanEffect, transform.position, Quaternion.identity);
				cyanEffectClone.transform.parent = gameObject.transform;
				createCyan = true;
			}
		}

		if (!isUltimateMode) {
			createCyan = false;
			Destroy (cyanEffectClone);
		}
	}

	void resetUltimate()
	{
		
	}

	void SpecialAttack()
	{
		if (isUltimateMode) {
			if (Input.GetButtonDown ("Special Attack")) {
				if (currentCharge >= specialCharge) {
					isSpecialAttack = true;
				}
			}
		}

		if (isSpecialAttack) {
			
			srender.color = Color.black; //make a UI that require player to follow button press, colour black for now
			specialAttackCounter += Time.deltaTime * 1000;;

			if (specialDelayCounter > specialDelayPress) {
				activateSpecialCounter = false;
				specialDelayCounter = 0f;
				imageExplode.SetActive (false);
				isRandom = false;
			}

			if (!isRandom) {
				randomN = Random.Range (0, 6); //0 == W, 1 == A, 2 == S, 3 == D,...
			}

			//find a way to create an image on the canvas at a specific position

			if (randomN == 0 && !isRandom) {
				imageW.SetActive (true);
				isRandom = true;
			}
			else if (randomN == 1 && !isRandom) {
				imageA.SetActive (true);
				isRandom = true;
			}
			else if (randomN == 2 && !isRandom) {
				imageS.SetActive (true);
				isRandom = true;
			}
			else if (randomN == 3 && !isRandom) {
				imageD.SetActive (true);
				isRandom = true;
			}
			else if (randomN == 4 && !isRandom) {
				imageLMB.SetActive (true);
				isRandom = true;
			}
			else if (randomN == 5 && !isRandom) {
				imageRMB.SetActive (true);
				isRandom = true;
			}

			/*
			if (Input.GetButtonDown ("Vertical") && Input.GetAxis ("Vertical") > 0 && randomN == 0) {
				imageExplode.SetActive (true); //special fx
				imageW.SetActive (false);
				activateSpecialCounter = true;
				correctSpecialPress++;
			}
			if (Input.GetButtonDown ("Horizontal") && Input.GetAxis ("Horizontal") < 0 && randomN == 1) {
				imageExplode.SetActive (true);
				imageA.SetActive (false);
				activateSpecialCounter = true;
				correctSpecialPress++;
			}
			if (Input.GetButtonDown ("Vertical") && Input.GetAxis ("Vertical") < 0 && randomN == 2) {
				imageExplode.SetActive (true);
				imageS.SetActive (false);
				activateSpecialCounter = true;
				correctSpecialPress++;
			}
			if (Input.GetButtonDown ("Horizontal") && Input.GetAxis ("Horizontal") > 0 && randomN == 3) {
				imageExplode.SetActive (true);
				imageD.SetActive (false);
				activateSpecialCounter = true;
				correctSpecialPress++;
			}
			if (Input.GetMouseButtonDown (0) && randomN == 4) {
				imageExplode.SetActive (true);
				imageLMB.SetActive (false);
				activateSpecialCounter = true;
				correctSpecialPress++;
			}
			if (Input.GetMouseButtonDown (1) && randomN == 5) {
				imageExplode.SetActive (true);
				imageRMB.SetActive (false);
				activateSpecialCounter = true;
				correctSpecialPress++;
			}
			*/

			if (activateSpecialCounter) {
				specialDelayCounter += Time.deltaTime * 1000;
			}
		}

		if (correctSpecialPress >= 3) {

			//get reference from other game objects and use here
			//something cool happens

			if (mecha.isMeleeMode) { //melee mode
				Debug.Log("MELEE SPECIAL");
			}
			else if (!mecha.isMeleeMode) { //range mode
				Debug.Log("RANGE SPECIAL");
			}
			SpecialReset ();
		}

		if (specialAttackCounter >= specialAttackTimer || !isSpecialAttack) {
			SpecialReset ();
		}

	}

	void SpecialReset()
	{
		HideSpecialButtons ();
		isSpecialAttack = false;
		isRandom = false;
		activateSpecialCounter = false;
		specialAttackCounter = 0f;
		specialDelayCounter = 0f;
		correctSpecialPress = 0;
	}

	public bool slashDashSequence1;

	void SlashDash() //LMB + RMB -> D / LMB + RMB -> A
	{
		if (isHitEnemy == true) {
			box2D.isTrigger = true;
		} 
		if (isHitEnemy == false) {
			box2D.isTrigger = false;
		}

		if (isShoot == 1 && isBlock == 1 && isUp == 0 && isDown == 0 && isLeft == 0 && isRight == 0 && isMelee == 0) {
			slashDashSequence1 = true;
		}

		if (slashDashSequence1) {
			//Right Dash
			if (isHoldRight >= dashHoldDuration && !isLeftSlash) { //additional condition to prevent player from slashing left & right
				isRightSlash = true;
				slashDashSequence1 = false;
			}
			//Left Dash
			if (isHoldLeft >= dashHoldDuration && !isRightSlash) {
				isLeftSlash = true;
				slashDashSequence1 = false;
			}
		}

		if (isRightSlash) { //execute dash
			gameObject.tag = "Special Attack"; //the mech change into a tag that makes it invincible and deals damage
			dashCounter += Time.deltaTime * 1000;;
			if (dashCounter <= dashDuration) { //can dash
				//isRightDash = true
				transform.Translate (Vector3.right * Time.deltaTime * dashPower); //actual dashing
				//box2D.isTrigger = true;
				rb2d.constraints = RigidbodyConstraints2D.FreezePositionY;
				/*
				GameObject newSlashEffect = Instantiate(slashEffectPrefab, transform.position, Quaternion.Euler(0, 0, -90));
				*/
				if (!createSlashRightEffect) {
					slashEffectPrefabClone = Instantiate (slashEffectPrefab, new Vector3 (transform.position.x + 1f, transform.position.y, transform.position.z), Quaternion.Euler (0f, 0f, -90f));
					slashEffectPrefabClone.transform.parent = gameObject.transform;
					createSlashRightEffect = true;

				}
			} else if (dashCounter > dashDuration) {
				box2D.isTrigger = false;
				rb2d.constraints = RigidbodyConstraints2D.None;
				rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
				ResetButton (); //allows player to execute sequential combos
			}
		}
		if (isLeftSlash) {
			gameObject.tag = "Special Attack";
			dashCounter += Time.deltaTime * 1000;
			if (dashCounter <= dashDuration) {
				transform.Translate (Vector3.left * Time.deltaTime * dashPower);
				//box2D.isTrigger = true;
				rb2d.constraints = RigidbodyConstraints2D.FreezePositionY;
				/*
				GameObject newSlashEffect = Instantiate(slashEffectPrefab, transform.position, Quaternion.Euler(0, 0, 90));
				*/
				if (!createSlashLeftEffect) {
					slashEffectPrefabClone = Instantiate (slashEffectPrefab, new Vector3 (transform.position.x - 1f, transform.position.y, transform.position.z), Quaternion.Euler (0f, 0f, 90f));
					slashEffectPrefabClone.transform.parent = gameObject.transform;
					createSlashLeftEffect = true;
				}
			} else if (dashCounter > dashDuration) {
				box2D.isTrigger = false;
				rb2d.constraints = RigidbodyConstraints2D.None;
				rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
				ResetButton ();
			}
		}
		if (dashCooldownCounter >= dashGracePeriod) {
			Destroy (slashEffectPrefabClone);
			createSlashLeftEffect = false;
			createSlashRightEffect = false;
			gameObject.tag = "Player"; //+200ms grace period before mech returns to normal state
		}
		if (dashGracePeriod > dashCooldown) { //long grace period breaks the game as mech remains invulnerable after slash dash
			dashGracePeriod = dashCooldown;
		}
	} 

	public bool isRightLowKick; //low kick attack
	public bool isLeftLowKick;
	public bool isLowKickCooldown;
	public float lowKickCooldown = 1000f;
	public float lowKickCooldownCounter;

	void LowKick() //S + D + LMB / S + A + LMB
	{
		if (isLowKickCooldown) {
			lowKickCooldownCounter += Time.deltaTime * 1000f;
		}

		if (lowKickCooldownCounter > lowKickCooldown) {
			isLowKickCooldown = false;
			lowKickCooldownCounter = 0f;
		}

		if (!isLowKickCooldown) {
			//right low kick
			if (isShoot == 1 && isBlock == 0 && isUp == 0 && isDown == 1 && isLeft == 0 && isRight == 1 && isMelee == 0) {
				isRightLowKick = true;
			} 
			//left low kick
			if (isShoot == 1 && isBlock == 0 && isUp == 0 && isDown == 1 && isLeft == 1 && isRight == 0 && isMelee == 0) {
				isLeftLowKick = true;
			}
		}

		if (!isLeftLowKick && isRightLowKick) {
			Debug.Log ("RIGHT LOW KICK!");
			//spawn a prefab clone of a force effect that travels forward
			lowKickPrefabClone = Instantiate (lowKickPrefab, new Vector3 (transform.position.x + 1f, transform.position.y - 1f, transform.position.z), Quaternion.Euler (0f, 0f, -90f));
			isRightLowKick = false;
			isLowKickCooldown = true;
			ResetButton ();
		}
		if (!isRightLowKick && isLeftLowKick) {
			Debug.Log ("LEFT LOW KICK!");
			lowKickPrefabClone = Instantiate (lowKickPrefab, new Vector3 (transform.position.x - 1f, transform.position.y - 1f, transform.position.z), Quaternion.Euler (0f, 0f, 90f));
			isLeftLowKick = false;
			isLowKickCooldown = true;
			ResetButton ();
		}
	}

	/*
	public KeyCode[] sequence = new KeyCode[]
	{
		KeyCode.A,
		KeyCode.B,
		KeyCode.C 
	};

	public int sequenceIndex;
		
	void SequenceAttack()
	{
		if (Input.GetKeyDown (sequence [sequenceIndex])) {
			if (++sequenceIndex == sequence.Length) {
				Debug.Log ("SEQUENCE ATTACK!");
				sequenceIndex = 0;
			} else if (Input.anyKeyDown) {
				sequenceIndex = 0;
			}
		}
	}
	*/

	public bool shadowlessSequence1;
	public bool shadowlessSequence2;

	void ShadowlessStrike() //LMB + A -> RMB + D -> LMB + A
	{
		if (isShoot == 1 && isBlock == 0 && isUp == 0 && isDown == 0 && isLeft == 1 && isRight == 0 && isMelee == 0) {
			shadowlessSequence1 = true;
		} 
		if (shadowlessSequence1) {
			if (isShoot == 1 && isBlock == 1 && isUp == 0 && isDown == 0 && isLeft == 1 && isRight == 1 && isMelee == 0) {
				shadowlessSequence2 = true;
			}
		}
		if (shadowlessSequence2) {
			if (isShoot == 2 && isBlock == 1 && isUp == 0 && isDown == 0 && isLeft == 2 && isRight == 1 && isMelee == 0) { //successful execution
				Debug.Log ("SHADOWLESS STRIKE!");
				shadowlessSequence1 = false;
				shadowlessSequence2 = false;
			}
		}
	}

	void ResetComboTimer()
	{
		if (isKeyPress) {
			//srender.color = Color.gray;
			comboTimer += Time.deltaTime * 1000;
		}
		if (comboTimer > comboDuration) { //resets the button press count to 0
			isUp = 0; 
			isLeft = 0; 
			isDown = 0; 
			isRight = 0;
			isMelee = 0;
			isShoot = 0;
			isBlock = 0; 
			isHoldUp = 0; 
			isHoldLeft = 0; 
			isHoldDown = 0; 
			isHoldRight = 0;
			isHoldMelee = 0;
			isHoldShoot = 0;
			isHoldBlock = 0; 
			comboTimer = 0;
			isKeyPress = false;
			slashDashSequence1 = false; //reset all sequence
			shadowlessSequence1 = false;
			shadowlessSequence2 = false;
			ComboAttackIndicator = ""; //clean string input
		}
	}

	void ResetButton()
	{
		isUp = 0; 
		isLeft = 0; 
		isDown = 0; 
		isRight = 0;
		isMelee = 0;
		isShoot = 0;
		isBlock = 0; 
		isHoldUp = 0; 
		isHoldLeft = 0; 
		isHoldDown = 0; 
		isHoldRight = 0;
		isHoldMelee = 0;
		isHoldShoot = 0;
		isHoldBlock = 0;
		ComboAttackIndicator = "";
	}

	void OnCollisionEnter2D(Collision2D target)
	{
		if (this.gameObject.tag == "Special Attack" && target.gameObject.tag == "Enemy") {
			isHitEnemy = true;
		}
	}
	void OnCollisionStay2D(Collision2D target)
	{
		if (this.gameObject.tag == "Special Attack" && target.gameObject.tag == "Enemy") {
			isHitEnemy = true;
		}
	}

	void OnTriggerEnter2D(Collider2D target)
	{
		if (this.gameObject.tag == "Player" || target.gameObject.tag == "Wall") {
			isHitEnemy = false;
		}
	}
	void OnTriggerStay2D(Collider2D target)
	{
		if (this.gameObject.tag == "Player" || target.gameObject.tag == "Wall") {
			isHitEnemy = false;
		}
	}
}

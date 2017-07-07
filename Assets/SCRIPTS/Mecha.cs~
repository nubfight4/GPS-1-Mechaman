using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Mecha: LifeObject
{

	public BoxCollider2D box2d;
	public SpriteRenderer srender;
	public Sync_Attack sync;

	bool canJumpDownPlatform = false;
	bool isJumping = false;
	bool isSquating = false;
	public bool isMeleeMode = false;
	public bool isHovering = false;

	public float speed;
	public float jumpPower;

	private Animator anim;

	public GameObject bulletPrefab;
	public GameObject meleePrefab;

	//Melee
	public float attackRate = 0.5f;
	const int TOTAL_ATTACK = 2;
	bool[] attack = new bool[TOTAL_ATTACK];
	float[] attackTimer = new float[TOTAL_ATTACK];
	private float comboTimer = 0;
	private bool delay = false;
	private float clickCount = 0;
	bool haveTaste = false;
	private float tasteTimer = 0;

	//Block
	private bool isBlocking = false;
	private bool canBlock = true;
	public float blockCooldown = 0;
	private float blockCdTimer = 0;
	public float blockTimer = 0;
	public float blockDuration = 0;


	//Ammo
	private int ammoAmount;
	private int maxAmmoAmount = 100;
	private bool isRecovering;

	void Awake ()
	{
		//		attackTrigger.enabled = false;
	}
	// Use this for initialization
	void Start ()
	{
		SetMaxHP (500);
		SetHP (this.GetMaxHP ());
		SetAmmoAmount (maxAmmoAmount);
		anim = GetComponent<Animator> ();
		srender = gameObject.GetComponent<SpriteRenderer> ();
		box2d = gameObject.GetComponent<BoxCollider2D> ();
	}
		
	// Update is called once per frame
	void Update ()
	{
		if (isAlive) {
			CheckDeath ();

			if (!isRecovering)
				StartCoroutine (RecoverAmmo ());

			if (Input.GetKeyDown (KeyCode.W) && !isJumping) {	
				GetComponent<Rigidbody2D> ().AddForce (Vector2.up * jumpPower, ForceMode2D.Impulse);
				isJumping = true;
			}

			if (Input.GetKeyDown (KeyCode.S)) {
				if (!isJumping) {
					isSquating = true;
				}
				GetComponent<Rigidbody2D> ().AddForce (Vector2.down * jumpPower, ForceMode2D.Impulse);
			}

			if (Input.GetKeyUp (KeyCode.S)) {
				isSquating = false;
			}

			if (Input.GetKey (KeyCode.A)) {
				transform.localScale = new Vector3 (-1, 1, 1);
				transform.Translate (Vector3.left * Time.deltaTime * speed);
			}

			if (Input.GetKey (KeyCode.D)) {
				transform.localScale = new Vector3 (1, 1, 1);
				transform.Translate (Vector3.right * Time.deltaTime * speed);	
			}

			if (Input.GetKeyDown (KeyCode.RightShift)) {
				if (isMeleeMode == false) {
					isMeleeMode = true;
				} else if (isMeleeMode == true) {
					isMeleeMode = false;	
				}

			}

			if (isMeleeMode) {
				MeleeAttack ();
			}
			Blocking ();
			UpdateAnimator ();		


			/*
		if (transform.position.y > 1.1f) 
		{
			isHovering = true;
			GetComponent<Rigidbody2D> ().constraints = RigidbodyConstraints2D.FreezePositionY;
			GetComponent<SpriteRenderer>().color = Color.yellow;
		}
		else 
		{
			isHovering = false;
			GetComponent<Rigidbody2D> ().constraints = ~RigidbodyConstraints2D.FreezePositionY;
			GetComponent<SpriteRenderer>().color = Color.white;
		}
		*/

		}
	}

	void MeleeAttack ()
	{
		if (Input.GetMouseButtonDown (0)) {
			delay = true;
			clickCount += 1;
		}
		if (delay) {
			canBlock = false;
			if (Input.GetMouseButtonDown (1)) {
				haveTaste = true;
				tasteTimer = 0;
			}
			if (haveTaste) {
				tasteTimer += Time.deltaTime;
				if (tasteTimer > attackRate) {
					haveTaste = false;
					tasteTimer = 0;
				}
			}
			canBlock = true;
			comboTimer += Time.deltaTime;
			if (comboTimer > 1) {
				delay = false;
				comboTimer = 0;
				clickCount = 0;
			}
		}
		if (clickCount == 2 && Input.GetMouseButtonDown (0)) {
			attack [1] = true;
			attackTimer [1] = 0;
		} else if (clickCount == 1 && Input.GetMouseButtonDown (0)) {
			attack [0] = true;
			attackTimer [0] = 0;
		}
		if (attack [0]) {
			attackTimer [0] += Time.deltaTime;
			if (attackTimer [0] > attackRate) {
				attack [0] = false;
				attackTimer [0] = 0;
			}
		}
		if (attack [1]) {
			attackTimer [1] += Time.deltaTime;
			if (attackTimer [1] > attackRate) {
				attack [1] = false;
				attackTimer [1] = 0;
			}
		}
	}

	void Blocking ()
	{
		{
			if (!isJumping) {
				if (Input.GetMouseButton (1) && canBlock == true) {
					isBlocking = true;
					blockTimer += Time.deltaTime;
					if (blockTimer > 3) {
						blockTimer = 0;
						isBlocking = false;
						canBlock = false;
					}
				}
				if (Input.GetMouseButtonUp (1)) {
					blockTimer = 0;
					isBlocking = false;
					canBlock = false;
				}
			}
			if (!canBlock) {
				blockCdTimer += Time.deltaTime;
				if (blockCdTimer >= blockCooldown) {
					blockCdTimer = 0;
					canBlock = true;
				}
			}
		}

	}

	void UpdateAnimator ()
	{
		anim.SetFloat ("Speed", speed);
		anim.SetBool ("isJumping", isJumping);
		anim.SetBool ("isSquating", isSquating);
		anim.SetBool ("isMeleeMode", isMeleeMode);
		anim.SetBool ("attack1", attack [0]);
		anim.SetBool ("attack2", attack [1]);
		anim.SetBool ("isBlocking", isBlocking);
		anim.SetBool ("haveaTaste", haveTaste);
		GetComponent<Animator> ().SetFloat ("Speed", Mathf.Abs (Input.GetAxis ("Horizontal") * speed));
	}

	void OnCollisionEnter2D (Collision2D coll)
	{
		if (coll.gameObject.tag == "Ground") {
			canJumpDownPlatform = false;
			isJumping = false;
		}

		if (coll.gameObject.tag == "Platforms") {
			isJumping = false;
		}
	}

	public int GetAmmoAmount ()
	{
		return this.ammoAmount;
	}

	public void SetAmmoAmount (int ammoAmount)
	{
		this.ammoAmount = ammoAmount;
	}

	public float GetAmmoAmountByPercentage ()
	{
		return (float)GetAmmoAmount () / maxAmmoAmount;
	}

	public bool UseAmmo (int amount)
	{
		if (GetAmmoAmount () - amount >= 0) {
			SetAmmoAmount (GetAmmoAmount () - amount);
			return true;
		}
		return false;
	}

	private IEnumerator RecoverAmmo ()
	{
		isRecovering = true;
		while (true) {
			yield return new WaitForSeconds (3);
			if (GetAmmoAmount () < maxAmmoAmount)
				SetAmmoAmount (GetAmmoAmount () + 1);
		}
		isRecovering = false;
	}
}

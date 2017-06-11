using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Mecha: LifeObject {


	bool canJumpDownPlatform = false;
	bool isJumping = false;
	bool isSquating = false;
	public bool isMeleeMode = true;
	public bool isHovering = false;

	public float velo;
	public float bulletDamage;
	public float speed;
	public float jumpPower;

	public GameObject bulletPrefab;
	public GameObject meleePrefab;

	//melee
	private bool attacking = false;
	private float attackTimer = 0;
	private float attackCoolDown = 0.5f;
	public Collider2D attackTrigger;
	private Animator anim;
	private Rigidbody2D rb2d;

	void Awake()
	{
		//		attackTrigger.enabled = false;
	}
	// Use this for initialization
	void Start () {
		SetMaxHP (500);
		SetHP (this.GetMaxHP ());
		anim = GetComponent<Animator> ();
		rb2d = GetComponent<Rigidbody2D> ();
		velo = GetComponent<Rigidbody2D> ().velocity.x;
	}
		
	// Update is called once per frame
	void Update () {
		if (isAlive) {
			CheckDeath ();

			anim.SetFloat ("Speed", speed);
			anim.SetBool ("isJumping", isJumping);
			anim.SetBool ("isSquating", isSquating);
			anim.SetBool ("isMeleeMode", isMeleeMode);
			if (Input.GetKeyDown (KeyCode.W) && !isJumping) {	
				GetComponent<Rigidbody2D> ().AddForce (Vector2.up * jumpPower, ForceMode2D.Impulse);
				isJumping = true;
			}
			if (Input.GetKeyDown (KeyCode.S)) {

				if (!isJumping) 
				{
					isSquating = true;
				}
				GetComponent<Rigidbody2D> ().AddForce (Vector2.down * jumpPower, ForceMode2D.Impulse);
			}

			if (Input.GetKeyUp (KeyCode.S)) 
			{
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


			//melee
			if (Input.GetKeyDown ("f") && !attacking) {

				attacking = true;
				attackTimer = attackCoolDown;
				Debug.Log ("I am attacking");
			}

			if (attacking) {
				if (attackTimer > 0) {
					attackTimer -= Time.deltaTime;
				} else {
					attacking = false;
					attackTrigger.enabled = false;
					Debug.Log ("I am done attacking");
				}
			}

			if (Input.GetKeyDown (KeyCode.RightShift)) {
				if (isMeleeMode == false) 
				{
					isMeleeMode = true;
				} 
				else if (isMeleeMode == true) 
				{
					isMeleeMode = false;	
				}

			}

			UpdateAnimator();							

			/*	if (Input.GetMouseButtonDown(0))
		{
			Vector3 mouseDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
			mouseDirection.z = 0.0f;
			mouseDirection.Normalize();

			if (isMeleeMode == false) {
				GameObject newBullet = Instantiate (bulletPrefab, transform.position, Quaternion.identity);
				newBullet.GetComponent<Bullet> ().direction = mouseDirection;
			} 
		}
        */

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

	void UpdateAnimator ()
	{
		GetComponent<Animator>().SetFloat("Speed",Mathf.Abs(Input.GetAxis("Horizontal")*speed));

/*
		//Flips character based on direction

		if (rb2d.velocity.x < 0) {
			transform.localScale = new Vector3 (-1, 1, 1);
		}

		else if (rb2d.velocity.x > 0) 
		{
			transform.localScale = new Vector3 (1, 1, 1);
		}
*/
	}

	void OnCollisionEnter2D (Collision2D coll)
	{
		if (coll.gameObject.tag == "Ground")
		{
			canJumpDownPlatform = false;
			isJumping = false;
		}

		if (coll.gameObject.tag == "Platforms")
		{
			isJumping = false;
		}
	}
}

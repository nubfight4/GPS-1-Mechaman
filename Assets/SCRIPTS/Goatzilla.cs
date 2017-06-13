using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Goatzilla : LifeObject 
{
	public BoxCollider2D box2d;
	public Rigidbody2D rb2d;

	public float initialSpeed = 1f;
	public float meleeRange = 3f;
	public float chargeSpeedFactor = 3f;

	public GameObject rockIndicatorPrefab;
	public GameObject eyeLaserPrefab;

	private Mecha target;
	private float speed;
	private float timer;
	private Direction movingDirection;
	private bool faceLeft;
	private bool attacked;
	public bool isEnraged;
	private int enrageHpThreshold;
	private bool nearToTarget;
	private bool freeze;

	private Animator anim;

	enum Direction
	{
		LEFT,
		RIGHT,
		NONE
	}

	void Start () 
	{
		target = GameObject.FindGameObjectWithTag ("Player").GetComponent<Mecha> ();
		anim = GetComponent<Animator> ();
		SetSpeed (GetInitialSpeed ());
		timer = 0f;
		attacked = false;
		SetMaxHP (1000);
		isEnraged = false;
		freeze = false;
		enrageHpThreshold = 300;
		SetHP (GetMaxHP ());
		//ReceiveDamage (700);
	}

	void Update () 
	{
		if (target != null) {
			if (isAlive) {
				CheckDeath ();
				UpdateMonsterCondition (); // Change monster to enrage when HP falls below enrage hp threshold

				if (!freeze) {
					if (!isEnraged)
						UpdateAction ();
					else
						UpdateActionWhileEnraged ();

					anim.SetFloat ("Speed", speed);
					if (movingDirection == Direction.LEFT)
						MoveLeft ();
					else if (movingDirection == Direction.RIGHT)
						MoveRight ();
				}

				timer += Time.deltaTime;
			}
		}
	}

	private IEnumerator UpdateMovingDirection (int numberOfUpdates, float duration)
	{
		float delayReactDuration = duration / numberOfUpdates;
		for (int i = 0; i < numberOfUpdates; i++) {
			yield return new WaitForSeconds (delayReactDuration);
			ChangeMovingDirection ();
		}
	}

	private void ChangeMovingDirection ()
	{
		movingDirection = (GetDistanceFromTarget () > meleeRange) ? SeekTarget () : Direction.NONE;
	}

	private void Reset ()
	{
		SetSpeed (GetInitialSpeed ());
		timer = 0;
		attacked = false;
	}

	private void UpdateAction ()
	{
		if (timer < 2.0f)
			StartCoroutine (UpdateMovingDirection (3, 2));
		else if (timer < 5.0f) {
			if (GetSpeed () != 0)
				SetSpeed (0);
			
			if (!attacked) {
				if (GetDistanceFromTarget () <= meleeRange)
					Slash ();
				else
					ThrowRock ();
			}
		} else
			Reset ();
	}

	private void UpdateActionWhileEnraged ()
	{
		if (timer < 3.0f) {
			if (GetSpeed () != GetInitialSpeed () * chargeSpeedFactor)
				SetSpeed (GetInitialSpeed () * chargeSpeedFactor);
			StartCoroutine (UpdateMovingDirection (5, 3));
		} else if (timer < 6.0f) {
			nearToTarget = (GetDistanceFromTarget () <= meleeRange) ? true : false;
			if (GetSpeed () != 0)
				SetSpeed (0);

			if (!attacked && nearToTarget)
				StartCoroutine (Headbutt ());
		} else if (timer < 7.0f) {
			if (!attacked && !nearToTarget)
				Laser ();
		} else if (timer < 9.0f) {
			if (!nearToTarget)
				Reset ();
		} else
			Reset ();
	}

	private void Flip ()
	{
		Vector3 v3 = transform.localScale;
		v3.x *= -1;
		transform.localScale = v3;
	}

	private void MoveLeft ()
	{
		faceLeft = true;
		transform.Translate (Vector3.left * Time.deltaTime * speed);
		if (transform.localScale.x < 0)
			Flip ();
	}

	private void MoveRight ()
	{
		faceLeft = false;
		transform.Translate (Vector3.right * Time.deltaTime * speed);
		if (transform.localScale.x > 0)
			Flip ();
	}

	private Direction SeekTarget ()
	{
		Vector3 targetDir = target.transform.position - transform.position;
		if (targetDir.x < 0)
			return Direction.LEFT;
		else if (targetDir.x > 0)
			return Direction.RIGHT;
		else
			return Direction.NONE;
	}

	private float GetDistanceFromTarget ()
	{
		return Vector3.Distance (transform.position, target.transform.position);
	}

	private void FaceTarget ()
	{
		float initSpeed = GetSpeed ();
		SetSpeed (1);
		if (movingDirection != SeekTarget ()) {
			if (SeekTarget () == Direction.LEFT)
				MoveLeft ();
			else
				MoveRight ();
		}
		SetSpeed (initSpeed);
	}

	public IEnumerator ApplyDamageWithDelay (int damage, float delay)
	{
		yield return new WaitForSeconds (delay);
		target.ReceiveDamage (damage);
		//target.Knockback (knockbackDir, 1);
	}

	private void Slash ()
	{
		attacked = true;
		if (GetDistanceFromTarget () <= meleeRange) {
			FaceTarget ();
			Vector3 dir = new Vector3 ((target.transform.position.x - transform.position.x) * 1.2f, 2);
			anim.SetTrigger ("Slash");
			StartCoroutine (ApplyDamageWithDelay (40, 1.2f));
		}
	}

	private void ThrowRock ()
	{
		attacked = true;
		anim.SetTrigger ("ThrowRock");
		Vector3 initPos = new Vector3 (target.transform.position.x, 1.5f);
		Instantiate (rockIndicatorPrefab, initPos, Quaternion.identity);
	}

	private void UpdateMonsterCondition ()
	{
		if (!isEnraged && GetHP () <= enrageHpThreshold) {
			isEnraged = true;
			StartCoroutine (Immobolize (1.8f));
			anim.SetTrigger ("Enrage");
			Reset ();
			Debug.Log ("Monster is enraged!");
		}
	}

	private IEnumerator Headbutt ()
	{
		attacked = true;
		FaceTarget ();
		for (int i = 0; i < 3; i++) {
			yield return new WaitForSeconds (1);
			Debug.Log ("Enemy used headbutt! x" + (i + 1));
			if (((faceLeft && SeekTarget () == Direction.LEFT) ||
				(!faceLeft && SeekTarget () == Direction.RIGHT))
				&& GetDistanceFromTarget () <= meleeRange)
				target.ReceiveDamage (80);
		}
	}

	private void Laser ()
	{
		attacked = true;
		FaceTarget ();
		bool isTopToBottom = (target.transform.position.y - transform.position.y >= 0) ? true : false;
		float offsetX = (faceLeft) ? -5f : 5f, offsetY = 2.5f;
		Vector3 initPos = (isTopToBottom) ? new Vector3 (transform.position.x + offsetX, transform.position.y + offsetY) : new Vector3 (transform.position.x + offsetX, transform.position.y - offsetY - 2.5f);
		float initAngle = (faceLeft) ? -45 : 45;
		initAngle *= (isTopToBottom) ? 1 : -1;
		anim.SetBool("IsTopToBottom", isTopToBottom);
		anim.SetTrigger("Laser");
		GameObject laserEye = Instantiate (eyeLaserPrefab, initPos, Quaternion.Euler (0, 0, initAngle));
		laserEye.GetComponent<EyeLaser> ().SetIsDirectionFromLeft (faceLeft);
		laserEye.GetComponent<EyeLaser> ().SetIsTopToBottom (isTopToBottom);
	}

	private float GetInitialSpeed ()
	{
		return this.initialSpeed;
	}

	private void SetSpeed (float speed)
	{
		this.speed = speed;
	}

	private float GetSpeed ()
	{
		return this.speed;
	}

	public void PlayKnockbackAnimation ()
	{
		anim.SetTrigger ("Damage");
	}
		
	public IEnumerator Immobolize (float duration)
	{
		float initTimer = timer, initSpeed = GetSpeed ();
		freeze = true;
		yield return new WaitForSeconds (duration);
		timer = initTimer;
		SetSpeed (initSpeed);
		freeze = false;
	}
}
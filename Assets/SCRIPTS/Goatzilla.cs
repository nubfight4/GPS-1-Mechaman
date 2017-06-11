using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Goatzilla : LifeObject 
{
	public float initialSpeed = 1f;
	public float meleeRange = 3f;
	public float chargeSpeedFactor = 3f;
	public GameObject rockPrefab;
	public GameObject eyeLaserPrefab;
	private Mecha target;
	private float speed;
	private float timer;
	private Direction movingDirection;
	private bool faceLeft;
	private bool attacked;
	private bool isEnraged;
	private int enrageHpThreshold;
	private bool nearToTarget;
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
		enrageHpThreshold = 300;
		SetHP (GetMaxHP ());
		//ReceiveDamage (700);
	}

	void Update () 
	{
		if (isAlive) {
			CheckDeath ();

			if (target != null) {
				UpdateMonsterCondition (); // Change monster to enrage when HP falls below enrage hp threshold

				if (!isEnraged) {
					UpdateAction ();
				} else {
					UpdateActionWhileEnraged ();
				}

				anim.SetFloat ("Speed", speed);
				if (movingDirection == Direction.LEFT) {
					MoveLeft ();
				} else if (movingDirection == Direction.RIGHT) {
					MoveRight ();
				}
				timer += Time.deltaTime;
			}
		}

	}

	IEnumerator UpdateMovingDirection (int numberOfUpdates, float duration)
	{
		float delayReactDuration = duration / numberOfUpdates;
		for (int i = 0; i < numberOfUpdates; i++) {
			yield return new WaitForSeconds (delayReactDuration);
			ChangeMovingDirection ();
		}
	}

	void ChangeMovingDirection ()
	{
		movingDirection = (GetDistanceFromTarget () > meleeRange) ? SeekTarget () : Direction.NONE;
	}

	void Reset ()
	{
		SetSpeed (GetInitialSpeed ());
		timer = 0;
		attacked = false;
	}

	void UpdateAction ()
	{
		if (timer < 2.0f) {
			StartCoroutine (UpdateMovingDirection (3, 2));
		} else if (timer < 5.0f) {
			if (GetSpeed () != 0) {
				SetSpeed (0);
			}
			if (!attacked) {
				if (GetDistanceFromTarget () <= meleeRange) {
					Slash ();
				} else {
					ThrowRock ();
				}
			}
		} else {
			Reset ();
		}
	}

	void UpdateActionWhileEnraged ()
	{
		if (timer < 3.0f) {
			if (GetSpeed () != GetInitialSpeed () * chargeSpeedFactor) {
				SetSpeed (GetInitialSpeed () * chargeSpeedFactor);
			}
			StartCoroutine (UpdateMovingDirection (5, 3));
		} else if (timer < 6.0f) {
			nearToTarget = (GetDistanceFromTarget () <= meleeRange) ? true : false;
			if (GetSpeed () != 0) {
				SetSpeed (0);
			}
			if (!attacked && nearToTarget) {
				StartCoroutine (Headbutt ());
			}
		} else if (timer < 7.0f) {
			if (!nearToTarget) {
				if (!attacked) {
					Laser ();
				}
			} else {
				Reset ();
			}
		} else if (timer >= 9) {
			Reset ();
		}
	}

	void Flip ()
	{
		Vector3 v3 = transform.localScale;
		v3.x *= -1;
		transform.localScale = v3;
	}

	void MoveLeft ()
	{
		faceLeft = true;
		transform.Translate (Vector3.left * Time.deltaTime * speed);
		if (transform.localScale.x < 0) {
			Flip ();
		}
	}

	void MoveRight ()
	{
		faceLeft = false;
		transform.Translate (Vector3.right * Time.deltaTime * speed);
		if (transform.localScale.x > 0) {
			Flip ();
		}
	}

	Direction SeekTarget ()
	{
		Vector3 targetDir = target.transform.position - transform.position;
		if (targetDir.x < 0)
			return Direction.LEFT;
		else if (targetDir.x > 0)
			return Direction.RIGHT;
		else
			return Direction.NONE;
	}

	float GetDistanceFromTarget ()
	{
		return Vector3.Distance (transform.position, target.transform.position);
	}

	void FaceTarget ()
	{
		float initSpeed = GetSpeed ();
		SetSpeed (1);
		if (movingDirection != SeekTarget ()) {
			if (SeekTarget () == Direction.LEFT) {
				MoveLeft ();
			} else {
				MoveRight ();
			}
		}
		SetSpeed (initSpeed);
	}

	IEnumerator ApplyDamageWithDelay (int damage, Vector3 knockbackDir, float delay)
	{
		yield return new WaitForSeconds (delay);
		target.ReceiveDamage (damage);
		target.Knockback (knockbackDir, 1);
	}

	void Slash ()
	{
		attacked = true;
		if (GetDistanceFromTarget () <= meleeRange) {
			FaceTarget ();
			Vector3 dir = new Vector3 ((target.transform.position.x - transform.position.x) * 1.2f, 2);
			anim.SetTrigger ("Slash");
			StartCoroutine (ApplyDamageWithDelay (40, dir, 1.2f));
		}
	}

	void ThrowRock ()
	{
		attacked = true;
		// FaceTarget ();
		// float offset = (faceLeft) ? -3 : 3;
		// Vector3 initPos = new Vector3 (transform.position.x + offset, transform.position.y);
		Vector3 initPos = new Vector3 (target.transform.position.x, 5);
		GameObject rock = Instantiate (rockPrefab, initPos, Quaternion.identity);
		rock.GetComponent<Rock> ().Initialize (target, 50, 8.0f);
	}

	void UpdateMonsterCondition ()
	{
		if (!isEnraged && GetHP () <= enrageHpThreshold) {
			isEnraged = true;
			anim.SetTrigger ("Enrage");
			Reset ();
			Debug.Log ("Monster is enraged!");
		}
	}

	IEnumerator Headbutt ()
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
		Debug.Log ("Enemy used laser eye!");
		attacked = true;
		FaceTarget ();
		float offsetX = (faceLeft) ? -5.7f : 5.7f, offsetY = 5.9f;
		Vector3 initPos = new Vector3 (transform.position.x + offsetX, transform.position.y + offsetY);
		float initAngle = (faceLeft) ? -45 : 45;
		GameObject laserEye = Instantiate (eyeLaserPrefab, initPos, Quaternion.Euler (0, 0, initAngle));
		laserEye.GetComponent<EyeLaser> ().SetIsDirectionFromLeft (faceLeft);
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
}
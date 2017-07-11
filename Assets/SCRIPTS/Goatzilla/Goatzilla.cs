using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Goatzilla : LifeObject 
{
	public BoxCollider2D box2d;
	public Rigidbody2D rb2d;

	public float rockSpawnHeight = 5.0f;
	public float initialSpeed = 1f; // Phase 1 initial speed
	public float meleeRange = 3.5f; // Melee Distance 1f = 100px
	public float meleeTimer = 0.0f;
	public float meleeDuration = 1.0f;
	//	public float chargeSpeedFactor = 3f; // Phase 2 charge speed
	public float walkTimer = 0.0f;

	//! Rock
	public float rockTimer = 0.0f;
	public float rockDuration = 3.0f;
	private int rockCounter = 0;
	private int rockThrowCounter = 0;

	//! Acid
	public float acidTimer = 0.0f;
	public float acidDuration = 1.0f;
	bool isAcidSpit = false;
	Vector3 spitPos;

	public GameObject rockIndicatorPrefab;
//	public GameObject eyeLaserPrefab;
	public GameObject rockPrefab;
	public GameObject acidProjectilePrefab;

	public float UU; // distance between the rocks

	private Mecha target;
	private float speed;
	private Direction movingDirection;
	public bool isEnraged;
	private int enrageHpThreshold;
	private bool nearToTarget;

	private Animator anim;
	public bool isWalkAnim = false;
	public bool isSwipeAnim = false;
	public bool isAcidAnim = false;
	public bool isRoarAnim = false;
	public bool isHeadbuttAnim = false;	
	public bool isThrowRockAnim = false;
	public bool isEnrageAnim = false;


	enum Direction
	{
		LEFT,
//		RIGHT,
		NONE,
	}

	public enum AttackState
	{
		SWIPE,
		HEADBUTT,
		THROWROCK,
		ACID,
		ROAR,
		NONE,
	}

	public enum BehaviorState
	{
		NORMAL,
		ENRAGE,
		DEATH,
	}

	public AttackState prevAttackState;
	public AttackState curAttackState;
	public BehaviorState curBehaviorState;



	void Awake ()
	{
		lives = 3;
		SetMaxHP (500); // Max HP for a bar
		SetHP (GetMaxHP ());
	}

	void Start () 
	{
		target = GameObject.FindGameObjectWithTag ("Player").GetComponent<Mecha> ();
		anim = GetComponent<Animator> ();
		SetSpeed (GetInitialSpeed ());
		isEnraged = false;
		enrageHpThreshold = (GetHP() * lives) / 2 ; // Enrage HP
		//isCharging = false;
		//ReceiveDamage (1000);
		curAttackState = AttackState.NONE;
		prevAttackState =  AttackState.NONE;
	}

	void Update () 
	{
		if (target != null) 
		{
			//! new code!
			CheckDeath();
			if(isAlive)
			{
				//! Normal
				UpdateMonsterCondition();
				if (curBehaviorState == BehaviorState.NORMAL) 
				{ 				
					if (curAttackState == AttackState.NONE) 
					{
						if(isSwipeAnim == false)
						{
							Walk (3.0f);
						}
					} 
					else if (curAttackState == AttackState.THROWROCK) 
					{
						ThrowRock ();
					} 
					else if (curAttackState == AttackState.ACID) 
					{
						if(isAcidSpit == true)
						{
							Acid ();
						}
						else
						{
							if(isAcidAnim == false)
							{
								isAcidAnim = true;
								anim.SetTrigger ("DoAcid");
							}
						}
					} 
					else if (curAttackState == AttackState.SWIPE) 
					{
						if(isSwipeAnim == false)
						{
							isSwipeAnim = true;
							anim.SetTrigger("DoSwipe");
						}
					} 
					else if (curAttackState == AttackState.ROAR) 
					{
						//Roar (500);
						UpdateAttackState(AttackState.NONE);
					}
				}

				//! Enrage
				else if (curBehaviorState == BehaviorState.ENRAGE)
				{
					if (curAttackState == AttackState.NONE )
					{
						Walk (5.0f);
					}
					else if ( curAttackState == AttackState.THROWROCK )
					{
						ThrowRock ();
					}
					else if ( curAttackState == AttackState.ACID )
					{
						Acid ();
					}
					else if ( curAttackState == AttackState.HEADBUTT )
					{
						Headbutt ();
					}
					else if (curAttackState == AttackState.ROAR) 
					{
						Roar (500);
					}
				}

			}
			else
			{

			}
		}


	}

	private Direction SeekTarget ()
	{
		Vector3 targetDir = target.transform.position - transform.position;
		if (targetDir.x < 0)
			return Direction.LEFT;
		//else if (targetDir.x > 0)
		//	return Direction.RIGHT;
		else
			return Direction.NONE;
	}

	private float GetDistanceFromTarget ()
	{
		return Vector3.Distance (transform.position, target.transform.position);
	}

	public override void CheckDeath ()
	{
		if (HP <= 0) 
		{
			if(lives > 0)
			{
				HP = 500;
			}
			else
			{
				Destroy (gameObject, 3f);
				isAlive = false;
			}
		}
	}

	public void ResetAnim()
	{
		Debug.Log("isReset?");
		meleeTimer = 0.0f;
		isSwipeAnim = false;
		isAcidAnim = false;
		isRoarAnim = false;
		isHeadbuttAnim = false;	
		isThrowRockAnim = false;
		isEnrageAnim = false;
	}

	private void Walk (float walkDuration)
	{
		if (walkTimer <= walkDuration)
		{
			if  (GetDistanceFromTarget() <= meleeRange)
			{
				meleeTimer += Time.deltaTime;
			}

			else
			{
				meleeTimer = 0.0f;
			}

			if (meleeTimer >= meleeDuration)
			{
				anim.SetBool("DoWalk", false);
				UpdateAttackState(AttackState.SWIPE);
			}

			else
			{
				walkTimer += Time.deltaTime;

				//! Problem
				anim.SetBool("DoWalk", true);
				transform.Translate (Vector3.left * Time.deltaTime * speed);
			}

		}

		else
		{
			anim.SetBool("DoWalk", false);
			if (curBehaviorState == BehaviorState.NORMAL)
			{
				if ( this.transform.position.x < 0.0f )
				{
					UpdateAttackState(AttackState.ROAR);
				}

				else
				{
					UpdateAttackState(AttackState.THROWROCK);
				}
				walkTimer = 0.0f;
			}
		}
	}

	private void Swipe ()
	{
		//StartCoroutine (ApplyDamage (20)); // Slash dmg, Delay to see hp decrease
		if(GetDistanceFromTarget () <= meleeRange)
		{
			target.ReceiveDamage(20);
		}
		UpdateAttackState(AttackState.NONE);
	}


	private void ThrowRock()
	{
		if(GetDistanceFromTarget () >= meleeRange)
		{
			/*
			 * 1) Randomly choose a rock throw type
			 * 2) Logic for Rock Toss 1
			 * 3) Logic for Rock Toss 2
			 */
			int choice = Random.Range(0, 2);

			if (choice == 0)
			{
				Instantiate(rockPrefab, target.transform.position + (Vector3.up * rockSpawnHeight), Quaternion.identity);
				Instantiate(rockPrefab, target.transform.position + (Vector3.up * rockSpawnHeight) + Vector3.right, Quaternion.identity);
				Instantiate(rockPrefab, target.transform.position + (Vector3.up * rockSpawnHeight) + Vector3.left, Quaternion.identity);
				rockThrowCounter++;
				if(rockThrowCounter >= 3)
				{
					UpdateAttackState(AttackState.ACID);
				}
				else
				{
					UpdateAttackState(AttackState.NONE);
					rockCounter = 0;
				}
			}
			else if (choice == 1)
			{
				//! Check if > 5 rocks switch state else to this logic
				if ( rockCounter >= 4 )
				{
					rockThrowCounter++;
					if(rockThrowCounter >= 3)
					{
						UpdateAttackState(AttackState.ACID);
					}
					else
					{
						UpdateAttackState(AttackState.NONE);
						rockCounter = 0;
					}
				}
				else
				{
					rockTimer += Time.deltaTime;
					if(rockTimer >= rockDuration)
					{
						rockCounter ++;
						Instantiate(rockPrefab, target.transform.position + (Vector3.up * rockSpawnHeight), Quaternion.identity);
						rockTimer = 0.0f;
					}	
				}
			}

		}
	}

	public void StartSpit()
	{
		Debug.Log("Spit");
		isAcidSpit = true;
		spitPos = new Vector3 (this.transform.position.x - 3.81f, -2.53f, 0.0f);
		Instantiate(acidProjectilePrefab, spitPos, Quaternion.identity);
	}

	private void Acid ()
	{
		//! Spit disappear

		//! 0.19 left of boss
		//! 2.46 between each acid
		//Instantiate (acidProjectilePrefab, transform.position, Quaternion.identity);

		acidTimer += Time.deltaTime;
		if(acidTimer >= acidDuration)
		{
			spitPos +=  (Vector3.left * 2.46f);
			acidTimer = 0.0f;

			if(spitPos.x < -11.0f)
			{
				UpdateAttackState(AttackState.NONE);
			}
			else
			{
				Instantiate(acidProjectilePrefab, spitPos, Quaternion.identity);
			}
		}	

	}

	private IEnumerator Headbutt ()
	{
		isHeadbuttAnim = true;
		//FaceTarget ();
		//for (int i = 0; i < 3; i++) 
		//{
			yield return new WaitForSeconds (1); // 1 sec headbutt once
		//	Debug.Log ("Enemy used headbutt! x" + (i + 1));
			anim.SetTrigger ("DoHeadbutt");
//			StartCoroutine (ApplyDamage (70));
		//}
		//yield return new WaitForSeconds (2); // Rest 2s
		UpdateAttackState(AttackState.NONE);
	}

	private void Roar(int damage)
	{
			isRoarAnim = true;
			anim.SetTrigger ("DoRoar");
//		else if (this.ReceiveDamage(500))
//		{
//			Debug.Log( "hihihihihi"); 
//		}
		UpdateAttackState(AttackState.NONE);
	}


	/*private void Laser ()
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
		StartCoroutine (Immobolize (4, false));
	}*/

	public IEnumerator Immobolize (float duration, bool invincible)
	{
		SetSpeed (0);
		if (invincible)
			SetIsInvincible (true);
		yield return new WaitForSeconds (duration);
		SetIsInvincible (false);
	}

	public void PlayKnockbackAnimation ()
	{
		anim.SetTrigger ("Damage");
	}

	void UpdateAttackState(AttackState state)
	{
		prevAttackState = curAttackState;
		curAttackState = state;
	}

	//	public void UpdateBehaviourState ()
	//	{
	//		if(GetBehaviourState () == BehaviorState.NORMAL)
	//			if(GetRemainingHPPercentage () <= 50)
	//				curBehaviorState = BehaviorState.ENRAGE;
	//		else if(GetBehaviourState () == BehaviorState.ENRAGE)
	//			if(GetRemainingHPPercentage () <= 0)
	//				curBehaviorState = BehaviorState.DEATH;
	//	}

	private void UpdateMonsterCondition ()
	{
		if (!isEnraged && (GetHP ()* lives) <= enrageHpThreshold) 
		{
			isEnraged = true;
			isEnrageAnim = true;
			StartCoroutine (Immobolize (3f, true)); //Invulnerable + Immoblize for 3s
			anim.SetTrigger ("DoEnrage");
			Debug.Log ("Monster is enraged!");
		}
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


	public bool GetIsEnraged ()
	{
		if (GetBehaviourState () == BehaviorState.ENRAGE)
			return true;
		else
			return false;
	}

	public BehaviorState GetBehaviourState ()
	{
		return curBehaviorState;
	}

	void OnCollisionEnter2D (Collision2D target)
	{
//		if (target.gameObject.CompareTag ("Player") && isCharging) 
//		{
//			target.gameObject.GetComponent<Mecha> ().ReceiveDamage (chargeDamage);
//		}
	}
}
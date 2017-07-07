using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeSmashScript : MonoBehaviour
{

	public float moveSpeed = 100f;
	public float moveDuration = 100f;
	public float moveDurationCounter = 0f;

	void Start ()
	{
		
	}

	void Update ()
	{
		meSmashMovement ();
	}

	void meSmashMovement ()
	{
		transform.Translate (Vector2.up * moveSpeed * Time.deltaTime);
		transform.localScale += new Vector3 (0.2f, 0.2f, 0f);
		if (moveDurationCounter <= moveDuration) {
			moveDurationCounter += Time.deltaTime * 1000f;
		} else {
			moveDurationCounter = 0f;
			Destroy (gameObject);
		}
	}
}

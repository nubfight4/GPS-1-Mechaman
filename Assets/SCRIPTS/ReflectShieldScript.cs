using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectShieldScript : MonoBehaviour
{

	public bool isReflecting = false;
	public float reflectDuration = 1000f;
	public float reflectDurationCounter = 0f;

	void Start ()
	{
		
	}

	void Update ()
	{
		if (!isReflecting) {
			if (reflectDurationCounter <= reflectDuration) {
				reflectDurationCounter += Time.deltaTime * 1000f;
			} else {
				reflectDurationCounter = 0f;
				isReflecting = true;
			}
		} else if (isReflecting) {
			Destroy (gameObject);
		}
	}
}

/* 
 * create an empty that shoots enemy bullet towards player!
 * detects collision from box collider of an enemy bullet with respective tag
 * find out whether the reflection is true for "up" or "down" from sync attack script
 * if "up" is true then change bullet movement upwards & vice versa
 */

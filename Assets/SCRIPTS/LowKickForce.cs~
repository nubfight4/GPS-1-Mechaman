using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowKickForce : MonoBehaviour {

	//when spawned set duration before it kills self
	//set duration for how long and how fast it travels

	public float lifeDuration = 1000f;
	public float lifeCounter;
	public float speed = 10f;

	Sync_Attack syncAttack;

	void Start () {
		syncAttack = gameObject.GetComponent<Sync_Attack> ();
	}

	void Update () {

		LowKick ();
	}

	void LowKick ()
	{
		lifeCounter += Time.deltaTime * 1000f;

		if (lifeCounter <= lifeDuration) {
			transform.Translate (Vector3.up * Time.deltaTime * speed);
			transform.localScale += new Vector3(0.2F, 0.1f, 0);
		} else if (lifeCounter > lifeDuration) {
			Destroy (this.gameObject);
		}
	}
}

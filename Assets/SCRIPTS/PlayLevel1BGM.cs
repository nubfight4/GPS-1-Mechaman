using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayLevel1BGM : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Debug.Log("AFUBAUIBAF");
		SoundManagerScript.Instance.PlayBGM(AudioClipID.BGM_LEVEL1);


	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

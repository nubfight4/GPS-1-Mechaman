using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_1_BGM : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Sound_Manager_Script.Instance.PlayBGM(AudioClipID.BGM_LEVEL_1);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}

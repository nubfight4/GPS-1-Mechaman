﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenOnInput : MonoBehaviour {
    
    // Update is called once per frame
    void Update () {
		
        if(Input.GetKeyDown(KeyCode.Q) && (Input.GetKeyDown(KeyCode.U)))
        {
			SceneManager.LoadScene("MainMenuScene");
        }
	}
}

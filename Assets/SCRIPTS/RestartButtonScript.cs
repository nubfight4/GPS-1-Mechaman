using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartButtonScript : MonoBehaviour {
	private GameSceneManager gSM;

	void Start()
	{
		gSM = GameObject.FindGameObjectWithTag("EditorOnly").GetComponent<GameSceneManager>();
	}

	void OnMouseDown()
	{
		gSM.GetComponent<GameSceneManager>().ReloadScene();
	}
}

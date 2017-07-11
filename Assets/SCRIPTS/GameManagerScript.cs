using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
	private static GameManagerScript mInstance = null;

	public static GameManagerScript Instance
	{
		get
		{
			//! singleton implementation for objects that can dynamically be created (does not have reference to hierarchy objects)
			if(mInstance == null)
			{
				GameObject tempObject = GameObject.FindWithTag("GameManager");

				if(tempObject == null)
				{
					tempObject = Instantiate(PrefabManagerScript.Instance.gameManagerPrefab, Vector3.zero, Quaternion.identity);
				}
				mInstance = tempObject.GetComponent<GameManagerScript>();
			}
			return mInstance;
		}
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboManagerScript : MonoBehaviour {

	private static ComboManagerScript _instance;
	public static ComboManagerScript Instance
	{
		get {return _instance;}
	}
		
	void Awake()
	{
		if(_instance == null) _instance = this;
		else if(_instance != this)
			Destroy( this.gameObject);
		DontDestroyOnLoad(transform.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

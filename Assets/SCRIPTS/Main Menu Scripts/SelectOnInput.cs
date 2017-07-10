using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectOnInput : MonoBehaviour {

    public EventSystem eventSyst;
    public GameObject objSelect;

    private bool buttonSelect;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
        if(Input.GetAxisRaw("Vertical") != 0 && buttonSelect == false)
        {
            eventSyst.SetSelectedGameObject(objSelect);
            buttonSelect = true;
        }
	}

    private void OnDisable()
    {
        buttonSelect = false;
    }
}

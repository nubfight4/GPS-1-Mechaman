using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerScript : MonoBehaviour {
	public bool paused;
	private Sprite menu;
	private GameObject camera;
	private Mecha mechaScript;
	private Shooting shootScript;
	private Sync_Attack sAttackScript;

	void Start()
	{
		paused = false;
		mechaScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Mecha>();
		shootScript = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Shooting>();
		sAttackScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Sync_Attack>();
		camera = Camera.main.transform.Find("PauseMenu").gameObject;
		menu = camera.GetComponent<MenuScript>().menu1;
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.P))
		{
			paused = !paused;
			camera.GetComponent<SpriteRenderer>().sprite = menu;
		}
		if(paused)
		{
			Time.timeScale = 0;
			camera.SetActive(true);
			mechaScript.GetComponent<Mecha>().enabled = false;
			shootScript.GetComponent<Shooting>().enabled = false;
			sAttackScript.GetComponent<Sync_Attack>().enabled = false;
		}
		else if (!paused)
		{
			Time.timeScale = 1;
			camera.SetActive(false);
			mechaScript.GetComponent<Mecha>().enabled = true;
			shootScript.GetComponent<Shooting>().enabled = true;
			sAttackScript.GetComponent<Sync_Attack>().enabled = true;
		}
	}
}

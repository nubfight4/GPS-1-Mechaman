using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerScript : MonoBehaviour {
	public bool paused;
	public Sprite victory;
	public Sprite defeat;
	private Sprite menu;
	private GameObject mecha;
	private GameObject enemy;
	private GameObject pauseScene;
	private GameObject endScene;
	private SpriteRenderer endRend;
	private Mecha mechaScript;
//	private Shooting shootScript;
	private Sync_Attack sAttackScript;

	void Start()
	{
		paused = false;
		mecha = GameObject.FindGameObjectWithTag("Player").gameObject;
		enemy = GameObject.FindGameObjectWithTag("Enemy").gameObject;
		mechaScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Mecha>();
//		shootScript = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Shooting>();
		sAttackScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Sync_Attack>();
		pauseScene = Camera.main.transform.Find("PauseMenu").gameObject;
		endScene = Camera.main.transform.Find("EndScene").gameObject;
		endRend = endScene.GetComponent<SpriteRenderer>();
		menu = pauseScene.GetComponent<PauseMenuScript>().menu1;
		 
	}

	void Update()
	{
		if(mecha != null && enemy != null)
		{
			endScene.SetActive(false);
			if(Input.GetKeyDown(KeyCode.P))
			{
				paused = !paused;
				pauseScene.GetComponent<SpriteRenderer>().sprite = menu;
			}
			if(paused)
			{
				Time.timeScale = 0;
				pauseScene.SetActive(true);
				mechaScript.GetComponent<Mecha>().enabled = false;
//				shootScript.GetComponent<Shooting>().enabled = false;
				sAttackScript.GetComponent<Sync_Attack>().enabled = false;
			}
			else if (!paused)
			{
				Time.timeScale = 1;
				pauseScene.SetActive(false);
				mechaScript.GetComponent<Mecha>().enabled = true;
//				shootScript.GetComponent<Shooting>().enabled = true;
				sAttackScript.GetComponent<Sync_Attack>().enabled = true;
			}	
		}
		else
		{
			if(mecha == null)
			{
				endRend.sprite = defeat;
			}
			else if(enemy == null)
			{
				Time.timeScale = 0;
				endRend.sprite = victory;
			}
			endScene.SetActive(true);
			Time.timeScale = 0;
		}
	}
}

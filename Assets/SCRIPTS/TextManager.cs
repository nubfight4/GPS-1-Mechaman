﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TextManager : MonoBehaviour
{
    bool levelTutorial = true;
    [SerializeField]
    bool textScroll = true;

    //Tutorial Level Bool
	bool tutorNormalAtk;
	bool tutorHeavyAtk;
	bool tutorBlock;
	bool tutorShadowlessStrike;
	bool tutorPause;
	bool tutorSyncAtk;

	public Vector3 gamepadPos;
    
   // int threeWayCombo = 0;

    public GameObject textBox;

    public Text theText;

    public TextAsset textFile;
    public string[] textLine;

    public int currentLine;
    public int endAtLine;


    // Use this for initialization
    void Start()
    {
		tutorNormalAtk = false;
		tutorHeavyAtk = false;
		tutorBlock = false;
		tutorShadowlessStrike = false;
		tutorPause = false;
		tutorSyncAtk = false;

        if (textFile != null)
        {
            textLine = (textFile.text.Split('\n'));
        }

        if (endAtLine == 0)
        {
            endAtLine = textLine.Length - 1;
        }

        if (levelTutorial == true)
        {
            currentLine = 0;
        }

    }

    void Update()
	{
		gamepadPos.x = Input.GetAxis ("Horizontal");
		gamepadPos.y = Input.GetAxis ("Vertical");

        theText.text = textLine[currentLine];

        if (textScroll == true)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                currentLine += 1;
            }
        }


        if (currentLine > endAtLine)
        {
            textBox.SetActive(false);
        }

        levelSelection();
        tutorial();
    }

    void levelSelection()
    {
        if (currentLine == 101) //level 1
        {
            SceneManager.LoadScene("GameScene");
            currentLine = 20;
        }
    }

    void tutorial()
    {
        if (currentLine == 7) //Movement
        {
            textScroll = false;

			if (gamepadPos.x > 0.1 || gamepadPos.x < -0.1) 
			{
				if (gamepadPos.y > 0.1 || gamepadPos.y < -0.1) {
					textScroll = true;
					currentLine += 1;
				}
			}

			/*
            if (Input.GetKeyDown(KeyCode.A))
            {
                tutorLeft = true;
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                tutorRight = true;
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                tutorJump = true;
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                tutorDown = true;
            }
			

            if (tutorJump == true && tutorLeft == true && tutorRight == true && tutorDown == true)
            {
                textScroll = true;
                currentLine += 1;
                tutorLeft = false;
                tutorRight = false;
                tutorJump = false;
                tutorDown = false;
            }
            */
        }

        if (currentLine == 10) //Attack n Block
        {
            textScroll = false;

			if (Input.GetKeyDown (KeyCode.Joystick1Button2)) 
			{ 
				tutorNormalAtk = true;
			}

			if (Input.GetKeyDown (KeyCode.Joystick1Button0)) 
			{
				tutorHeavyAtk = true;
			}

			if (Input.GetKeyDown (KeyCode.Joystick1Button1)) {
				tutorBlock = true;
			}

			if (tutorNormalAtk == true && tutorHeavyAtk == true && tutorBlock == true)
			{
				textScroll = true;
				currentLine += 1;
			}


			/*
            if (Input.GetMouseButtonDown(0))
            {
                Punch = true;
            }

            if (Input.GetMouseButtonDown(1))
            {
                Block = true;
            }

            if (Punch == true && Block == true)
            {
                textScroll = true;
                currentLine += 1;
                Block = false;
                Punch = false;
            }
			*/
        }

        if (currentLine == 14) //Combo Attack
        {
            textScroll = false;
			/*
            if (Input.GetMouseButtonDown(0))
            {
                threeWayCombo += 1;
            }

            if (threeWayCombo == 3)
            {
                textScroll = true;
                currentLine += 1;
                threeWayCombo = 0;
            }
            */
        }

        if (currentLine == 16) //Sync Attack
        {
            textScroll = false;
			/*
            if (Input.GetKeyDown(KeyCode.Y))
            {
                textScroll = true;
                currentLine = 22;
            }

            else if (Input.GetKeyDown(KeyCode.N))
            {
                textScroll = true;
                currentLine += 1;
            }
            */
        }

        if (currentLine == 21)
        {
            currentLine = 101;
        }

    }
}



using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TextBoxManager : MonoBehaviour
{
    bool levelTutorial = true;
    [SerializeField]
    bool textScroll = true;

    //Tutorial Level Bool
    bool tutorLeft = false;
    bool tutorRight = false;
    bool tutorJump = false;
    bool tutorDown = false;
    bool Punch = false;
    bool Block = false;
    bool comboBut1 = false;
    bool comboBut2 = false;
    bool comboBut3 = false;
    
    int threeWayCombo = 0;

    public GameObject textBox;

    public Text theText;

    public TextAsset textFile;
    public string[] textLine;

    public int currentLine;
    public int endAtLine;


    // Use this for initialization
    void Start()
    {
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

    private void Update()
    {
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
        if (currentLine == 8) //Movement
        {
            textScroll = false;
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

        if (currentLine == 11) //Attack n Block
        {
            textScroll = false;
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

        if (currentLine == 15) //Combo Attack
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

        if (currentLine == 17) //Sync Attack
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


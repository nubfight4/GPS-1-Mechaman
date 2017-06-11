using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


public class TextBoxManager : MonoBehaviour {


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
    }

    private void Update()
    {
        theText.text = textLine[currentLine];

        if(Input.GetKeyDown(KeyCode.Return))
        {
            currentLine += 1;
        }

        if(currentLine > endAtLine)
        {
            textBox.SetActive(false);
        }
    }



}

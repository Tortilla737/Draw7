using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawHand : MonoBehaviour {

    private List<string> deckList = new List<string>();
    public bool pictureMode;
    public Text textField;
    public GameObject[] cardPanels;
    public GameObject nextCardPanel;

    private void Start()
    {
        
    }

    public void DrawHandButton()
    {
        GetDecklist();
        GetComponent<SceneHandler>().NewhandScene();
        MakeRandom();
    }

    public void RedrawButton()
    {

    }

    private void MakeRandom()
    {
        foreach (GameObject i in cardPanels)
        {

        }
    }

    private void GetDecklist()
    {
        deckList.Clear();

        char[] delimiters = new char[] { '\r', '\n' };
        string[] deckListBuffer = textField.text.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

        char[] trimsStart = new char[] {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'x', ' '};
        foreach (string i in deckListBuffer)
        {
            string cardName = i.TrimStart(trimsStart);
            cardName.TrimEnd(' ');
            deckList.Add(cardName);
            Debug.Log(cardName);
            /*
            bool withPre = false;
            foreach (char c in trimsStart)
            {
                if (i.StartsWith(c.ToString()))
                {
                    withPre = true;
                }
            }
            if (withPre)
            {
                string cardName = i.TrimStart(trimsStart);
                cardName.TrimEnd(' ');
                deckList.Add(cardName);
            }
            else
            {
                deckList.Add(i.TrimEnd(' '));
            }
            */
        }

        //deck sortieren und syntax fixen
    }
}

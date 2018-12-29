using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class DrawHand : MonoBehaviour
{

    private List<string> deckList = new List<string>();
    public bool pictureMode;
    public Text textField;
    public GameObject[] cardPanels;
    public GameObject nextCardPanel;
    

    public void DrawHandButton()
    {
        GetDecklist();
        GetComponent<SceneHandler>().NewhandScene();
        foreach (GameObject i in cardPanels)
        {
            RandomCardInPanel(i);
        }
    }

    public void RedrawButton()
    {
        foreach (GameObject i in cardPanels)
        {
            RandomCardInPanel(i);
        }
    }

    private void RandomCardInPanel(GameObject cardPanel)
    {
        int[] deckIndex = new int[deckList.Count];
        for(int i = 0; i< deckList.Count; i++) //für jede Karte gibt es einen prüf-Index, um Duplikate zu vermeiden
        {
            deckIndex[i] = i;
        }
        int randomInt = UnityEngine.Random.Range(0, deckList.Count);
        // karte muss eingetragen werden
        cardPanel.GetComponentInChildren<Text>().text = deckList[randomInt];
        cardPanel.GetComponentInChildren<Image>();
    }

    private void GetDecklist()
    {
        deckList.Clear();

        char[] delimiters = new char[] { '\r', '\n' };
        string[] deckListBuffer = textField.text.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

        char[] trimsStart = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'x', ' ' };
        foreach (string i in deckListBuffer)
        {
            int cardCount;
            int.TryParse(Regex.Replace(i, "[^0-9]", ""), out cardCount);
            string cardName = i.TrimStart(trimsStart);
            cardName.TrimEnd(' ');

            if (cardName.EndsWith(")")) // Set Kürzel entfernen
            {
                cardName = cardName.Substring(0, cardName.Length - 6);
            }

            if (cardCount == 0) // falls keine nummer vor der Karte steht wird sie einmal hinzugefügt
            {
                cardCount++;
            }

            for (int n = 0; n < cardCount; n++)
            {
                if (cardName.EndsWith("*"))
                {
                    //der Commander wird nicht ins Deck gemischt
                }
                else
                {
                    deckList.Add(cardName);
                }
            }
        }
    }
}

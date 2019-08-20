using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DrawHand : MonoBehaviour
{

    private List<string> deckList;
    private CardMover mover;
    private List<bool> alreadyDrawn;
    public List<GameObject> cardPanels;
    public TextMeshProUGUI textField;
    public GameObject cardOverPanel;
    public int cardsDrawn;

    private void Start()
    {
        mover = GetComponent<CardMover>();
        deckList = new List<string>();
        alreadyDrawn = new List<bool>();
        cardsDrawn = 0;
    }

    public void DrawHandButton()
    {
        GetDecklist();
        GetComponent<SceneHandler>().NewhandScene();
        RedrawButton();
    }

    public void RedrawButton()
    {
        cardsDrawn = 0;
        foreach (GameObject panel in cardPanels)
        {
            panel.transform.position = mover.startPoint.transform.position;
        }
        for (int i = 0; i < alreadyDrawn.Count; i++)
        {
            alreadyDrawn[i] = false;
        }
        while(cardsDrawn < 7)
        {
            DrawAdditionalCard();
        }
    }

    public void DrawAdditionalCard()
    {
        if (cardsDrawn < deckList.Count)
        {
            cardsDrawn++;

            if (cardsDrawn >= cardPanels.Count)
            {
                cardPanels.Add(CreateEmptyCard());
            }
            cardPanels[cardsDrawn - 1].GetComponent<CardButton>().nthCard = cardsDrawn;
            RandomCardToPanel(cardPanels[cardsDrawn-1]);

            for (int i = 0; i < cardsDrawn; i++)    //jedes Mal wenn eine Karte gezogen wird, werden alle Karten neu positioniert
            {
                mover.AnimateToPosition(cardPanels[i]);
            }
        }
    }

    private GameObject CreateEmptyCard()
    {
        GameObject panelBuffer = Instantiate(mover.cardPref, cardOverPanel.transform.GetChild(0));
        panelBuffer.transform.position = mover.startPoint.transform.position;
        return panelBuffer;
    }

    private void RandomCardToPanel(GameObject cardPanel)
    {
        int randomInt = UnityEngine.Random.Range(0, deckList.Count);

        if (alreadyDrawn[randomInt] == false)
        {
            cardPanel.GetComponentInChildren<Text>().text = deckList[randomInt];
            if (mover.pictureMode)
            {
                cardPanel.transform.GetChild(0).GetComponent<Image>().sprite = ByteToSprite(GetImageData(deckList[randomInt]));
            }
            alreadyDrawn[randomInt] = true;
        }
        else
        {
            RandomCardToPanel(cardPanel);
        }
    }
    private byte[] GetImageData(String cardName)
    {
        String fixedName = cardName;
        fixedName = fixedName.Replace("/", "");
        fixedName = fixedName.Replace(" ", "_");
        fixedName = fixedName.Replace(",", "");
        String fixedDeckName = GetComponent<DeckHandlerSystem>().loadNameField.text;
        fixedDeckName = fixedDeckName.Replace(" ", "_");
        String path = GetComponent<DeckHandlerSystem>().GetFixedPath();
        path = path + "ImageData/" + fixedDeckName + "/" + fixedName + ".png";

        byte[] imageBytes = {};

        try
        {
            imageBytes = File.ReadAllBytes(path);
        }
        catch (Exception e)
        {
            Debug.LogWarning("Failed To Load Data from: " + path.Replace("/", "\\"));
            Debug.LogWarning("Error: " + e.Message);
        }
        return imageBytes;
    }
    private Sprite ByteToSprite(byte[] input)
    {
        Texture2D bufferTexture = new Texture2D(745, 1040); //ist eigentlich egal
        ImageConversion.LoadImage(bufferTexture, input);
        Sprite bufferSprite = Sprite.Create(bufferTexture, new Rect(0,0,bufferTexture.width, bufferTexture.height), new Vector2(0.5f, 0.5f));

        return bufferSprite;
    }

    private void GetDecklist()
    {
        deckList.Clear();
        alreadyDrawn.Clear();

        char[] delimiters = new char[] { '\r', '\n' };
        string[] deckListBuffer = textField.text.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

        char[] trimsStart = new char[] {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'x', ' ', '\r', '\n'};
        foreach (string card in deckListBuffer)
        {
            string cardName = card;
            cardName.TrimEnd(' ');
            if (cardName.EndsWith(")")) // Set Kürzel entfernen
            {
                cardName = cardName.Substring(0, cardName.Length - 6);
            }
            int cardCount;
            int.TryParse(Regex.Replace(cardName, "[^0-9]", ""), out cardCount);
            cardName = cardName.TrimStart(trimsStart);


            if (cardCount == 0) // falls keine nummer vor der Karte steht wird sie einmal hinzugefügt
            {
                cardCount++;
            }

            for (int n = 0; n < cardCount; n++)
            {
                if (cardName.EndsWith("*CMDR*"))
                {
                    //der Commander wird nicht ins Deck gemischt
                    cardName = cardName.Substring(0, cardName.Length - 7);
                    GameObject panelBuffer = Instantiate(mover.cardPref, cardOverPanel.transform.GetChild(0));
                    panelBuffer.transform.position = mover.startPoint.transform.GetChild(0).position;
                    panelBuffer.transform.GetChild(0).GetComponent<Image>().sprite = ByteToSprite(GetImageData(cardName));
                    panelBuffer.GetComponent<CardButton>().nthCard = 0;
                }
                else
                {
                    if (cardName.Length > 1)
                    {
                        deckList.Add(cardName);
                        alreadyDrawn.Add(false);
                    }
                }
            }
        }
    }
}

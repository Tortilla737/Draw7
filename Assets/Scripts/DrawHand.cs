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
    public bool pictureMode = true;
    public TextMeshProUGUI textField;
    public List<GameObject> cardPanels;
    public GameObject nextCardPanel;
    public GameObject pictureModeIcon;
    private CardMover mover;

    private void Start()
    {
        mover = GetComponent<CardMover>();
        deckList = new List<string>();
    }

    public void DrawHandButton()
    {
        GetDecklist();
        GetComponent<SceneHandler>().NewhandScene();
        RedrawButton();
    }

    public void RedrawButton()
    {
        foreach (GameObject panel in cardPanels)
        {
            RandomCardInPanel(panel);
        }
        //Animation
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
        // hier noch random next logik eintragen
        cardPanel.GetComponentInChildren<Text>().text = deckList[randomInt];
        cardPanel.transform.GetChild(0).GetComponent<Image>().sprite = ByteToSprite(GetImageData(deckList[randomInt]));
    }
    private byte[] GetImageData(String cardName)
    {
        String fixedName = cardName;
        fixedName = fixedName.Replace("/", "+");
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

        char[] delimiters = new char[] { '\r', '\n' };
        string[] deckListBuffer = textField.text.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

        char[] trimsStart = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'x', ' ' };
        foreach (string card in deckListBuffer)
        {
            int cardCount;
            int.TryParse(Regex.Replace(card, "[^0-9]", ""), out cardCount);
            string cardName = card.TrimStart(trimsStart);
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
                    //Instantiate Commander 
                }
                else
                {
                    deckList.Add(cardName);
                }
            }
        }
    }

    public void TogglePictureMode()
    {
        pictureMode = !pictureMode;

        pictureModeIcon.SetActive(pictureMode);
    }
}

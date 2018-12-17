using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class DeckHandlerSystem : MonoBehaviour {

    public Text textField;
    public Text nameField;
    private string[] deckList;


	void Start () {
		
	}

    public void TextToList()
    {
        char[] delimiters = new char[] { '\r', '\n' };
        deckList = textField.text.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
        foreach (string i in deckList)
        {
            Debug.Log(i);

        }
    }

    public void SaveStringToText(string stringInput, string deckName)
    {
        string path = Application.dataPath + "/DecklistData/" + deckName + ".txt";

        StreamWriter writer = new StreamWriter(path);
        if (File.Exists(path))
        {
            writer.AutoFlush = true;
            writer.WriteLine(stringInput);
            
        }
        else
        {
            File.Create(path);
            SaveStringToText(stringInput, deckName);
        }
        writer.Close();
    }

    public void LoadDecklist(string filePath)
    {
        if (File.Exists(filePath))
        {
            StreamReader reader = new StreamReader(filePath);

            for (int i = 0; i<= deckList.Length; i++)
            {

            }
            string job = reader.ReadLine();

            reader.Close();
        }
        else
        {
            Debug.Log(filePath + " nicht gefunden");
        }
    }
}

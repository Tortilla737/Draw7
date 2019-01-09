using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class DeckHandlerSystem : MonoBehaviour {

    public Text textField;
    public Text saveNameField;
    public Text loadNameField;
    private string fixedDataPath;
    private List<string> deckList = new List<string>();
    ExternalStorageHandler storageHandler;


	void Start () {
        storageHandler = GetComponent<ExternalStorageHandler>();
        SetDataPath();
	}
    
    private void SetDataPath()
    {
        //check existance first...
#if UNITY_EDITOR
        fixedDataPath = Application.dataPath;

#elif UNITY_ANDROID
        fixedDataPath = storageHandler.GetAndroidExternalFilesDirLaunch();

#endif

        fixedDataPath = fixedDataPath + "/DrawSevenData/";
        if (!Directory.Exists(fixedDataPath))
        {
            Directory.CreateDirectory(fixedDataPath);
        }
    }

    public void SaveDecklistButton()
    {
        if (saveNameField.text == "")
        {
            //No Name Warning
        }
        else
        {
            TextToList();
            SaveStringToText(deckList.ToArray(), saveNameField.text);
        }
    }

    public void LoadDecklistButton()
    {
        string path = fixedDataPath + loadNameField.text.Replace(" ", "_") + ".txt";
        LoadDecklist(path);
    }

    private void TextToList() //saves textField content in a List<string> deckList
    {
        deckList.Clear();

        char[] delimiters = new char[] { '\r', '\n' };
        string[] deckListBuffer = textField.text.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
        foreach (string i in deckListBuffer)
        {
            // Syntax korrigieren (5 plains -> 5x Plains)
            deckList.Add(i);
        }
    }

    private void SaveStringToText(string[] stringInput, string deckName)
    {
        string path = fixedDataPath + deckName.Replace(" ", "_") + ".txt";
        
        
        if (File.Exists(path))
        {
            Debug.Log("file exists");
            //überschreiben? message
            StreamWriter writer = new StreamWriter(path);
            writer.AutoFlush = true;
            foreach(string i in stringInput)
            {
                writer.WriteLine(i);
            }
            writer.Close();
        }
        else
        {
            //'does not exist' message
            var fileBuffer = File.Create(path);
            fileBuffer.Close();
            SaveStringToText(stringInput, deckName);
        }
    }

    private void LoadDecklist(string filePath)
    {
        deckList.Clear();
        if (File.Exists(filePath))
        {
            StreamReader reader = new StreamReader(filePath);
            string textBuffer;

            textBuffer = reader.ReadLine();
            while (textBuffer != null)
            {
                deckList.Add(textBuffer);
                textBuffer = reader.ReadLine();
            }

            reader.Close();

            textField.text = "";
            foreach (string i in deckList)
            {
                textField.text = textField.text + i + '\n';
            }
        }
        else
        {
            Debug.Log(filePath + " nicht gefunden");
        }
    }
    public string GetFixedPath()
    {
        return fixedDataPath;
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class DeckHandlerSystem : MonoBehaviour {

    public Text textField;
    public Text nameField;
    private string deckBuffer;
    private string fixedDataPath;
    ExternalStorageHandler storageHandler;


	void Start () {
        storageHandler = GetComponent<ExternalStorageHandler>();
        SetDataPath();
	}
    
    private void SetDataPath()
    {

#if UNITY_EDITOR
        fixedDataPath = Application.dataPath;
#elif UNITY_ANDROID
        fixedDataPath = storageHandler.GetAndroidExternalFilesDirLaunch();
#endif
    }

    public string[] TextToList(string textLine)
    {
        char[] delimiters = new char[] { '\r', '\n' };
        return textLine.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
    }

    public void SaveButtonPressed()
    {
        string shortName = nameField.text.Replace(" ", "_");
        string content = textField.text;
        SaveStringToText(content, shortName);
    }
    

    public void SaveStringToText(string stringInput, string deckName)
    {
        string path = fixedDataPath + "/DecklistData/" + deckName + ".txt";

        Debug.Log(Application.persistentDataPath);
        Debug.Log(Application.dataPath);
        Debug.Log(path);

        if (File.Exists(path))
        {
            StreamWriter writer = new StreamWriter(path);

            //überschreiben Abfrage
            writer.AutoFlush = true;
            writer.WriteLine(stringInput);

            writer.Close();
        }
        else
        {
            Debug.Log("trying to create " +path);
            File.Create(path);
            SaveStringToText(stringInput, deckName);
        }
    }

    public void LoadDecklist(string deckName)
    {
        deckName = nameField.text;
        string path = fixedDataPath + "/DecklistData/" + deckName.Replace(" ", "_") + ".txt";

        if (File.Exists(path))
        {
            StreamReader reader = new StreamReader(path);
            
            //deckBuffer bekommt den String aus der Datei
            deckBuffer = reader.ReadToEnd();
            Debug.Log("kompletter text:\n" + deckBuffer);
            string[] listBuffer = TextToList(deckBuffer);
            foreach (string i in listBuffer)
            {
                Debug.Log("Zeile: " + i);
                textField.text = string.Concat(textField.text, i);
            }
            reader.Close();
        }
        else
        {
            Debug.Log(path + "\nnicht gefunden");
        }
    }
    
}

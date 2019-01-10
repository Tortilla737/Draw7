using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.IO;
using System;

public class ImageGaleryHandler : MonoBehaviour {

    // https://img.scryfall.com/cards/png/en/ktk/170.png


    public Text deckNameTextField;
    private DeckHandlerSystem _deckHandler;
    private string fixedPath;

    private void Start()
    {
        _deckHandler = GetComponent<DeckHandlerSystem>();
        fixedPath = _deckHandler.GetFixedPath();
    }

    public void GetOnlineImage()
    {
        string deckPath = fixedPath + deckNameTextField.text.Replace(" ", "_") + ".txt";
        List<string> deckList = _deckHandler.LoadDecklist(deckPath);

        char[] trimsStart = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'x', ' ' };
        foreach (string name in deckList)
        {
            StartCoroutine(GetCardData(name.TrimStart(trimsStart)));
        }
        
    }

    IEnumerator GetCardData(string cardName)
    {
        //Get card link
        UnityWebRequest datawww = UnityWebRequest.Get("https://api.scryfall.com/cards/named?fuzzy=" + cardName.Replace(" ", string.Empty));
        yield return datawww.SendWebRequest();
        if (datawww.isNetworkError || datawww.isHttpError)
        {
            Debug.Log(datawww.error + cardName);
        }
        else
        {
            //Get specific data
            string[] cardData;
            cardData = datawww.downloadHandler.text.Split(","[0]);
            string fixedUrl = "";
            string fixedName = "";

            for (int i = 0; i< cardData.Length; i++)
            {
                if (cardData[i].StartsWith("\"name"))
                {
                    fixedName = cardData[i].TrimEnd('"');
                    break;
                }
                if (cardData[i].StartsWith("\"png"))
                {
                    fixedUrl = cardData[i].TrimEnd('"');
                    break;
                }
            }
            fixedUrl = fixedUrl.Substring(fixedUrl.LastIndexOf(":") + 1);
            fixedName = fixedName.Substring(fixedName.LastIndexOf(":") + 2);
            string imagePath = fixedPath;
            imagePath = imagePath + "ImageData/"+ deckNameTextField.text.Replace(" ", "_") + "/" + fixedName.Replace(" ", "_");

            if (!File.Exists(imagePath))
            {
                DownloadImage(fixedUrl, imagePath);
            }
            else
            {
                Debug.Log(fixedName + " bereits in der Bibliothek");
            }


            /*
            for (int i = 0; i< cardData.Length; i++)
            {
                Debug.Log(i + " - " + cardData[i]);  //achtung! Kommata im Oracletext können zu Fehlern führen
            }
            */
        }
    }
    

    //----------online lösung-------------

    public void DownloadImage(string url, string pathToSaveImage)
    {
        WWW www = new WWW(url);
        StartCoroutine(_downloadImage(www, pathToSaveImage));
    }

    private IEnumerator _downloadImage(WWW www, string savePath)
    {
        yield return www;

        //Check if we failed to send
        if (string.IsNullOrEmpty(www.error))
        {
            Debug.Log("Success");

            //Save Image
            SaveImage(savePath, www.bytes);
        }
        else
        {
            Debug.Log("Error: " + www.error);
        }
    }

    void SaveImage(string path, byte[] imageBytes)
    {
        //Create Directory if it does not exist
        if (!Directory.Exists(Path.GetDirectoryName(path)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
        }

        try
        {
            File.WriteAllBytes(path, imageBytes);
            Debug.Log("Saved Data to: " + path.Replace("/", "\\"));
        }
        catch (Exception e)
        {
            Debug.LogWarning("Failed To Save Data to: " + path.Replace("/", "\\"));
            Debug.LogWarning("Error: " + e.Message);
        }
    }

    byte[] LoadImage(string path)
    {
        byte[] dataByte = null;

        //Exit if Directory or File does not exist
        if (!Directory.Exists(Path.GetDirectoryName(path)))
        {
            Debug.LogWarning("Directory does not exist");
            return null;
        }

        if (!File.Exists(path))
        {
            Debug.Log("File does not exist");
            return null;
        }

        try
        {
            dataByte = File.ReadAllBytes(path);
            Debug.Log("Loaded Data from: " + path.Replace("/", "\\"));
        }
        catch (Exception e)
        {
            Debug.LogWarning("Failed To Load Data from: " + path.Replace("/", "\\"));
            Debug.LogWarning("Error: " + e.Message);
        }

        return dataByte;
    }



}

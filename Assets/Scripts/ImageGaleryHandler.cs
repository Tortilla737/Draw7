using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.IO;
using System;

public class ImageGaleryHandler : MonoBehaviour {

    // https://img.scryfall.com/cards/png/en/ktk/170.png

    public Image bufferImage;
    public string inputName;

    public void GetOnlineImage()
    {
        
        StartCoroutine(GetCardData(inputName));
        
    }

    IEnumerator GetCardData(string cardName)
    {
        //Get card link
        UnityWebRequest datawww = UnityWebRequest.Get("https://api.scryfall.com/cards/named?fuzzy=" + cardName.Replace(" ", string.Empty));
        yield return datawww.SendWebRequest();
        if (datawww.isNetworkError || datawww.isHttpError)
        {
            Debug.Log(datawww.error);
        }
        else
        {
            //Get specific data
            string[] cardData;
            cardData = datawww.downloadHandler.text.Split(","[0]);
            Debug.Log(cardData[0]); //Card name
        }
    }

    IEnumerator GetCardImage()
    {

        UnityWebRequest www = UnityWebRequestTexture.GetTexture("");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Texture myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            //bufferImage.sprite = myTexture;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.IO;
using System;
using TMPro;

public class ImageGaleryHandler : MonoBehaviour {

    // https://img.scryfall.com/cards/png/en/ktk/170.png


    public Text deckNameTextField;
    public Text toDeleteTextField;
    public GameObject progressBarObj;
    private Image progressBar;
    private TextMeshProUGUI progressText;
    private int downloadProgress;
    private string fixedPath;
    private List<string> deckList;
    private int failed;

    private void Start()
    {
        fixedPath = GetComponent<DeckHandlerSystem>().GetFixedPath();
        progressBar = progressBarObj.transform.GetChild(0).GetComponent<Image>();
        progressText = progressBarObj.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        deckList = new List<string>();
    }

    public void GetOnlineImage()
    {
        string deckPath = fixedPath + deckNameTextField.text.Replace(" ", "_") + ".txt";
        deckList = GetComponent<DeckHandlerSystem>().LoadDecklist(deckPath);
        downloadProgress = 0;
        failed = 0;

        StartCoroutine(GetAllImages());
    }
    private IEnumerator GetAllImages()
    {
        char[] trimsStart = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'x', ' ' };
        for (int i = 0; i < deckList.Count; i++)
        {
            String cardName;
            cardName = deckList[i].TrimStart(trimsStart);
            if (cardName.EndsWith(")")) // Set Kürzel entfernen
            {
                cardName = cardName.Substring(0, cardName.Length - 6);
            }
            StartCoroutine(GetCardData(cardName));
            while (i >= downloadProgress + failed)
            {
                yield return null;
            }
        }
    }

    public void DeleteDeckFolder()
    {
        string deletePath = fixedPath + "ImageData/" + toDeleteTextField.text.Replace(" ", "_") + "/";
        if (Directory.Exists(deletePath))
        {
            Directory.Delete(deletePath, true);
        }
        else
        {
            Debug.Log("File not found " + deletePath);
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
            failed++;
            progressText.text = "failed " + failed;
        }
        else
        {
            //Get specific data
            string[] cardData;
            cardData = datawww.downloadHandler.text.Split(","[0]);  
            string fixedImageUrl = "";
            string fixedName = "";

            for (int i = 0; i< cardData.Length; i++)
            {
                if (cardData[i].StartsWith("\"name"))
                {
                    if (cardData[i+1].StartsWith(" "))  //falls der Name ein "," enthält, folgt ein " "
                    {
                        fixedName = cardData[i] + cardData[i+1];
                    }
                    else
                    {
                        fixedName = cardData[i];
                    }
                    fixedName = fixedName.TrimEnd('"');
                    fixedName = fixedName.Substring(fixedName.LastIndexOf(":") + 2);
                    fixedName = fixedName.Replace("/", "");
                    fixedName = fixedName.Replace(" ", "_");
                }
                if (cardData[i].StartsWith("\"png"))
                {
                    fixedImageUrl = cardData[i].TrimEnd('"');
                    fixedImageUrl = fixedImageUrl.Substring(fixedImageUrl.LastIndexOf(":") + 1);
                    break;
                }
            }

            string imagePath = fixedPath + "ImageData/" + deckNameTextField.text.Replace(" ", "_");
            if (!Directory.Exists(imagePath))   //Ordner für die Bilder vom Deck wird erstellt
            {
                Directory.CreateDirectory(imagePath);
            }

            imagePath = imagePath + "/" + fixedName + ".png"; //eventuell das ".png" wieder wegnehmen?

            if (!File.Exists(imagePath))
            {
                DownloadImage(fixedImageUrl, imagePath);
            }
            else
            {
                downloadProgress++;
                progressBar.color = new Color(0.34f, 0.75f, 0.42f); //grün
                UpdateProgress();
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
    private void UpdateProgress()
    {
        progressText.text = downloadProgress + "/" + deckList.Count;
        progressBar.fillAmount = (float)downloadProgress/deckList.Count;
        if (downloadProgress == deckList.Count)
        {
            progressText.text = "Done!";
        }
    }

    //----------online lösung-------------

    public void DownloadImage(string url, string pathToSaveImage)
    {
        UnityWebRequest www = new UnityWebRequest(url);
        www.downloadHandler = new DownloadHandlerBuffer();
        StartCoroutine(_downloadImage(www, pathToSaveImage));
    }

    private IEnumerator _downloadImage(UnityWebRequest www, string savePath)
    {
        yield return www.SendWebRequest();

        //Check if we failed to send
        if (string.IsNullOrEmpty(www.error))
        {
            SaveImage(savePath, www.downloadHandler.data);

            if (www.isDone)
            {
                downloadProgress++;
                progressBar.color = new Color(0, 0.882f, 1); //cyan
                UpdateProgress();
            }
        }
        else
        {
            Debug.Log("Error: " + www.error);
            failed++;
            progressBar.color = new Color(1, 0.3f, 0.184f); //red
            progressText.text = "failed " + failed;
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

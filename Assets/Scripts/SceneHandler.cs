using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneHandler : MonoBehaviour {

    public GameObject[] deckEditorScene;
    public GameObject[] newHandScene;

    public void NewhandScene()
    {
        foreach (GameObject i in deckEditorScene)
        {
            i.SetActive(false);
        }
        foreach (GameObject i in newHandScene)
        {
            i.SetActive(true);
        }
    }

    public void EditorScene()
    {
        foreach (GameObject i in newHandScene)
        {
            i.SetActive(false);
        }
        foreach (GameObject i in deckEditorScene)
        {
            i.SetActive(true);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextInputHandler : MonoBehaviour {
    
    
    private TouchScreenKeyboard keyboard;
    private bool writeEnabled = false;
    private TextMeshProUGUI textField;

    private void Start()
    {
        textField = GetComponent<TextMeshProUGUI>();
    }

    // Updates button's text while user is typing
    void OnGUI()
    {
        if (writeEnabled)
        {
            keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Search, false, true, false);
            TouchScreenKeyboard.hideInput = true;
        }

        if (keyboard != null)
        {
            textField.text = keyboard.text;
            writeEnabled = false;
        }
    }
#if UNITY_EDITOR
    //Code here for Editor only.
#endif

    public void StartEdit()
    {
        writeEnabled = true;
        
    }
}

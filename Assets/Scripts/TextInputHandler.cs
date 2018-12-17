using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextInputHandler : MonoBehaviour {
    
    
    private TouchScreenKeyboard keyboard;
    private bool writeEnabled = false;

    // Updates button's text while user is typing
    void OnGUI()
    {
        if (writeEnabled)
        {
            keyboard = TouchScreenKeyboard.Open(GetComponent<Text>().text, TouchScreenKeyboardType.Default, false, true, false);
        }

        if (keyboard != null)
        {
            GetComponent<Text>().text = keyboard.text;
            writeEnabled = false;
        }
    }

    public void StartEdit()
    {
        writeEnabled = true;
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownDeckOptions : MonoBehaviour {
    
    private List<string> m_DropOptions = new List<string> { "Option 1", "Option 2" };
    private Dropdown m_Dropdown;

    

    public void RefreshDropdown()
    {
        m_Dropdown = GetComponent<Dropdown>();
        m_Dropdown.ClearOptions();
        m_DropOptions.Clear();

        foreach (string file in System.IO.Directory.GetFiles(GameObject.Find("Handler").GetComponent<DeckHandlerSystem>().GetFixedPath()))
        {
            if (file.EndsWith(".txt"))
            {
                string fixedName;
                fixedName = file.Substring(0, file.Length - 4);    //.txt entfernen
                fixedName = fixedName.Substring(file.LastIndexOf("/")+1);      //Pfad entfernen
                fixedName = fixedName.Replace("_", " ");

                m_DropOptions.Add(fixedName);

            }
        }

        m_Dropdown.AddOptions(m_DropOptions);
    }
}

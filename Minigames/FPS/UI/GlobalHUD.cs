using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalHUD : MonoBehaviour
{
    public GameObject messagePanel;
    public GameObject incorrectWeaponPanel;
    public Text textToEdit;

    public void OpenPanel(GameObject panel, string text)
    {
        textToEdit.text = text;
        panel.SetActive(true);
    }
    
    public void ClosePanel(GameObject panel)
    {
        panel.SetActive(false);
    }
}

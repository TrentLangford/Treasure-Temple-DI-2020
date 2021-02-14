using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public TMP_Text interactionText;
    public bool isAlreadyActive;

    private void Awake()
    {
        interactionText.text = "";
        interactionText.gameObject.SetActive(false);
    }
    public void EnableInteractText(string objName, string text, string interactKey, bool state)
    {
        interactionText.text = $"Press {interactKey.ToUpper()} " + text + objName;
        interactionText.gameObject.SetActive(state);
        isAlreadyActive = state;
    }
}

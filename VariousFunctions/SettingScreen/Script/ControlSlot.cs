using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ControlSlot : MonoBehaviour
{
    public TextMeshProUGUI text;
    public ActionKey keyType;
    private KeyCode previousKey = KeyCode.None;
    [SerializeField] private GameObject returnImage;

    public void PreviousKeySave()
    {
        if (Enum.TryParse(text.text, out KeyCode parsedKeyCode))
        {
            previousKey = parsedKeyCode;
            returnImage.SetActive(true);
        }
    }

    public void ReturnKeyBind()
    {
        if(previousKey != KeyCode.None)
        {
            InputManager.GetInstance.Bind(keyType, previousKey);
            returnImage.SetActive(false);
        }
    }
}

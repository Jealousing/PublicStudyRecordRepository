using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ControlsOption : MonoBehaviour
{
    [SerializeField] private ControlSlot[] slots;

    private void Update()
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if (InputManager.GetInstance.bindKeys.TryGetValue(slots[i].keyType, out KeyCode keyCode))
            {
                slots[i].text.text = keyCode.ToString();
            }
            else
            {
                slots[i].text.text = "Not Set";
            }
        }

        if (keyType != -1 && Input.anyKeyDown)
        {
            foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKey(keyCode))
                {
                    InputManager.GetInstance.Bind((ActionKey)keyType, keyCode);
                    keyType = -1;
                    break;
                }
            }
        }
    }


    int keyType = -1;
    public void KeyBinding(int num)
    {
        keyType = num;
    }

    public void ResetKeyBinding()
    {
        InputManager.GetInstance.DefaultKeyBindings();
    }

}

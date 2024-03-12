using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsManager : MonoBehaviour
{
    [SerializeField] private List<OptionButton> optionButtons;
    private OptionButton curOption;

    private void Awake()
    {
        optionButtons[0].Set(true);
    }

    public void ShowOption(int num)
    {
        if(curOption != null)
        {
            curOption.Set(false);
        }
        else
        {
            optionButtons[0].Set(false);
        }
        curOption = optionButtons[num].Set(true);
    }
}

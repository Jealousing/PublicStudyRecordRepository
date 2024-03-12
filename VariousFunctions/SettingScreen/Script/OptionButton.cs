using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class OptionButton : MonoBehaviour
{
    [SerializeField] private Image background;
    [SerializeField] private Image border;
    [SerializeField] private GameObject view;

    public OptionButton Set(bool set)
    {
        Color bgColor = background.color;
        if (set)
        {
            bgColor.a = 0;
        }
        else
        {
            bgColor.a = 100f / 255f;
        }
        background.color = bgColor;
        border.enabled = !set;
        view.SetActive(set);
        return this;
    }
}

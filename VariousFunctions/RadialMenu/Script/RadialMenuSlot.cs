using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RadialMenuSlot : MonoBehaviour
{
    public Color hoverColor;
    public Color baseColor;
    public Image background;

    public GameObject hoverObject;

    private void Start()
    {
        DeSelect();
    }

    public void Select()
    {
        background.color = hoverColor;
        if(hoverObject !=null )hoverObject.SetActive(true);
        this.transform.localScale = Vector3.one*1.05f;
    }

    public void DeSelect()
    {
        background.color = baseColor;
        if (hoverObject != null) hoverObject.SetActive(false);
        this.transform.localScale = Vector3.one;
    }

}

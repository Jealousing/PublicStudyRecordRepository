using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialMenuManager : MonoBehaviour
{
    public List<RadialMenuSlot> menuSlots;// 메뉴 리스트

    [HideInInspector]
    public int menuCount = 5; // 메뉴 갯수

    /* Slot Control */
    public Vector2 mousePos;
    private float curAngle;
    private int curSelection;
    private int preSelection;


    private RadialMenuSlot curSlot;
    private RadialMenuSlot preSlot;

    #region eventFunction
    private void Start()
    {
        RadialMenuSlot[] temp = GetComponentsInChildren<RadialMenuSlot>();
        menuSlots = new List<RadialMenuSlot>(temp);
    }

    private void Update()
    {
        SelectSlost();
    }

    #endregion


    #region Slot Control

    void SelectSlost()
    {
        if (menuSlots.Count <= 0) return;

        mousePos = new Vector2(Input.mousePosition.x - Screen.width / 2, Input.mousePosition.y - Screen.height / 2);
        curAngle = (Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg + 360) % 360;
        curSelection = (int)((360 - curAngle + 270) % 360) / (360 / menuCount);

        if (Vector2.Distance(mousePos, Vector2.zero) < 50f && curSlot !=null)
        {
            curSlot.DeSelect();
            return;
        }

        if (curSelection != preSelection)
        {
            preSlot = menuSlots[preSelection];
            preSlot.DeSelect();
            preSelection = curSelection;

            curSlot = menuSlots[curSelection];
            curSlot.Select();
        }
    }



    #endregion
}

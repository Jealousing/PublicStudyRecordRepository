using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialMenuSlotBuilder : MonoBehaviour
{
    public RadialMenuManager manager;

    /* MenuSlot Builder  */
    public GameObject menuItemPrefab; // 라디얼 메뉴 프리팹

    [HideInInspector]
    public int menuCount
    {
        get { return manager.menuCount; }
        set 
        {
            manager.menuCount = value;
        }
    }
    [Range(0, 0.01f)]
    public float space = 0;


    #region MenuSlot Builder

    // + 버튼 클릭 시 실행되는 메서드
    public void IncreaseItemCount()
    {
        menuCount++;
        UpdateMenuItems();
    }

    // - 버튼 클릭 시 실행되는 메서드
    public void DecreaseItemCount()
    {
        if (menuCount > 2) // 최소 2개의 메뉴 아이템이 남도록 설정
        {
            menuCount--;
            UpdateMenuItems();
        }
    }
    // ReSet 버튼 클릭 시 실행되는 메서드
    public void Reset()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
        manager.menuSlots.Clear();

        menuCount = 2;
        UpdateMenuItems();
    }

    void UpdateMenuItems()
    {
        // 갯수가 더 많으면 추가 메뉴 아이템 생성
        if (manager.menuSlots.Count < menuCount)
        {
            for (int i = manager.menuSlots.Count; i < menuCount; i++)
            {
                GameObject menuSlot = Instantiate(menuItemPrefab, transform);
                manager.menuSlots.Add(menuSlot.GetComponent<RadialMenuSlot>());
            }
        }
        // 갯수가 더 적으면 메뉴 아이템 제거
        else if (manager.menuSlots.Count > menuCount)
        {
            for (int i = manager.menuSlots.Count - 1; i >= menuCount; i--)
            {
                DestroyImmediate(manager.menuSlots[i]);
                manager.menuSlots.RemoveAt(i);
            }

        }

        // 각 메뉴 아이템의 회전 각도
        float angleStep = 360f / menuCount;
        float rad = (1f / menuCount) - space;

        // 메뉴 아이템 생성
        for (int i = 0; i < manager.menuSlots.Count; i++)
        {
            // 메뉴 아이템의 각도 계산
            float angle = i * angleStep;

            // Image 컴포넌트 가져오기
            Image image = manager.menuSlots[i].GetComponentInChildren<Image>();

            // 메뉴 아이템의 회전 설정
            image.transform.localRotation = Quaternion.Euler(0f, 0f, -angle);

            // fill amount 설정
            image.fillAmount = rad;

            // 메뉴 아이템에 번호 부여
            manager.menuSlots[i].name = (i + 1).ToString();
        }
    }

    #endregion
}

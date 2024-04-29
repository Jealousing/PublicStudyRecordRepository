using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialMenuSlotBuilder : MonoBehaviour
{
    public RadialMenuManager manager;

    /* MenuSlot Builder  */
    public GameObject menuItemPrefab; // ���� �޴� ������

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

    // + ��ư Ŭ�� �� ����Ǵ� �޼���
    public void IncreaseItemCount()
    {
        menuCount++;
        UpdateMenuItems();
    }

    // - ��ư Ŭ�� �� ����Ǵ� �޼���
    public void DecreaseItemCount()
    {
        if (menuCount > 2) // �ּ� 2���� �޴� �������� ������ ����
        {
            menuCount--;
            UpdateMenuItems();
        }
    }
    // ReSet ��ư Ŭ�� �� ����Ǵ� �޼���
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
        // ������ �� ������ �߰� �޴� ������ ����
        if (manager.menuSlots.Count < menuCount)
        {
            for (int i = manager.menuSlots.Count; i < menuCount; i++)
            {
                GameObject menuSlot = Instantiate(menuItemPrefab, transform);
                manager.menuSlots.Add(menuSlot.GetComponent<RadialMenuSlot>());
            }
        }
        // ������ �� ������ �޴� ������ ����
        else if (manager.menuSlots.Count > menuCount)
        {
            for (int i = manager.menuSlots.Count - 1; i >= menuCount; i--)
            {
                DestroyImmediate(manager.menuSlots[i]);
                manager.menuSlots.RemoveAt(i);
            }

        }

        // �� �޴� �������� ȸ�� ����
        float angleStep = 360f / menuCount;
        float rad = (1f / menuCount) - space;

        // �޴� ������ ����
        for (int i = 0; i < manager.menuSlots.Count; i++)
        {
            // �޴� �������� ���� ���
            float angle = i * angleStep;

            // Image ������Ʈ ��������
            Image image = manager.menuSlots[i].GetComponentInChildren<Image>();

            // �޴� �������� ȸ�� ����
            image.transform.localRotation = Quaternion.Euler(0f, 0f, -angle);

            // fill amount ����
            image.fillAmount = rad;

            // �޴� �����ۿ� ��ȣ �ο�
            manager.menuSlots[i].name = (i + 1).ToString();
        }
    }

    #endregion
}

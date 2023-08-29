using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������Ʈ�� Ž���ϴ� Ŭ����
/// </summary>
public class DetectObj : MonoBehaviour
{
    // ������ �±� �̸��� ��������
    [SerializeField] private string DetectTagName = ""; 
    public bool IsDetect; 

    // ������ ������Ʈ �� �ݶ��̴�
    public GameObject DetectObject;
    public Collider DetectCollder;

    private void Update()
    {
        // Ȥ�� �߻��� ���� ��Ȳ ó��
        if (DetectObject == null || !DetectCollder.enabled)
        {
            IsDetect = false;
        }
        if (DetectObject != null)
        {
            if (!DetectObject.activeInHierarchy)
            {
                IsDetect = false;
            }
        }
    }

    void OnTriggerStay(Collider col)
    {
        // �̹� �����Ǿ� ������ return
        if (IsDetect || col.CompareTag("Untagged")) 
            return;
        
        if (DetectTagName != "")
        {
            // ������ ��ũ�̸����� ���� �Ǿ��ִ��� Ȯ��
            if (col != null && !col.isTrigger && col.CompareTag(DetectTagName)) 
            {
                IsDetect = true;
                DetectObject = col.gameObject;
                DetectCollder = col;
            }
        }
        else if (DetectTagName == "" )
        {
            if (col != null && !col.isTrigger)
            {
                IsDetect = true;
                DetectCollder = col;
            }
        }

    }

    void OnTriggerExit(Collider col)
    {
        if (col == DetectCollder)
        {
            IsDetect = false;
        }
    }

}

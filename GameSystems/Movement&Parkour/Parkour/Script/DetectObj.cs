using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 오브젝트를 탐지하는 클래스
/// </summary>
public class DetectObj : MonoBehaviour
{
    // 감지할 태그 이름과 감지여부
    [SerializeField] private string DetectTagName = ""; 
    public bool IsDetect; 

    // 감지된 오브젝트 및 콜라이더
    public GameObject DetectObject;
    public Collider DetectCollder;

    private void Update()
    {
        // 혹시 발생할 예외 상황 처리
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
        // 이미 감지되어 있으면 return
        if (IsDetect || col.CompareTag("Untagged")) 
            return;
        
        if (DetectTagName != "")
        {
            // 감지할 태크이름으로 설정 되어있는지 확인
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 작성자: 서동주
// 카메라의 위치를 이동하게 만들 콜라이더 오브젝트의 스크립트
public class CameraCollider : MonoBehaviour
{
    // 데이터를 보내줄 카메라 메니저 스크립트 참조
    CameraManager m_CameraManager;
    void Start()
    {
        // 메인카메라를 찾아 할당된 CameraManager 정보를 가져옴
        m_CameraManager = GameObject.FindWithTag("MainCamera").GetComponent<CameraManager>();
    }

    // 해당 콜라이더에 충돌(들어오는 상태)가 되었을 경우
    private void OnTriggerEnter(Collider other)
    {
        // 그 충돌체가 플레이어 인 경우
        if (other.CompareTag("Player"))
        {
            // 카메라의 위치를 변경
            m_CameraManager.ChangeOfCamera(this.gameObject);
        }

    }
}

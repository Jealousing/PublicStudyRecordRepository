using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 작성자: 서동주
// 메인카메라에 사용할 스크립트
public class CameraManager : MonoBehaviour
{
    // 변수
    public Transform m_Target;                                     // 플레이어 Transform 참조용
    public Vector3 MotionCameraPlusPos;                     // 카메라 위치 보정 (Position)
    public Vector3 MotionCameraRotation;                    // 카메라 위치 보정 (Rotation)
    [SerializeField] CameraSetting[] m_CameraData;  // 카메라 셋팅 설정 변수
    int CameraCount;                                                    // 현재 참조하고있는 카메라 번호
    bool IsMotionSetting;                                               // Motion카메라 세팅 명령

    void Start()
    {
        CameraCount = 0;
        this.transform.position = m_CameraData[CameraCount].CameraPos.transform.position;
    }

    void Update()
    {
        CameraWork();
    }

    // 카메라 변경 (외부 콜라이더 명령)
    public void ChangeOfCamera(GameObject other)
    {
        // 설정된 카메라와 비교
        for (int i = 0; i < m_CameraData.Length; i++)
        {
            // 같은 오브젝트인지 확인
            if (ReferenceEquals(other, m_CameraData[i].DetectionCollider))
            {
                // 설정된 카메라랑 같은지 확인 
                if (CameraCount == i)
                    return;
                
                // 카메라 번호 설정
                CameraCount = i;

                // Motion용 카메라 이동 설정
                if (m_CameraData[i].CameraType==E_CameraType.Motion)
                {
                    IsMotionSetting = true;
                }
            }
        }
    }

    void CameraWork()
    {
        //카메라 타입에 따른 카메라 움직임 설정

        // 모션타입 카메라
        if (m_CameraData[CameraCount].CameraType == E_CameraType.Motion)
        {
            // 초기세팅
            if(IsMotionSetting)
            {
                // 설정된 위치로 이동 및 각도 설정
                this.transform.position = Vector3.Lerp(this.transform.position, m_Target.position+MotionCameraPlusPos, 
                    Time.deltaTime * m_CameraData[CameraCount].ChangeSpeed);
                this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.Euler(MotionCameraRotation.x, 
                    MotionCameraRotation.y, MotionCameraRotation.z), Time.deltaTime * m_CameraData[CameraCount].FollowSpeed);
                
                // 도착하면 false로 변경
                if (Vector3.Distance(this.transform.position, m_Target.position + MotionCameraPlusPos) <=0.05f)
                {
                    IsMotionSetting = false;
                }
            }
            else
            {
                // 설정이 되었으면 타겟을 따라 이동
                this.transform.position = m_Target.position + MotionCameraPlusPos;
                this.transform.rotation = Quaternion.Euler(MotionCameraRotation.x, MotionCameraRotation.y, MotionCameraRotation.z);
            }
            
        }
        // 고정타입 카메라 ( 설정된 위치에서 설정된 방향을 바라볼뿐 플레이어를 관찰하지 않는 카메라 )
        else if(m_CameraData[CameraCount].CameraType == E_CameraType.Stand)
        {
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.Euler(m_CameraData[CameraCount].StandAngle.x, 
                m_CameraData[CameraCount].StandAngle.y, m_CameraData[CameraCount].StandAngle.z), Time.deltaTime * m_CameraData[CameraCount].FollowSpeed);
            this.transform.position = Vector3.Lerp(this.transform.position, m_CameraData[CameraCount].CameraPos.transform.position, 
                Time.deltaTime * m_CameraData[CameraCount].ChangeSpeed);
        }
        // CCTV타입 카메라 ( 고정타입과 비슷하지만 플레이어를 바라봄 )
        else if (m_CameraData[CameraCount].CameraType == E_CameraType.CCTV)
        {
            Vector3 result = m_Target.transform.position - this.transform.position;
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(result), 
                Time.deltaTime * m_CameraData[CameraCount].FollowSpeed);
            this.transform.position = Vector3.Lerp(this.transform.position, 
                m_CameraData[CameraCount].CameraPos.transform.position, Time.deltaTime * m_CameraData[CameraCount].ChangeSpeed);
        }
        else
        {
            //없음
            Debug.Log("카메라 타입 오류");
        }
        
    }
}

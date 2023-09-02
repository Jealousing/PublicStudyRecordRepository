using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �ۼ���: ������
// ����ī�޶� ����� ��ũ��Ʈ
public class CameraManager : MonoBehaviour
{
    // ����
    public Transform m_Target;                                     // �÷��̾� Transform ������
    public Vector3 MotionCameraPlusPos;                     // ī�޶� ��ġ ���� (Position)
    public Vector3 MotionCameraRotation;                    // ī�޶� ��ġ ���� (Rotation)
    [SerializeField] CameraSetting[] m_CameraData;  // ī�޶� ���� ���� ����
    int CameraCount;                                                    // ���� �����ϰ��ִ� ī�޶� ��ȣ
    bool IsMotionSetting;                                               // Motionī�޶� ���� ���

    void Start()
    {
        CameraCount = 0;
        this.transform.position = m_CameraData[CameraCount].CameraPos.transform.position;
    }

    void Update()
    {
        CameraWork();
    }

    // ī�޶� ���� (�ܺ� �ݶ��̴� ���)
    public void ChangeOfCamera(GameObject other)
    {
        // ������ ī�޶�� ��
        for (int i = 0; i < m_CameraData.Length; i++)
        {
            // ���� ������Ʈ���� Ȯ��
            if (ReferenceEquals(other, m_CameraData[i].DetectionCollider))
            {
                // ������ ī�޶�� ������ Ȯ�� 
                if (CameraCount == i)
                    return;
                
                // ī�޶� ��ȣ ����
                CameraCount = i;

                // Motion�� ī�޶� �̵� ����
                if (m_CameraData[i].CameraType==E_CameraType.Motion)
                {
                    IsMotionSetting = true;
                }
            }
        }
    }

    void CameraWork()
    {
        //ī�޶� Ÿ�Կ� ���� ī�޶� ������ ����

        // ���Ÿ�� ī�޶�
        if (m_CameraData[CameraCount].CameraType == E_CameraType.Motion)
        {
            // �ʱ⼼��
            if(IsMotionSetting)
            {
                // ������ ��ġ�� �̵� �� ���� ����
                this.transform.position = Vector3.Lerp(this.transform.position, m_Target.position+MotionCameraPlusPos, 
                    Time.deltaTime * m_CameraData[CameraCount].ChangeSpeed);
                this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.Euler(MotionCameraRotation.x, 
                    MotionCameraRotation.y, MotionCameraRotation.z), Time.deltaTime * m_CameraData[CameraCount].FollowSpeed);
                
                // �����ϸ� false�� ����
                if (Vector3.Distance(this.transform.position, m_Target.position + MotionCameraPlusPos) <=0.05f)
                {
                    IsMotionSetting = false;
                }
            }
            else
            {
                // ������ �Ǿ����� Ÿ���� ���� �̵�
                this.transform.position = m_Target.position + MotionCameraPlusPos;
                this.transform.rotation = Quaternion.Euler(MotionCameraRotation.x, MotionCameraRotation.y, MotionCameraRotation.z);
            }
            
        }
        // ����Ÿ�� ī�޶� ( ������ ��ġ���� ������ ������ �ٶ󺼻� �÷��̾ �������� �ʴ� ī�޶� )
        else if(m_CameraData[CameraCount].CameraType == E_CameraType.Stand)
        {
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.Euler(m_CameraData[CameraCount].StandAngle.x, 
                m_CameraData[CameraCount].StandAngle.y, m_CameraData[CameraCount].StandAngle.z), Time.deltaTime * m_CameraData[CameraCount].FollowSpeed);
            this.transform.position = Vector3.Lerp(this.transform.position, m_CameraData[CameraCount].CameraPos.transform.position, 
                Time.deltaTime * m_CameraData[CameraCount].ChangeSpeed);
        }
        // CCTVŸ�� ī�޶� ( ����Ÿ�԰� ��������� �÷��̾ �ٶ� )
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
            //����
            Debug.Log("ī�޶� Ÿ�� ����");
        }
        
    }
}

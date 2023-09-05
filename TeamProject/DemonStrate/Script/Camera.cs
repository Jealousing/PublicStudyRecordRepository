using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 작성자: 서동주
// 임시카메라
public class Define
{
    public enum CameraMode
    {
        QuaterView,
    }
}
public class Camera : MonoBehaviour
{
    //카메라 타겟위치
    public Transform m_Target;
    //카메라 위치
    private Transform m_CameraTr;
    //카메라 움직임 딜레이타임
    public float m_DelayTime = 2f;

    public float height = 5.0f;
    public float dist = 10.0f;
    public float turnSpeed = 4.0f; // 마우스 회전 속도    
    private float xRotate = 0.0f; // 내부 사용할 X축 회전량은 별도 정의 ( 카메라 위 아래 방향 )
    public float moveSpeed = 4.0f; // 이동 속도
    public float c_rot = 30f; // 각도

    [SerializeField]
    Vector3 delta;

    [SerializeField]
    Define.CameraMode mode = Define.CameraMode.QuaterView;

    void Start()
    {
        //카메라 위치정보 얻어옴
        m_CameraTr = GetComponent<Transform>();
    }

    void Update()
    {
        //카메라 이동부분
        Vector3 Pos = new Vector3(m_Target.position.x, m_Target.position.y + height, m_Target.position.z - dist);
        m_CameraTr.position = Pos;//Vector3.Lerp(transform.position, Pos, Time.deltaTime * m_DelayTime);

        if (Input.GetMouseButton(0))
        {
            // 좌우로 움직인 마우스의 이동량 * 속도에 따라 카메라가 좌우로 회전할 양 계산
            float yRotateSize = Input.GetAxis("Mouse X") * turnSpeed;
            // 현재 y축 회전값에 더한 새로운 회전각도 계산
            float yRotate = transform.eulerAngles.y + yRotateSize;

            // 위아래로 움직인 마우스의 이동량 * 속도에 따라 카메라가 회전할 양 계산(하늘, 바닥을 바라보는 동작)
            float xRotateSize = -Input.GetAxis("Mouse Y") * turnSpeed;

            // Clamp 는 값의 범위를 제한하는 함수
            xRotate = Mathf.Clamp(xRotate + xRotateSize, -45, 80);

            // 카메라 회전량을 카메라에 반영(X, Y축만 회전)
            transform.eulerAngles = new Vector3(xRotate, yRotate, 0);

        }
        else
        {
            transform.eulerAngles = new Vector3(c_rot, 0, 0);
        }

        // 이동량을 좌표에 반영
        //transform.position += move * moveSpeed * Time.deltaTime;

        if (mode == Define.CameraMode.QuaterView)
        {
            RaycastHit hit;
            if (Physics.Raycast(m_Target.transform.position, delta, out hit, delta.magnitude, LayerMask.GetMask("Wall")))
            {
                float dist = (hit.point - m_Target.transform.position).magnitude * 0.8f;
                transform.position = m_Target.transform.position + delta.normalized * dist;
            }
            else
            {
                transform.position = m_Target.transform.position + delta;
                transform.LookAt(m_Target.transform);
            }
        }
    }

}

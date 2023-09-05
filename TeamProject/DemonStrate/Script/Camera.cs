using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// �ۼ���: ������
// �ӽ�ī�޶�
public class Define
{
    public enum CameraMode
    {
        QuaterView,
    }
}
public class Camera : MonoBehaviour
{
    //ī�޶� Ÿ����ġ
    public Transform m_Target;
    //ī�޶� ��ġ
    private Transform m_CameraTr;
    //ī�޶� ������ ������Ÿ��
    public float m_DelayTime = 2f;

    public float height = 5.0f;
    public float dist = 10.0f;
    public float turnSpeed = 4.0f; // ���콺 ȸ�� �ӵ�    
    private float xRotate = 0.0f; // ���� ����� X�� ȸ������ ���� ���� ( ī�޶� �� �Ʒ� ���� )
    public float moveSpeed = 4.0f; // �̵� �ӵ�
    public float c_rot = 30f; // ����

    [SerializeField]
    Vector3 delta;

    [SerializeField]
    Define.CameraMode mode = Define.CameraMode.QuaterView;

    void Start()
    {
        //ī�޶� ��ġ���� ����
        m_CameraTr = GetComponent<Transform>();
    }

    void Update()
    {
        //ī�޶� �̵��κ�
        Vector3 Pos = new Vector3(m_Target.position.x, m_Target.position.y + height, m_Target.position.z - dist);
        m_CameraTr.position = Pos;//Vector3.Lerp(transform.position, Pos, Time.deltaTime * m_DelayTime);

        if (Input.GetMouseButton(0))
        {
            // �¿�� ������ ���콺�� �̵��� * �ӵ��� ���� ī�޶� �¿�� ȸ���� �� ���
            float yRotateSize = Input.GetAxis("Mouse X") * turnSpeed;
            // ���� y�� ȸ������ ���� ���ο� ȸ������ ���
            float yRotate = transform.eulerAngles.y + yRotateSize;

            // ���Ʒ��� ������ ���콺�� �̵��� * �ӵ��� ���� ī�޶� ȸ���� �� ���(�ϴ�, �ٴ��� �ٶ󺸴� ����)
            float xRotateSize = -Input.GetAxis("Mouse Y") * turnSpeed;

            // Clamp �� ���� ������ �����ϴ� �Լ�
            xRotate = Mathf.Clamp(xRotate + xRotateSize, -45, 80);

            // ī�޶� ȸ������ ī�޶� �ݿ�(X, Y�ุ ȸ��)
            transform.eulerAngles = new Vector3(xRotate, yRotate, 0);

        }
        else
        {
            transform.eulerAngles = new Vector3(c_rot, 0, 0);
        }

        // �̵����� ��ǥ�� �ݿ�
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

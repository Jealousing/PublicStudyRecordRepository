using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �ۼ���: ������
// ī�޶��� ��ġ�� �̵��ϰ� ���� �ݶ��̴� ������Ʈ�� ��ũ��Ʈ
public class CameraCollider : MonoBehaviour
{
    // �����͸� ������ ī�޶� �޴��� ��ũ��Ʈ ����
    CameraManager m_CameraManager;
    void Start()
    {
        // ����ī�޶� ã�� �Ҵ�� CameraManager ������ ������
        m_CameraManager = GameObject.FindWithTag("MainCamera").GetComponent<CameraManager>();
    }

    // �ش� �ݶ��̴��� �浹(������ ����)�� �Ǿ��� ���
    private void OnTriggerEnter(Collider other)
    {
        // �� �浹ü�� �÷��̾� �� ���
        if (other.CompareTag("Player"))
        {
            // ī�޶��� ��ġ�� ����
            m_CameraManager.ChangeOfCamera(this.gameObject);
        }

    }
}

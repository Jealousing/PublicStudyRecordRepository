using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �ۼ���: ������
public class SlaveCognizance : MonoBehaviour
{
    //���� ������ ������ �÷��̾� ������
    public SlaveAI m_Slave = null;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //�߰��ؾߵ� �̺�Ʈ
            Debug.Log("�÷��̾ �������ϴ�.");
            //m_Slave.m_StopSlave = true;
            //Player_Control.GetInstance.m_PlayerState = E_PlayerState.Die;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
        }
    }
}

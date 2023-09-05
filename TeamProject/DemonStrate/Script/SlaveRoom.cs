using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �ۼ���: ������
public class SlaveRoom : MonoBehaviour
{
    //�ش� ���� ������ npc ����
    public SlaveAI m_Slave = null;
    [Range(0, 10)]
    [Tooltip("�ش� ������ ���� �����̽ð�.")] public float m_DelayTime;

    //������ �濡 ������ �÷��̾� Ȯ�� �� �����ҹ� ����
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Invoke("setPatrolRoom", m_DelayTime);
        }

    }

    void setPatrolRoom()
    {
        m_Slave.ChangeOfPatrolRoom(this.gameObject);
    }
}

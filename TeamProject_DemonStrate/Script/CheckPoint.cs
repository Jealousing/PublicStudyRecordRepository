using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �ۼ���: ������
public class CheckPoint : MonoBehaviour
{
    //npc ����
    public SlaveAI m_Slave = null;
    public bool StartEnd;

    private void OnTriggerEnter(Collider other)
    {
        if(StartEnd)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("�뿹�ý��۽���");
                m_Slave.StartFlag();
            }
        }
       else
        {
            if (other.CompareTag("Player"))
            {
                m_Slave.EndFlag();
            }
        }

    }
}

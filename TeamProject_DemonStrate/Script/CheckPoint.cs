using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 작성자: 서동주
public class CheckPoint : MonoBehaviour
{
    //npc 정보
    public SlaveAI m_Slave = null;
    public bool StartEnd;

    private void OnTriggerEnter(Collider other)
    {
        if(StartEnd)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("노예시스템시작");
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 작성자: 서동주
public class SlaveCognizance : MonoBehaviour
{
    //일정 범위에 들어오는 플레이어 감지용
    public SlaveAI m_Slave = null;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //추가해야됨 이벤트
            Debug.Log("플레이어가 잡혔습니다.");
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

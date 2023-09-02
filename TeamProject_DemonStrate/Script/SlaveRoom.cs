using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 작성자: 서동주
public class SlaveRoom : MonoBehaviour
{
    //해당 방을 순찰할 npc 정보
    public SlaveAI m_Slave = null;
    [Range(0, 10)]
    [Tooltip("해당 방으로 가는 딜레이시간.")] public float m_DelayTime;

    //순찰할 방에 들어오는 플레이어 확인 및 순찰할방 설정
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

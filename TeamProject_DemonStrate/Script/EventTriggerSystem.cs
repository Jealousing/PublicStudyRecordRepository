using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// 작성자: 서동주
public class EventTriggerSystem : MonoBehaviour
{
    // 참조할 npc
    public SlaveAI m_Slave = null;

    //Event 설정
    [Tooltip("특정 위치에 도달하면 물건이 떨어지는 이벤트")] public EventTriggerSet_1 m_EventTriggerSet_1;
    //[Tooltip("환풍구가 돌아가면 환풍구를 끄는 이벤트")] public EventTriggerSet_2 m_EventTriggerSet_2; // 진행x

    [Serializable]
    public class EventTriggerSet_1
    {
        [Tooltip("날라갈 오브젝트설정.")] public GameObject obj;

        [Range(1, 100)]
        [Tooltip("날라갈 파워설정.")] public float power;

        [Range(-1, 1)]
        [Tooltip("날라갈 X방향설정.")] public int directionX;
        [Range(-1, 1)]
        [Tooltip("날라갈 Z방향설정.")] public int directionZ;
    }

    public void EventTriggerStart_1()
    {
        m_EventTriggerSet_1.obj.transform.GetComponent<Rigidbody>().AddForce
            (new Vector3(m_EventTriggerSet_1.directionX, 1, m_EventTriggerSet_1.directionZ)* m_EventTriggerSet_1.power);
    }

    [Serializable]
    public class EventTriggerSet_2
    {
        [Tooltip("환풍구 스위치를 작동할 위치")] public Transform Pos;
        [Tooltip("환풍구 오브젝트")] public GameObject Vent;
    }

    public void EventTriggerStart_2()
    {
        //m_Slave.SetTarget(m_EventTriggerSet_2.Pos);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// �ۼ���: ������
public class EventTriggerSystem : MonoBehaviour
{
    // ������ npc
    public SlaveAI m_Slave = null;

    //Event ����
    [Tooltip("Ư�� ��ġ�� �����ϸ� ������ �������� �̺�Ʈ")] public EventTriggerSet_1 m_EventTriggerSet_1;
    //[Tooltip("ȯǳ���� ���ư��� ȯǳ���� ���� �̺�Ʈ")] public EventTriggerSet_2 m_EventTriggerSet_2; // ����x

    [Serializable]
    public class EventTriggerSet_1
    {
        [Tooltip("���� ������Ʈ����.")] public GameObject obj;

        [Range(1, 100)]
        [Tooltip("���� �Ŀ�����.")] public float power;

        [Range(-1, 1)]
        [Tooltip("���� X���⼳��.")] public int directionX;
        [Range(-1, 1)]
        [Tooltip("���� Z���⼳��.")] public int directionZ;
    }

    public void EventTriggerStart_1()
    {
        m_EventTriggerSet_1.obj.transform.GetComponent<Rigidbody>().AddForce
            (new Vector3(m_EventTriggerSet_1.directionX, 1, m_EventTriggerSet_1.directionZ)* m_EventTriggerSet_1.power);
    }

    [Serializable]
    public class EventTriggerSet_2
    {
        [Tooltip("ȯǳ�� ����ġ�� �۵��� ��ġ")] public Transform Pos;
        [Tooltip("ȯǳ�� ������Ʈ")] public GameObject Vent;
    }

    public void EventTriggerStart_2()
    {
        //m_Slave.SetTarget(m_EventTriggerSet_2.Pos);
    }

}

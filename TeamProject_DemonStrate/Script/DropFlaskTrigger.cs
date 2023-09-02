using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 작성자: 서동주
public class DropFlaskTrigger : MonoBehaviour
{
    bool Isuse = false;
    public EventTriggerSystem Trigger;
    public void OnTriggerEnter(Collider other)
    {
        if(!Isuse)
        {
            if (other.CompareTag("Player"))
            {
                Isuse = true;
                this.GetComponentInChildren<DropFlask>().IsDrop = true;
                Trigger.EventTriggerStart_1();
            }
        }
    }
}

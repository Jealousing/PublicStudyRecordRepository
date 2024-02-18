using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuikaGame3D
{
    // 과일이 떨어졌을 경우에 대한 스크립트
    public class SuikaGameDeadLine : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent<SuikaGameFruit>(out SuikaGameFruit temp))
            {
                SuikaGameManager.GetInstance.GameOver();
            }
        }
    }
}



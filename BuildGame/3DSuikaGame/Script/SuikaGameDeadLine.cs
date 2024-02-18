using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuikaGame3D
{
    // ������ �������� ��쿡 ���� ��ũ��Ʈ
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



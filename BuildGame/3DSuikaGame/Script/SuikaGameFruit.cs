using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace SuikaGame3D
{
    // ���� ������ ���� ��ũ��Ʈ
    public class SuikaGameFruit : MonoBehaviour
    {
        // ������ٵ�, �ݶ��̴�����, ���Ϲ�ȣ, ������ȣ, ������Ʈ Ǯ
        public Rigidbody rigid;
        public Collider colliderInfo;
        public Fruit fruit;
        public int numbering;
        IObjectPool<GameObject> pool;
        public bool trigger = false;

        // ������Ʈ Ǯ�� �� ���Ͽ� ���� ����
        public void Set(IObjectPool<GameObject> pool,int numbering)
        {
            this.pool = pool;
            this.numbering = numbering;
            rigid.isKinematic = true;
            rigid.useGravity = false;
            colliderInfo.isTrigger = true;
        }

        // ���� �߻�
        public void Shoot()
        {
            trigger = true;
            colliderInfo.isTrigger = false;
            rigid.isKinematic = false;
            rigid.useGravity = true;
        }

        // ���� ��ȯ
        public void Release(bool addScore)
        {
            if (!trigger) return;

            trigger = false;
            rigid.useGravity = false;
            if(addScore) SuikaGameManager.GetInstance.AddScore(fruit);
            pool.Release(this.gameObject);
        }

        // �浹�� ���� ���� ó��
        private void OnCollisionEnter(Collision collision)
        {
            if( fruit < Fruit.Watermelon &&trigger && collision.gameObject.TryGetComponent<SuikaGameFruit>(out SuikaGameFruit temp) && temp.fruit==fruit 
                &&temp.numbering>numbering && temp.trigger)
            {
                SuikaGameFruit createdFruit = SuikaGameManager.GetInstance.GetDictionaryPool((int)fruit + 1).GetComponent<SuikaGameFruit>();
                createdFruit.transform.SetParent(SuikaGameManager.GetInstance.transform.GetChild(0));
                createdFruit.Shoot();
                createdFruit.transform.localPosition = this.transform.localPosition + new Vector3(0, 0.25f, 0);

                temp.Release(true);
                Release(true);
            }
        }
    }
}



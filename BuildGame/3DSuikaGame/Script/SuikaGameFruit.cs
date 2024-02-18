using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace SuikaGame3D
{
    // 과일 정보에 대한 스크립트
    public class SuikaGameFruit : MonoBehaviour
    {
        // 리지드바디, 콜라이더정보, 과일번호, 생성번호, 오브젝트 풀
        public Rigidbody rigid;
        public Collider colliderInfo;
        public Fruit fruit;
        public int numbering;
        IObjectPool<GameObject> pool;
        public bool trigger = false;

        // 오브젝트 풀링 및 과일에 대한 설정
        public void Set(IObjectPool<GameObject> pool,int numbering)
        {
            this.pool = pool;
            this.numbering = numbering;
            rigid.isKinematic = true;
            rigid.useGravity = false;
            colliderInfo.isTrigger = true;
        }

        // 과일 발사
        public void Shoot()
        {
            trigger = true;
            colliderInfo.isTrigger = false;
            rigid.isKinematic = false;
            rigid.useGravity = true;
        }

        // 과일 반환
        public void Release(bool addScore)
        {
            if (!trigger) return;

            trigger = false;
            rigid.useGravity = false;
            if(addScore) SuikaGameManager.GetInstance.AddScore(fruit);
            pool.Release(this.gameObject);
        }

        // 충돌에 의한 과일 처리
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



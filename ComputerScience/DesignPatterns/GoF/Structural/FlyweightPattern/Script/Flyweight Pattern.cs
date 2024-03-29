using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlyweightPattern 
{
    // 플라이웨이트 패턴을 구현하기 위한 인터페이스
    interface IWeapon
    {
        void Fire(Vector3 position);
    }

    // 플라이웨이트 객체 (공유 객체)
    class SharedWeapon : MonoBehaviour, IWeapon
    {
        private GameObject bulletPrefab;

        public SharedWeapon(GameObject bulletPrefab)
        {
            this.bulletPrefab = bulletPrefab;
        }

        public void Fire(Vector3 position)
        {
            Debug.Log("공유된 무기로 발사: " + bulletPrefab.name);
            // 총알 발사 로직
            Instantiate(bulletPrefab, position, Quaternion.identity);
        }
    }

    // 플라이웨이트 팩토리
    class WeaponFactory
    {
        private Dictionary<string, IWeapon> sharedWeapons = new Dictionary<string, IWeapon>();

        public IWeapon GetSharedWeapon(string key, GameObject bulletPrefab)
        {
            if (!sharedWeapons.ContainsKey(key))
            {
                sharedWeapons[key] = new SharedWeapon(bulletPrefab);
            }
            return sharedWeapons[key];
        }
    }

    // 비공유 객체 (유니크한 외부 상태를 갖는 객체)
    class UniqueWeapon : MonoBehaviour, IWeapon
    {
        private string name;

        public UniqueWeapon(string name)
        {
            this.name = name;
        }

        public void Fire(Vector3 position)
        {
            Debug.Log("유니크한 무기로 발사: " + name);
            // 총알 발사 로직
            GameObject bulletPrefab = Resources.Load<GameObject>("BulletPrefabs/" + name);
            if (bulletPrefab != null)
            {
                Instantiate(bulletPrefab, position, Quaternion.identity);
            }
            else
            {
                Debug.LogError("총알 프리팹을 찾을 수 없습니다: " + name);
            }
        }
    }

    public class GameManager : MonoBehaviour
    {
        void Start()
        {
            WeaponFactory factory = new WeaponFactory();

            // 공유된 무기 사용 예시
            IWeapon sharedWeapon = factory.GetSharedWeapon("shared", Resources.Load<GameObject>("BulletPrefabs/Bullet"));
            sharedWeapon.Fire(Vector3.zero);
            sharedWeapon.Fire(Vector3.up);

            // 유니크한 무기 사용 예시
            IWeapon uniqueWeapon1 = new UniqueWeapon("Gun");
            uniqueWeapon1.Fire(Vector3.forward);

            IWeapon uniqueWeapon2 = new UniqueWeapon("Missile");
            uniqueWeapon2.Fire(Vector3.right);
        }
    }
}

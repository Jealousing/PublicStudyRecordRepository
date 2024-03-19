using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlyweightPattern 
{
    // �ö��̿���Ʈ ������ �����ϱ� ���� �������̽�
    interface IWeapon
    {
        void Fire(Vector3 position);
    }

    // �ö��̿���Ʈ ��ü (���� ��ü)
    class SharedWeapon : MonoBehaviour, IWeapon
    {
        private GameObject bulletPrefab;

        public SharedWeapon(GameObject bulletPrefab)
        {
            this.bulletPrefab = bulletPrefab;
        }

        public void Fire(Vector3 position)
        {
            Debug.Log("������ ����� �߻�: " + bulletPrefab.name);
            // �Ѿ� �߻� ����
            Instantiate(bulletPrefab, position, Quaternion.identity);
        }
    }

    // �ö��̿���Ʈ ���丮
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

    // ����� ��ü (����ũ�� �ܺ� ���¸� ���� ��ü)
    class UniqueWeapon : MonoBehaviour, IWeapon
    {
        private string name;

        public UniqueWeapon(string name)
        {
            this.name = name;
        }

        public void Fire(Vector3 position)
        {
            Debug.Log("����ũ�� ����� �߻�: " + name);
            // �Ѿ� �߻� ����
            GameObject bulletPrefab = Resources.Load<GameObject>("BulletPrefabs/" + name);
            if (bulletPrefab != null)
            {
                Instantiate(bulletPrefab, position, Quaternion.identity);
            }
            else
            {
                Debug.LogError("�Ѿ� �������� ã�� �� �����ϴ�: " + name);
            }
        }
    }

    public class GameManager : MonoBehaviour
    {
        void Start()
        {
            WeaponFactory factory = new WeaponFactory();

            // ������ ���� ��� ����
            IWeapon sharedWeapon = factory.GetSharedWeapon("shared", Resources.Load<GameObject>("BulletPrefabs/Bullet"));
            sharedWeapon.Fire(Vector3.zero);
            sharedWeapon.Fire(Vector3.up);

            // ����ũ�� ���� ��� ����
            IWeapon uniqueWeapon1 = new UniqueWeapon("Gun");
            uniqueWeapon1.Fire(Vector3.forward);

            IWeapon uniqueWeapon2 = new UniqueWeapon("Missile");
            uniqueWeapon2.Fire(Vector3.right);
        }
    }
}

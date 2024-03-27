using UnityEngine;

namespace FactoryPattern
{
    public enum MonsterType
    {
        Valtan,
        Vykas,
        KakulSaydon,
        Brelshaza,
        Akkan,
        Thaemine
    }

    // �������丮�� �׽�Ʈ
    public class FactoryManager : MonoBehaviour
    {
        MonsterFactory monsterFactory = null;
        GameObject monster1;

        private void Start()
        {
            monsterFactory = new MonsterFactory();

            // ���� ����
            monster1 = monsterFactory.CreateMonster(MonsterType.Valtan);

            // �߻�Ŭ���� �������� ������ ������ ������� ���� ��� ���
            monster1.GetComponent<Monster>().Attack();
        }
    }

    // ���丮 ������ �ٽ� ��Ȱ ����ó�� ���ϴ� ���� Ÿ���� ���� ������ �ش� ���� ��ü�� �����ؼ� ��ȯ�ϴ� Ŭ����
    public class MonsterFactory : MonoBehaviour
    {
        public GameObject valtanPrefab;

        public GameObject CreateMonster(MonsterType type)
        {
            GameObject monster = null;

            switch (type)
            {
                case MonsterType.Valtan:
                    monster = Instantiate(valtanPrefab, RandomVector(), Quaternion.identity);
                    break;
                /*
            case MonsterType.Vykas:
                monster = new Vykas();
                break;
            case MonsterType.KakulSaydon:
                monster = new KakulSaydon();
                break;
            case MonsterType.Brelshaza:
                monster = new Brelshaza();
                break;
            case MonsterType.Akkan:
                monster = new Akkan();
                break;
            case MonsterType.Thaemine:
                monster = new Thaemine();
                break;
                */
                default:
                    Debug.LogError("Invalid monster type: " + type);
                    break;
            }
            return monster;
        }

        Vector3 RandomVector()
        {
            float minX = -10f;
            float maxX = 10f;
            float minY = -10f;
            float maxY = 10f;
            float minZ = -10f;
            float maxZ = 10f;

            float randomX = Random.Range(minX, maxX);
            float randomY = Random.Range(minY, maxY);
            float randomZ = Random.Range(minZ, maxZ);

            return new Vector3(randomX, randomY, randomZ);
        }

    }

    // �پ��� ������ ������ �߻� Ŭ���� ���� ��� ����
    public abstract class Monster : MonoBehaviour
    {
        public abstract void Attack();
        public abstract void Move();
        public abstract void Die();
    }

    #region Monster�� ����ϴ� ������ ������ ������

    public class Valtan : Monster
    {
        void Start()
        {
            Debug.Log("Create Valtan");
        }
        public override void Attack()
        {
            Debug.Log("Valtan Attack");
        }
        public override void Move()
        {
            Debug.Log("Valtan Move");
        }
        public override void Die()
        {
            Debug.Log("Valtan Die");
        }
    }

    public class Vykas : Monster
    {
        void Start()
        {
            Debug.Log("Create Vykas");
        }
        public override void Attack()
        {
            Debug.Log("Vykas Attack");
        }
        public override void Move()
        {
            Debug.Log("Vykas Move");
        }
        public override void Die()
        {
            Debug.Log("Vykas Die");
        }
    }
    public class KakulSaydon : Monster
    {
        void Start()
        {
            Debug.Log("Create KakulSaydon");
        }
        public override void Attack()
        {
            Debug.Log("KakulSaydon Attack");
        }
        public override void Move()
        {
            Debug.Log("KakulSaydon Move");
        }
        public override void Die()
        {
            Debug.Log("KakulSaydon Die");
        }
    }
    public class Brelshaza : Monster
    {
        void Start()
        {
            Debug.Log("Create Brelshaza");
        }
        public override void Attack()
        {
            Debug.Log("Brelshaza Attack");
        }
        public override void Move()
        {
            Debug.Log("Brelshaza Move");
        }
        public override void Die()
        {
            Debug.Log("Brelshaza Die");
        }
    }
    public class Akkan : Monster
    {
        void Start()
        {
            Debug.Log("Create Akkan");
        }
        public override void Attack()
        {
            Debug.Log("Akkan Attack");
        }
        public override void Move()
        {
            Debug.Log("Akkan Move");
        }
        public override void Die()
        {
            Debug.Log("Akkan Die");
        }
    }
    public class Thaemine : Monster
    {
        void Start()
        {
            Debug.Log("Create Thaemine");
        }
        public override void Attack()
        {
            Debug.Log("Thaemine Attack");
        }
        public override void Move()
        {
            Debug.Log("Thaemine Move");
        }
        public override void Die()
        {
            Debug.Log("Thaemine Die");
        }
    }

    #endregion

}

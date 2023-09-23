using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AbstractFactoryPattern
{
    // �߻� ��ǰ(�ϼ�ǰ) �������̽�
    public interface IMonster
    {
        void Attack();
        void Move();
    }
    // �߻� ���丮(����) �������̽�
    public interface IMonsterFactory
    {
        IMonster CreateMonster();
    }


    #region Factory
    public class ValtanFactory : IMonsterFactory
    {
        public IMonster CreateMonster()
        {
            return new Valtan();
        }
    }

    public class VykasFactory : IMonsterFactory
    {
        public IMonster CreateMonster()
        {
            return new Vykas();
        }
    }

    #endregion

    #region Monster
    public class Valtan : IMonster
    {
        public void Attack()
        {
            Debug.Log("Valtan attack");
        }

        public void Move()
        {
            Debug.Log("Valtan move");
        }
    }
    public class Vykas : IMonster
    {
        public void Attack()
        {
            Debug.Log("Vykas attack");
        }

        public void Move()
        {
            Debug.Log("Vykas move");
        }
    }
    #endregion

    public class MonsterSpawner : MonoBehaviour
    {
        private IMonsterFactory monsterFactory;

        void Start()
        {
            // � ���� �������� ����
            monsterFactory = new VykasFactory();

            // ���丮�� ����Ͽ� ���� ����
            IMonster monster = monsterFactory.CreateMonster();

            // ���
            monster.Attack();
            monster.Move();
        }
    }
}
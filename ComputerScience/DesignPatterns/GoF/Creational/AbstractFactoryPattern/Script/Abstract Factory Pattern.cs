using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AbstractFactoryPattern
{
    // 추상 제품(완성품) 인터페이스
    public interface IMonster
    {
        void Attack();
        void Move();
    }
    // 추상 팩토리(공장) 인터페이스
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
            // 어떤 적을 생성할지 선택
            monsterFactory = new VykasFactory();

            // 팩토리를 사용하여 적을 생성
            IMonster monster = monsterFactory.CreateMonster();

            // 명령
            monster.Attack();
            monster.Move();
        }
    }
}
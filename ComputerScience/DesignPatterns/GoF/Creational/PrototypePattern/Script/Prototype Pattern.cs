using UnityEngine;

namespace PrototypePattern
{
    public interface IMonster 
    {
        void Attack();
        void Move();
        IMonster Clone();
    }

    public class MonsterType1 : MonoBehaviour, IMonster
    {
        public void Attack() => Debug.Log("type1 attack");
        public void Move() => Debug.Log("type1 move");
        public IMonster Clone()
        {
            //return Instantiate(this);
            return this.MemberwiseClone() as IMonster;
        }
    }
    public class MonsterType2 : MonoBehaviour, IMonster
    {
        public void Attack() => Debug.Log("type2 attack");
        public void Move() => Debug.Log("type2 move");
        public IMonster Clone()
        {
            return Instantiate(this);
            //return this.MemberwiseClone() as IMonster;
        }
    }
    public class MonsterPrototypeManager : MonoBehaviour
    {
        public IMonster MonsterPrototype1;
        public IMonster MonsterPrototype2;

        public IMonster CloneMonster1()
        {
            return MonsterPrototype1.Clone();
        }

        public IMonster CloneMonster2()
        {
            return MonsterPrototype2.Clone();
        }
    }

    public class GameManager : MonoBehaviour
    {
        public MonsterPrototypeManager prototypeManager;

        void Start()
        {
            // 인스턴스를 생성
            IMonster monster1 = prototypeManager.CloneMonster1();
            IMonster monster2 = prototypeManager.CloneMonster2();

            // 동작 테스트
            monster1.Move();
            monster1.Attack();

            monster2.Move();
            monster2.Attack();
        }
    }
}
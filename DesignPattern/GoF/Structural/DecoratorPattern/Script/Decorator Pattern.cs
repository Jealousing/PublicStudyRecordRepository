using UnityEngine;

namespace DecoratorPattern
{
    // Component Interface
    public interface ICharacter
    {
        void DisplayStats(); // 캐릭터의 스텟를 표시하는 메서드
    }

    // 기준이 될 캐릭터정보
    public class BasicCharacter : ICharacter
    {
        public void DisplayStats()
        {
            Debug.Log("기본 캐릭터: 체력 100, 공격력 10, 방어력 5");
        }
    }

    // Decorator Base
    public abstract class CharacterDecorator : ICharacter
    {
        protected ICharacter character;

        public CharacterDecorator(ICharacter character)
        {
            this.character = character;
        }

        public virtual void DisplayStats()
        {
            character.DisplayStats();
        }
    }

    // 체력추가 데코
    public class HealthBoostDecorator : CharacterDecorator
    {
        public HealthBoostDecorator(ICharacter character) : base(character) { }

        public override void DisplayStats()
        {
            base.DisplayStats();
            Debug.Log("체력 상승: +50");
        }
    }

    // 공격력 추가 데코
    public class AttackBoostDecorator : CharacterDecorator
    {
        public AttackBoostDecorator(ICharacter character) : base(character) { }

        public override void DisplayStats()
        {
            base.DisplayStats();
            Debug.Log("공격력 상승: +20");
        }
    }

    public class GameManager : MonoBehaviour
    {
        void Start()
        {
            // 기본캐릭터 생성 및 정보 출력
            ICharacter basicCharacter = new BasicCharacter();
            basicCharacter.DisplayStats();

            Debug.Log("---");

            // 체력이 추가된 정보 출력
            ICharacter characterWithHealthBoost = new HealthBoostDecorator(basicCharacter);
            characterWithHealthBoost.DisplayStats();

            Debug.Log("---");

            //공격력이 추가된 정보 출력
            ICharacter characterWithAttackBoost = new AttackBoostDecorator(basicCharacter);
            characterWithAttackBoost.DisplayStats();

            Debug.Log("---");

            // 체력과 공격력이 추가된 정보 출력
            ICharacter characterWithBothBoosts = new AttackBoostDecorator(new HealthBoostDecorator(basicCharacter));
            characterWithBothBoosts.DisplayStats();
        }
    }
}
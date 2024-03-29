using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StrategyPattern
{
    // 전략 인터페이스
    public interface IAttackStrategy
    {
        void Attack();
    }

    // 구체적인 전략 클래스들
    public class MeleeAttack : IAttackStrategy
    {
        public void Attack()
        {
            Debug.Log("근접 공격!");
        }
    }

    public class RangedAttack : IAttackStrategy
    {
        public void Attack()
        {
            Debug.Log("원거리 공격!");
        }
    }

    // 캐릭터 클래스
    public class Character : MonoBehaviour
    {
        private IAttackStrategy attackStrategy;

        // 전략 설정
        public void SetAttackStrategy(IAttackStrategy strategy)
        {
            attackStrategy = strategy;
        }

        // 전략에 따른 공격
        public void PerformAttack()
        {
            attackStrategy.Attack();
        }
    }

    // 예시 사용
    public class Example : MonoBehaviour
    {
        private void Start()
        {
            // 캐릭터 생성
            Character player = new Character();

            // 근접 공격 전략 설정
            player.SetAttackStrategy(new MeleeAttack());
            // 근접 공격 수행
            player.PerformAttack();

            // 원거리 공격 전략 설정
            player.SetAttackStrategy(new RangedAttack());
            // 원거리 공격 수행
            player.PerformAttack();
        }
    }
}

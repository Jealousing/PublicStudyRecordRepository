using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StrategyPattern
{
    // ���� �������̽�
    public interface IAttackStrategy
    {
        void Attack();
    }

    // ��ü���� ���� Ŭ������
    public class MeleeAttack : IAttackStrategy
    {
        public void Attack()
        {
            Debug.Log("���� ����!");
        }
    }

    public class RangedAttack : IAttackStrategy
    {
        public void Attack()
        {
            Debug.Log("���Ÿ� ����!");
        }
    }

    // ĳ���� Ŭ����
    public class Character : MonoBehaviour
    {
        private IAttackStrategy attackStrategy;

        // ���� ����
        public void SetAttackStrategy(IAttackStrategy strategy)
        {
            attackStrategy = strategy;
        }

        // ������ ���� ����
        public void PerformAttack()
        {
            attackStrategy.Attack();
        }
    }

    // ���� ���
    public class Example : MonoBehaviour
    {
        private void Start()
        {
            // ĳ���� ����
            Character player = new Character();

            // ���� ���� ���� ����
            player.SetAttackStrategy(new MeleeAttack());
            // ���� ���� ����
            player.PerformAttack();

            // ���Ÿ� ���� ���� ����
            player.SetAttackStrategy(new RangedAttack());
            // ���Ÿ� ���� ����
            player.PerformAttack();
        }
    }
}

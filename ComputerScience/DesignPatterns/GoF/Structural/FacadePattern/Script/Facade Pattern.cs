using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FacadePattern
{
    // ����ý����� ��Ÿ���� Ŭ������
    class PlayerActions
    {
        public void Attack()
        {
            Debug.Log("�÷��̾ �����մϴ�!");
        }
        public void Defend()
        {
            Debug.Log("�÷��̾ ����մϴ�!");
        }
    }

    class EnemyBehaviors
    {
        public void Attack()
        {
            Debug.Log("���� �����մϴ�!");
        }
        public void SpecialAbility()
        {
            Debug.Log("���� Ư�� �ɷ��� ����մϴ�!");
        }
    }

    // ���� �ý����� �ܼ�ȭ�� �������̽��� �����ϴ� ���� Ŭ����
    class BattleManager
    {
        private PlayerActions playerActions;
        private EnemyBehaviors enemyBehaviors;

        public BattleManager()
        {
            playerActions = new PlayerActions();
            enemyBehaviors = new EnemyBehaviors();
        }

        // ������ �����ϴ� �޼���
        public void StartBattle()
        {
            Debug.Log("���� ����!");

            // �÷��̾�� ���� ���� �ùķ��̼�
            playerActions.Attack();
            playerActions.Defend();
            enemyBehaviors.Attack();
            enemyBehaviors.SpecialAbility();
        }
    }

    public class GameManager : MonoBehaviour
    {
        void Start()
        {
            BattleManager battleManager = new BattleManager();
            battleManager.StartBattle();
        }
    }
}
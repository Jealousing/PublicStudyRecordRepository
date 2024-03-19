using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FacadePattern
{
    // 서브시스템을 나타내는 클래스들
    class PlayerActions
    {
        public void Attack()
        {
            Debug.Log("플레이어가 공격합니다!");
        }
        public void Defend()
        {
            Debug.Log("플레이어가 방어합니다!");
        }
    }

    class EnemyBehaviors
    {
        public void Attack()
        {
            Debug.Log("적이 공격합니다!");
        }
        public void SpecialAbility()
        {
            Debug.Log("적이 특수 능력을 사용합니다!");
        }
    }

    // 전투 시스템을 단순화된 인터페이스로 제공하는 페사드 클래스
    class BattleManager
    {
        private PlayerActions playerActions;
        private EnemyBehaviors enemyBehaviors;

        public BattleManager()
        {
            playerActions = new PlayerActions();
            enemyBehaviors = new EnemyBehaviors();
        }

        // 전투를 시작하는 메서드
        public void StartBattle()
        {
            Debug.Log("전투 시작!");

            // 플레이어와 적의 동작 시뮬레이션
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
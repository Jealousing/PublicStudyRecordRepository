using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TemplateMethodPattern
{
    // AbstractClass (추상 클래스)
    public abstract class Character : MonoBehaviour
    {
        // 템플릿 메서드
        public void Move()
        {
            // 전처리 단계
            PrepareForMove();

            // 이동 메서드 호출
            PerformMove();

            // 후처리 단계
            AfterMove();
        }

        // 서브 클래스에서 구현할 추상 메서드
        protected abstract void PrepareForMove();
        protected abstract void PerformMove();
        protected abstract void AfterMove();
    }

    // ConcreteClass (구체 클래스) - 플레이어 캐릭터
    public class PlayerCharacter : Character
    {
        protected override void PrepareForMove()
        {
            Debug.Log("플레이어 캐릭터 이동을 위한 준비를 합니다.");
        }

        protected override void PerformMove()
        {
            Debug.Log("플레이어 캐릭터가 이동합니다.");
        }

        protected override void AfterMove()
        {
            Debug.Log("플레이어 캐릭터 이동 후 처리를 합니다.");
        }
    }

    // ConcreteClass (구체 클래스) - 적 캐릭터
    public class EnemyCharacter : Character
    {
        protected override void PrepareForMove()
        {
            Debug.Log("적 캐릭터 이동을 위한 준비를 합니다.");
        }

        protected override void PerformMove()
        {
            Debug.Log("적 캐릭터가 이동합니다.");
        }

        protected override void AfterMove()
        {
            Debug.Log("적 캐릭터 이동 후 처리를 합니다.");
        }
    }

    // 게임 매니저
    public class GameManager : MonoBehaviour
    {
        void Start()
        {
            // 게임 예제
            Character player = new PlayerCharacter();
            Character enemy = new EnemyCharacter();

            player.Move();
            enemy.Move();
        }
    }
}
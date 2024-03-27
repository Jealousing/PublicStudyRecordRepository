using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StatePattern
{
    // 상태 인터페이스
    public interface IState
    {
        void Enter();
        void Update();
        void Exit();
    }

    // 구체적인 상태 클래스들
    public class IdleState : IState
    {
        private readonly GameObject character;

        public IdleState(GameObject character)
        {
            this.character = character;
        }

        public void Enter()
        {
            Debug.Log("캐릭터가 대기 중입니다.");
        }

        public void Update()
        {
            // 대기 상태에서의 업데이트 로직
        }

        public void Exit()
        {
            // 대기 상태에서의 종료 로직
        }
    }

    public class MoveState : IState
    {
        private readonly GameObject character;

        public MoveState(GameObject character)
        {
            this.character = character;
        }

        public void Enter()
        {
            Debug.Log("캐릭터가 이동 중입니다.");
        }

        public void Update()
        {
            // 이동 상태에서의 업데이트 로직
        }

        public void Exit()
        {
            // 이동 상태에서의 종료 로직
        }
    }

    public class AttackState : IState
    {
        private readonly GameObject character;

        public AttackState(GameObject character)
        {
            this.character = character;
        }

        public void Enter()
        {
            Debug.Log("캐릭터가 공격 중입니다.");
        }

        public void Update()
        {
            // 공격 상태에서의 업데이트 로직
        }

        public void Exit()
        {
            // 공격 상태에서의 종료 로직
        }
    }

    // 상태 머신 클래스
    public class StateMachine
    {
        private IState currentState;

        public void ChangeState(IState newState)
        {
            if (currentState != null)
            {
                currentState.Exit();
            }

            currentState = newState;
            currentState.Enter();
        }

        public void Update()
        {
            if (currentState != null)
            {
                currentState.Update();
            }
        }
    }

    // 캐릭터 클래스
    public class Character : MonoBehaviour
    {
        private StateMachine stateMachine;

        private void Start()
        {
            stateMachine = new StateMachine();
            stateMachine.ChangeState(new IdleState(gameObject));
        }

        private void Update()
        {
            stateMachine.Update();
        }

        // 사용자 입력 등에 따라 상태 변경
        public void SetStateToMove()
        {
            stateMachine.ChangeState(new MoveState(gameObject));
        }

        public void SetStateToAttack()
        {
            stateMachine.ChangeState(new AttackState(gameObject));
        }
    }
}

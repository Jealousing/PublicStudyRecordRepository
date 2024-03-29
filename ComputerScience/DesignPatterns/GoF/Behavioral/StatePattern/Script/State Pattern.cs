using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StatePattern
{
    // ���� �������̽�
    public interface IState
    {
        void Enter();
        void Update();
        void Exit();
    }

    // ��ü���� ���� Ŭ������
    public class IdleState : IState
    {
        private readonly GameObject character;

        public IdleState(GameObject character)
        {
            this.character = character;
        }

        public void Enter()
        {
            Debug.Log("ĳ���Ͱ� ��� ���Դϴ�.");
        }

        public void Update()
        {
            // ��� ���¿����� ������Ʈ ����
        }

        public void Exit()
        {
            // ��� ���¿����� ���� ����
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
            Debug.Log("ĳ���Ͱ� �̵� ���Դϴ�.");
        }

        public void Update()
        {
            // �̵� ���¿����� ������Ʈ ����
        }

        public void Exit()
        {
            // �̵� ���¿����� ���� ����
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
            Debug.Log("ĳ���Ͱ� ���� ���Դϴ�.");
        }

        public void Update()
        {
            // ���� ���¿����� ������Ʈ ����
        }

        public void Exit()
        {
            // ���� ���¿����� ���� ����
        }
    }

    // ���� �ӽ� Ŭ����
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

    // ĳ���� Ŭ����
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

        // ����� �Է� � ���� ���� ����
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TemplateMethodPattern
{
    // AbstractClass (�߻� Ŭ����)
    public abstract class Character : MonoBehaviour
    {
        // ���ø� �޼���
        public void Move()
        {
            // ��ó�� �ܰ�
            PrepareForMove();

            // �̵� �޼��� ȣ��
            PerformMove();

            // ��ó�� �ܰ�
            AfterMove();
        }

        // ���� Ŭ�������� ������ �߻� �޼���
        protected abstract void PrepareForMove();
        protected abstract void PerformMove();
        protected abstract void AfterMove();
    }

    // ConcreteClass (��ü Ŭ����) - �÷��̾� ĳ����
    public class PlayerCharacter : Character
    {
        protected override void PrepareForMove()
        {
            Debug.Log("�÷��̾� ĳ���� �̵��� ���� �غ� �մϴ�.");
        }

        protected override void PerformMove()
        {
            Debug.Log("�÷��̾� ĳ���Ͱ� �̵��մϴ�.");
        }

        protected override void AfterMove()
        {
            Debug.Log("�÷��̾� ĳ���� �̵� �� ó���� �մϴ�.");
        }
    }

    // ConcreteClass (��ü Ŭ����) - �� ĳ����
    public class EnemyCharacter : Character
    {
        protected override void PrepareForMove()
        {
            Debug.Log("�� ĳ���� �̵��� ���� �غ� �մϴ�.");
        }

        protected override void PerformMove()
        {
            Debug.Log("�� ĳ���Ͱ� �̵��մϴ�.");
        }

        protected override void AfterMove()
        {
            Debug.Log("�� ĳ���� �̵� �� ó���� �մϴ�.");
        }
    }

    // ���� �Ŵ���
    public class GameManager : MonoBehaviour
    {
        void Start()
        {
            // ���� ����
            Character player = new PlayerCharacter();
            Character enemy = new EnemyCharacter();

            player.Move();
            enemy.Move();
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VisitorPattern
{
    // �湮�� �������̽�
    public interface IVisitor
    {
        void Visit(Character character);
        void Visit(Monster monster);
    }

    // ��ü���� �湮��
    public class DebugVisitor : IVisitor
    {
        public void Visit(Character character)
        {
            Debug.Log("ĳ���� �湮: " + character.name);
        }

        public void Visit(Monster monster)
        {
            Debug.Log("���� �湮: " + monster.name);
        }
    }

    // ��� �������̽�
    public interface IElement
    {
        // �湮�ڸ� �����մϴ�.
        void Accept(IVisitor visitor);
    }

    // ��ü���� ���
    public class Character : MonoBehaviour, IElement
    {
        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        // ĳ���Ϳ� ���� Ư�� ����
        public void Move()
        {
            Debug.Log("ĳ���� �̵�");
        }
    }

    public class Monster : MonoBehaviour, IElement
    {
        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        // ���Ϳ� ���� Ư�� ����
        public void Attack()
        {
            Debug.Log("���� ����");
        }
    }

    // ���� �Ŵ���
    public class GameManager : MonoBehaviour
    {
        private List<IElement> gameObjects = new List<IElement>();

        // ���� ��ü�� �߰��մϴ�.
        public void AddGameObject(IElement gameObject)
        {
            gameObjects.Add(gameObject);
        }

        // ��� ���� ��ü�� �湮�ڸ� �����մϴ�.
        public void AcceptVisitor(IVisitor visitor)
        {
            foreach (var gameObject in gameObjects)
            {
                gameObject.Accept(visitor);
            }
        }

        void Start()
        {
            // ���� ����
            Character player = new Character();
            Monster enemy = new Monster();

            AddGameObject(player);
            AddGameObject(enemy);

            DebugVisitor debugVisitor = new DebugVisitor();
            AcceptVisitor(debugVisitor);
        }
    }
}

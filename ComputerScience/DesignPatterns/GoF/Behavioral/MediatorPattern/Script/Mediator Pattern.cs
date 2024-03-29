using System;   
using System.Collections.Generic;
using UnityEngine;

namespace MediatorPattern
{
    // ������ �������̽�
    public interface IMediator
    {
        // �ٸ� ��ü���� �˸��� �޼���
        void Notify(GameObject sender, string eventInfo);

        // ���� ������Ʈ ��� �޼���
        void Register(string name, GameObject gameObject);
    }

    // ��ü���� ������
    public class GameManager : MonoBehaviour, IMediator
    {
        // ���� ������Ʈ �̸��� �ش� ������Ʈ�� �����ϴ� ��ųʸ�
        private Dictionary<string, GameObject> gameObjects = new Dictionary<string, GameObject>();

        // ���� ������Ʈ ��� �޼���
        public void Register(string name, GameObject gameObject)
        {
            gameObjects.Add(name, gameObject);
        }

        // �ٸ� ��ü���� �˸�
        public void Notify(GameObject sender, string eventInfo)
        {
            // ���� ��ü�� �����ϰ� ��� ��ü���� �̺�Ʈ ����
            foreach (var gameObject in gameObjects)
            {
                if (gameObject.Key != sender.name)
                {
                    gameObject.Value.SendMessage("OnNotify", eventInfo);
                }
            }
        }
    }

    // ����
    public class Player : MonoBehaviour
    {
        private IMediator mediator;

        private void Start()
        {
            mediator = FindObjectOfType<GameManager>();
            mediator.Register(gameObject.name, gameObject);
        }

        // �÷��̾� ���� �޼���
        public void Attack()
        {
            Debug.Log("�÷��̾ �����մϴ�!");
            mediator.Notify(gameObject, "�÷��̾ �����߽��ϴ�!");
        }

        // �˸� ���� �޼���
        public void OnNotify(string eventInfo)
        {
            Debug.Log("�÷��̾ ���� ����: " + eventInfo);
        }
    }

    // ����
    public class Enemy : MonoBehaviour
    {
        private IMediator mediator;

        private void Start()
        {
            mediator = FindObjectOfType<GameManager>();
            mediator.Register(gameObject.name, gameObject);
        }

        // �� ���� �޼���
        public void Attack()
        {
            Debug.Log("���� �����մϴ�!");
            mediator.Notify(gameObject, "���� �����߽��ϴ�!");
        }

        // �˸� ���� �޼���
        public void OnNotify(string eventInfo)
        {
            Debug.Log("���� ���� ����: " + eventInfo);
        }
    }

    // Ŭ���̾�Ʈ
    public class Game : MonoBehaviour
    {
        private void Start()
        {
            var player = FindObjectOfType<Player>();
            var enemy = FindObjectOfType<Enemy>();

            // ���� ���� �� �÷��̾�� ���� ���� ������ ����
            player.Attack();
            enemy.Attack();
        }
    }
}

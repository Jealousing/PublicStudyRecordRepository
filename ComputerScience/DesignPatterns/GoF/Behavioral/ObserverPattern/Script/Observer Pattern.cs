using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObserverPattern
{
    // Subject: �÷��̾� Ŭ����
    public class Player
    {
        // ���������� �����ϴ� ����Ʈ
        private List<IPlayerObserver> observers = new List<IPlayerObserver>();

        // �÷��̾��� ü��
        private int health;

        // ü�� ������Ƽ
        public int Health
        {
            get { return health; }
            set
            {
                health = value;
                NotifyObservers(); // �������鿡�� ���� ������ �˸�
            }
        }

        // ������ ��� �޼���
        public void RegisterObserver(IPlayerObserver observer)
        {
            observers.Add(observer);
        }

        // ������ ���� �޼���
        public void RemoveObserver(IPlayerObserver observer)
        {
            observers.Remove(observer);
        }

        // ���� ������ �˸��� �޼���
        private void NotifyObservers()
        {
            foreach (var observer in observers)
            {
                observer.OnHealthChanged(Health);
            }
        }
    }

    // Observer: �÷��̾� ���� ��ȭ�� �����ϴ� �������̽�
    public interface IPlayerObserver
    {
        void OnHealthChanged(int health);
    }

    // Concrete Observer 1: ü���� ǥ���ϴ� UI Ŭ����
    public class HealthUI : MonoBehaviour, IPlayerObserver
    {
        public void OnHealthChanged(int health)
        {
            Debug.Log("ü�� UI ������Ʈ: " + health);
            // ü���� ȭ�鿡 ������Ʈ�ϴ� ������ ������ �� ����
        }
    }

    // Concrete Observer 2: �÷��̾��� ���¸� �α׷� ����ϴ� Ŭ����
    public class PlayerLogger : MonoBehaviour, IPlayerObserver
    {
        public void OnHealthChanged(int health)
        {
            Debug.Log("�÷��̾� ü�� ����: " + health);
            // �÷��̾��� ���¸� �α׿� ����ϴ� ������ ������ �� ����
        }
    }

    // ���� �Ŵ��� Ŭ����
    public class GameManager : MonoBehaviour
    {
        void Start()
        {
            // �÷��̾� ��ü ����
            Player player = new Player();

            // ������ ��ü ����
            HealthUI healthUI = FindObjectOfType<HealthUI>();
            PlayerLogger playerLogger = FindObjectOfType<PlayerLogger>();

            // ������ ���
            player.RegisterObserver(healthUI);
            player.RegisterObserver(playerLogger);

            // �÷��̾� ü�� ���� (�������鿡�� �˸�)
            player.Health = 100;
        }
    }
}

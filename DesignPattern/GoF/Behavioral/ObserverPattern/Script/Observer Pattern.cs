using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObserverPattern
{
    // Subject: 플레이어 클래스
    public class Player
    {
        // 옵저버들을 저장하는 리스트
        private List<IPlayerObserver> observers = new List<IPlayerObserver>();

        // 플레이어의 체력
        private int health;

        // 체력 프로퍼티
        public int Health
        {
            get { return health; }
            set
            {
                health = value;
                NotifyObservers(); // 옵저버들에게 상태 변경을 알림
            }
        }

        // 옵저버 등록 메서드
        public void RegisterObserver(IPlayerObserver observer)
        {
            observers.Add(observer);
        }

        // 옵저버 삭제 메서드
        public void RemoveObserver(IPlayerObserver observer)
        {
            observers.Remove(observer);
        }

        // 상태 변경을 알리는 메서드
        private void NotifyObservers()
        {
            foreach (var observer in observers)
            {
                observer.OnHealthChanged(Health);
            }
        }
    }

    // Observer: 플레이어 상태 변화를 감지하는 인터페이스
    public interface IPlayerObserver
    {
        void OnHealthChanged(int health);
    }

    // Concrete Observer 1: 체력을 표시하는 UI 클래스
    public class HealthUI : MonoBehaviour, IPlayerObserver
    {
        public void OnHealthChanged(int health)
        {
            Debug.Log("체력 UI 업데이트: " + health);
            // 체력을 화면에 업데이트하는 로직을 구현할 수 있음
        }
    }

    // Concrete Observer 2: 플레이어의 상태를 로그로 출력하는 클래스
    public class PlayerLogger : MonoBehaviour, IPlayerObserver
    {
        public void OnHealthChanged(int health)
        {
            Debug.Log("플레이어 체력 변경: " + health);
            // 플레이어의 상태를 로그에 출력하는 로직을 구현할 수 있음
        }
    }

    // 게임 매니저 클래스
    public class GameManager : MonoBehaviour
    {
        void Start()
        {
            // 플레이어 객체 생성
            Player player = new Player();

            // 옵저버 객체 생성
            HealthUI healthUI = FindObjectOfType<HealthUI>();
            PlayerLogger playerLogger = FindObjectOfType<PlayerLogger>();

            // 옵저버 등록
            player.RegisterObserver(healthUI);
            player.RegisterObserver(playerLogger);

            // 플레이어 체력 변경 (옵저버들에게 알림)
            player.Health = 100;
        }
    }
}

using System;   
using System.Collections.Generic;
using UnityEngine;

namespace MediatorPattern
{
    // 중재자 인터페이스
    public interface IMediator
    {
        // 다른 객체에게 알리는 메서드
        void Notify(GameObject sender, string eventInfo);

        // 게임 오브젝트 등록 메서드
        void Register(string name, GameObject gameObject);
    }

    // 구체적인 중재자
    public class GameManager : MonoBehaviour, IMediator
    {
        // 게임 오브젝트 이름과 해당 오브젝트를 저장하는 딕셔너리
        private Dictionary<string, GameObject> gameObjects = new Dictionary<string, GameObject>();

        // 게임 오브젝트 등록 메서드
        public void Register(string name, GameObject gameObject)
        {
            gameObjects.Add(name, gameObject);
        }

        // 다른 객체에게 알림
        public void Notify(GameObject sender, string eventInfo)
        {
            // 보낸 객체를 제외하고 모든 객체에게 이벤트 전달
            foreach (var gameObject in gameObjects)
            {
                if (gameObject.Key != sender.name)
                {
                    gameObject.Value.SendMessage("OnNotify", eventInfo);
                }
            }
        }
    }

    // 동료
    public class Player : MonoBehaviour
    {
        private IMediator mediator;

        private void Start()
        {
            mediator = FindObjectOfType<GameManager>();
            mediator.Register(gameObject.name, gameObject);
        }

        // 플레이어 공격 메서드
        public void Attack()
        {
            Debug.Log("플레이어가 공격합니다!");
            mediator.Notify(gameObject, "플레이어가 공격했습니다!");
        }

        // 알림 수신 메서드
        public void OnNotify(string eventInfo)
        {
            Debug.Log("플레이어가 받은 내용: " + eventInfo);
        }
    }

    // 동료
    public class Enemy : MonoBehaviour
    {
        private IMediator mediator;

        private void Start()
        {
            mediator = FindObjectOfType<GameManager>();
            mediator.Register(gameObject.name, gameObject);
        }

        // 적 공격 메서드
        public void Attack()
        {
            Debug.Log("적이 공격합니다!");
            mediator.Notify(gameObject, "적이 공격했습니다!");
        }

        // 알림 수신 메서드
        public void OnNotify(string eventInfo)
        {
            Debug.Log("적이 받은 내용: " + eventInfo);
        }
    }

    // 클라이언트
    public class Game : MonoBehaviour
    {
        private void Start()
        {
            var player = FindObjectOfType<Player>();
            var enemy = FindObjectOfType<Enemy>();

            // 게임 시작 시 플레이어와 적이 각각 공격을 실행
            player.Attack();
            enemy.Attack();
        }
    }
}

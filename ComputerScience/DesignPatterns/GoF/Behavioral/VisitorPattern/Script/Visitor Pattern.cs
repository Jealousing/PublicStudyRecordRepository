using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VisitorPattern
{
    // 방문자 인터페이스
    public interface IVisitor
    {
        void Visit(Character character);
        void Visit(Monster monster);
    }

    // 구체적인 방문자
    public class DebugVisitor : IVisitor
    {
        public void Visit(Character character)
        {
            Debug.Log("캐릭터 방문: " + character.name);
        }

        public void Visit(Monster monster)
        {
            Debug.Log("몬스터 방문: " + monster.name);
        }
    }

    // 요소 인터페이스
    public interface IElement
    {
        // 방문자를 수락합니다.
        void Accept(IVisitor visitor);
    }

    // 구체적인 요소
    public class Character : MonoBehaviour, IElement
    {
        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        // 캐릭터에 대한 특정 동작
        public void Move()
        {
            Debug.Log("캐릭터 이동");
        }
    }

    public class Monster : MonoBehaviour, IElement
    {
        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        // 몬스터에 대한 특정 동작
        public void Attack()
        {
            Debug.Log("몬스터 공격");
        }
    }

    // 게임 매니저
    public class GameManager : MonoBehaviour
    {
        private List<IElement> gameObjects = new List<IElement>();

        // 게임 객체를 추가합니다.
        public void AddGameObject(IElement gameObject)
        {
            gameObjects.Add(gameObject);
        }

        // 모든 게임 객체에 방문자를 수락합니다.
        public void AcceptVisitor(IVisitor visitor)
        {
            foreach (var gameObject in gameObjects)
            {
                gameObject.Accept(visitor);
            }
        }

        void Start()
        {
            // 게임 예제
            Character player = new Character();
            Monster enemy = new Monster();

            AddGameObject(player);
            AddGameObject(enemy);

            DebugVisitor debugVisitor = new DebugVisitor();
            AcceptVisitor(debugVisitor);
        }
    }
}

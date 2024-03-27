using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IteratorPattern
{
    // 요소(Element) 인터페이스
    public interface IGameElement
    {
        void Display();
    }

    // 게임 요소(Element) 클래스
    public class GameObject : IGameElement
    {
        private string name;

        public GameObject(string name)
        {
            this.name = name;
        }

        public void Display()
        {
            Debug.Log("게임 오브젝트: " + name);
        }
    }

    // 집합체(Iterable) 인터페이스
    public interface IGameCollection
    {
        IIterator CreateIterator();
    }

    // 게임 요소 집합체(Iterable) 클래스
    public class GameObjectCollection : IGameCollection
    {
        private ArrayList gameObjects = new ArrayList();

        public IIterator CreateIterator()
        {
            return new GameObjectIterator(this);
        }

        public void AddGameObject(IGameElement gameObject)
        {
            gameObjects.Add(gameObject);
        }

        public IGameElement GetGameObject(int index)
        {
            return (IGameElement)gameObjects[index];
        }

        public int Count()
        {
            return gameObjects.Count;
        }
    }

    // 반복자(Iterator) 인터페이스
    public interface IIterator
    {
        IGameElement First();
        IGameElement Next();
        bool IsDone();
        IGameElement CurrentItem();
    }

    // 게임 오브젝트 반복자(Iterator) 클래스
    public class GameObjectIterator : IIterator
    {
        private GameObjectCollection collection;
        private int current = 0;

        public GameObjectIterator(GameObjectCollection collection)
        {
            this.collection = collection;
        }

        public IGameElement First()
        {
            current = 0;
            return collection.GetGameObject(current);
        }

        public IGameElement Next()
        {
            current++;
            if (!IsDone())
            {
                return collection.GetGameObject(current);
            }
            else
            {
                return null;
            }
        }

        public bool IsDone()
        {
            return current >= collection.Count();
        }

        public IGameElement CurrentItem()
        {
            return collection.GetGameObject(current);
        }
    }

    // 클라이언트 코드
    public class GameManager : MonoBehaviour
    {
        void Start()
        {
            GameObjectCollection collection = new GameObjectCollection();
            collection.AddGameObject(new GameObject("Player"));
            collection.AddGameObject(new GameObject("Enemy1"));
            collection.AddGameObject(new GameObject("Enemy2"));
            collection.AddGameObject(new GameObject("Obstacle"));

            IIterator iterator = collection.CreateIterator();

            Debug.Log("게임 요소 순회 시작:");
            for (IGameElement element = iterator.First(); !iterator.IsDone(); element = iterator.Next())
            {
                element.Display();
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IteratorPattern
{
    // ���(Element) �������̽�
    public interface IGameElement
    {
        void Display();
    }

    // ���� ���(Element) Ŭ����
    public class GameObject : IGameElement
    {
        private string name;

        public GameObject(string name)
        {
            this.name = name;
        }

        public void Display()
        {
            Debug.Log("���� ������Ʈ: " + name);
        }
    }

    // ����ü(Iterable) �������̽�
    public interface IGameCollection
    {
        IIterator CreateIterator();
    }

    // ���� ��� ����ü(Iterable) Ŭ����
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

    // �ݺ���(Iterator) �������̽�
    public interface IIterator
    {
        IGameElement First();
        IGameElement Next();
        bool IsDone();
        IGameElement CurrentItem();
    }

    // ���� ������Ʈ �ݺ���(Iterator) Ŭ����
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

    // Ŭ���̾�Ʈ �ڵ�
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

            Debug.Log("���� ��� ��ȸ ����:");
            for (IGameElement element = iterator.First(); !iterator.IsDone(); element = iterator.Next())
            {
                element.Display();
            }
        }
    }
}

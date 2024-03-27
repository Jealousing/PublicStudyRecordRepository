using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace MementoPattern
{
    // Originator: ���� ���¸� ��Ÿ���� ��ü
    public class GameStatus
    {
        public int score { get; set; }
        public int level { get; set; }
        public string currentScene { get; set; }

        public GameStatus(int score, int level, string currentScene)
        {
            this.score = score;
            this.level = level;
            this.currentScene = currentScene;
        }

        // �޸��� ����
        public GameMemento CreateMemento()
        {
            return new GameMemento(score, level, currentScene);
        }

        // �޸��並 ����Ͽ� ���� ����
        public void RestoreFromMemento(GameMemento memento)
        {
            score = memento.score;
            level = memento.level;
            currentScene = memento.CurrentScene;
        }

        // ���� ���� ���
        public void PrintStatus()
        {
            Debug.Log($"Score: {score}, Level: {level}, Scene: {currentScene}");
        }
    }

    // Memento: ���� ������ �������� �����ϴ� ��ü
    public class GameMemento
    {
        public int score { get; }
        public int level { get; }
        public string CurrentScene { get; }

        public GameMemento(int score, int level, string currentScene)
        {
            this.score = score;
            this.level = level;
            CurrentScene = currentScene;
        }
    }

    // Caretaker: Memento ��ü�� �����ϴ� ��ü
    public class GameManager : MonoBehaviour
    {
        private GameStatus gameStatus;

        private void Start()
        {
            // ���� ���� �ʱ�ȭ
            gameStatus = new GameStatus(0, 1, "Level1");

            // ���� ����
            gameStatus.score += 100;
            gameStatus.level++;

            // ���� ���� ���
            Debug.Log("Current Status:");
            gameStatus.PrintStatus();

            // ���� ���� ����
            GameMemento memento = gameStatus.CreateMemento();

            // ���� ����
            gameStatus.score += 200;
            gameStatus.level++;
            gameStatus.currentScene = "Level2";

            // ���� ���� ���
            Debug.Log("Changed Status:");
            gameStatus.PrintStatus();

            // ����� �޸��並 ����Ͽ� ���� ���·� ����
            gameStatus.RestoreFromMemento(memento);

            // ������ ���� ���
            Debug.Log("Restored Status:");
            gameStatus.PrintStatus();
        }
    }
}

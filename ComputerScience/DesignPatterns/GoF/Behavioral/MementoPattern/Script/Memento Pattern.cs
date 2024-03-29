using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace MementoPattern
{
    // Originator: 게임 상태를 나타내는 객체
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

        // 메멘토 생성
        public GameMemento CreateMemento()
        {
            return new GameMemento(score, level, currentScene);
        }

        // 메멘토를 사용하여 상태 복원
        public void RestoreFromMemento(GameMemento memento)
        {
            score = memento.score;
            level = memento.level;
            currentScene = memento.CurrentScene;
        }

        // 현재 상태 출력
        public void PrintStatus()
        {
            Debug.Log($"Score: {score}, Level: {level}, Scene: {currentScene}");
        }
    }

    // Memento: 게임 상태의 스냅샷을 저장하는 객체
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

    // Caretaker: Memento 객체를 관리하는 객체
    public class GameManager : MonoBehaviour
    {
        private GameStatus gameStatus;

        private void Start()
        {
            // 게임 상태 초기화
            gameStatus = new GameStatus(0, 1, "Level1");

            // 상태 변경
            gameStatus.score += 100;
            gameStatus.level++;

            // 현재 상태 출력
            Debug.Log("Current Status:");
            gameStatus.PrintStatus();

            // 현재 상태 저장
            GameMemento memento = gameStatus.CreateMemento();

            // 상태 변경
            gameStatus.score += 200;
            gameStatus.level++;
            gameStatus.currentScene = "Level2";

            // 현재 상태 출력
            Debug.Log("Changed Status:");
            gameStatus.PrintStatus();

            // 저장된 메멘토를 사용하여 이전 상태로 복원
            gameStatus.RestoreFromMemento(memento);

            // 복원된 상태 출력
            Debug.Log("Restored Status:");
            gameStatus.PrintStatus();
        }
    }
}

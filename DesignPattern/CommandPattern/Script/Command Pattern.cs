using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CommandPattern 
{
    // Command 인터페이스
    public interface ICommand
    {
        void Execute();
    }

    // 캐릭터 이동을 위한 커맨드
    public class MoveCommand : ICommand
    {
        private Transform character;
        private Vector3 destination;

        public MoveCommand(Transform character, Vector3 destination)
        {
            this.character = character;
            this.destination = destination;
        }

        public void Execute()
        {
            // 캐릭터를 목적지로 이동시킵니다.
            character.position = destination;
        }
    }

    // 캐릭터 공격을 위한 커맨드
    public class AttackCommand : ICommand
    {
        private GameObject attacker;
        private GameObject target;

        public AttackCommand(GameObject attacker, GameObject target)
        {
            this.attacker = attacker;
            this.target = target;
        }

        public void Execute()
        {
            // 공격 로직을 구현합니다.
            Debug.Log(attacker.name + "이(가) " + target.name + "을(를) 공격합니다!");
        }
    }

    // 아이템 사용을 위한 커맨드
    public class UseItemCommand : ICommand
    {
        private GameObject player;
        private GameObject item;

        public UseItemCommand(GameObject player, GameObject item)
        {
            this.player = player;
            this.item = item;
        }

        public void Execute()
        {
            // 아이템 사용 로직을 구현합니다.
            Debug.Log(player.name + "이(가) " + item.name + "을(를) 사용합니다!");
        }
    }

    // 커맨드를 실행하는 Invoker 클래스
    public class CommandInvoker : MonoBehaviour
    {
        // 커맨드 실행 메서드
        public void ExecuteCommand(ICommand command)
        {
            command.Execute();
        }
    }

    // 게임 관리자 클래스
    public class RPGGameManager : MonoBehaviour
    {
        public Transform playerCharacter;
        public GameObject enemy;
        public GameObject healthPotion;

        private CommandInvoker commandInvoker;

        private void Start()
        {
            commandInvoker = GetComponent<CommandInvoker>();

            // 예시: 캐릭터를 목적지로 이동시킵니다.
            ICommand moveCommand = new MoveCommand(playerCharacter, new Vector3(10f, 0f, 10f));
            commandInvoker.ExecuteCommand(moveCommand);

            // 예시: 적을 공격합니다.
            ICommand attackCommand = new AttackCommand(playerCharacter.gameObject, enemy);
            commandInvoker.ExecuteCommand(attackCommand);

            // 예시: 체력 물약을 사용합니다.
            ICommand useItemCommand = new UseItemCommand(playerCharacter.gameObject, healthPotion);
            commandInvoker.ExecuteCommand(useItemCommand);
        }
    }
}

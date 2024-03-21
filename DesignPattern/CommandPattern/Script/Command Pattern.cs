using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CommandPattern 
{
    // Command �������̽�
    public interface ICommand
    {
        void Execute();
    }

    // ĳ���� �̵��� ���� Ŀ�ǵ�
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
            // ĳ���͸� �������� �̵���ŵ�ϴ�.
            character.position = destination;
        }
    }

    // ĳ���� ������ ���� Ŀ�ǵ�
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
            // ���� ������ �����մϴ�.
            Debug.Log(attacker.name + "��(��) " + target.name + "��(��) �����մϴ�!");
        }
    }

    // ������ ����� ���� Ŀ�ǵ�
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
            // ������ ��� ������ �����մϴ�.
            Debug.Log(player.name + "��(��) " + item.name + "��(��) ����մϴ�!");
        }
    }

    // Ŀ�ǵ带 �����ϴ� Invoker Ŭ����
    public class CommandInvoker : MonoBehaviour
    {
        // Ŀ�ǵ� ���� �޼���
        public void ExecuteCommand(ICommand command)
        {
            command.Execute();
        }
    }

    // ���� ������ Ŭ����
    public class RPGGameManager : MonoBehaviour
    {
        public Transform playerCharacter;
        public GameObject enemy;
        public GameObject healthPotion;

        private CommandInvoker commandInvoker;

        private void Start()
        {
            commandInvoker = GetComponent<CommandInvoker>();

            // ����: ĳ���͸� �������� �̵���ŵ�ϴ�.
            ICommand moveCommand = new MoveCommand(playerCharacter, new Vector3(10f, 0f, 10f));
            commandInvoker.ExecuteCommand(moveCommand);

            // ����: ���� �����մϴ�.
            ICommand attackCommand = new AttackCommand(playerCharacter.gameObject, enemy);
            commandInvoker.ExecuteCommand(attackCommand);

            // ����: ü�� ������ ����մϴ�.
            ICommand useItemCommand = new UseItemCommand(playerCharacter.gameObject, healthPotion);
            commandInvoker.ExecuteCommand(useItemCommand);
        }
    }
}

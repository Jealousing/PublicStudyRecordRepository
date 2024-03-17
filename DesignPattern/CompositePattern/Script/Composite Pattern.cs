using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CompositePattern
{
    // Component (�⺻ ���� ���)
    public abstract class Component
    {
        protected string name;

        public Component(string name)
        {
            this.name = name;
        }

        //  ���� Ŭ������ ���� ������ Display �޼���
        public abstract void Display(int depth);
    }

    // Leaf (���� ��ü)
    public class Character : Component
    {
        public Character(string name) : base(name) { }

        // ĳ���͸� ǥ���ϴ� Display �޼��� ����
        public override void Display(int depth)
        {
            Debug.Log(new string('-', depth) + " " + name);
        }
    }

    // Composite (���� ��ü)
    public class Party : Component
    {
        private List<Component> members = new List<Component>();

        public Party(string name) : base(name) { }

        // ��Ƽ�� ����� �߰��մϴ� (�ִ� 5�����)
        public void Add(Component member)
        {
            if (members.Count < 5)
            {
                members.Add(member);
            }
            else
            {
                Debug.Log("��Ƽ�� ���� á���ϴ�. �� �̻� ����� �߰��� �� �����ϴ�.");
            }
        }

        // ��Ƽ�� ǥ���ϴ� Display �޼��� ����
        public override void Display(int depth)
        {
            // ���̿� ���� �鿩���⸦ �߰��Ͽ� ��Ƽ�� ǥ���մϴ�.
            Debug.Log(new string('-', depth) + " " + name + " (��Ƽ)");
            foreach (var member in members)
            {
                member.Display(depth + 1); // ��Ƽ ����� ���̸� �� �ܰ� �� ��� ǥ���մϴ�.
            }
        }
    }

    // Composite (���� ��ü)
    public class RaidTeam : Component
    {
        private List<Component> parties = new List<Component>();

        public RaidTeam(string name) : base(name) { }

        // ���̵忡 ��Ƽ�� �߰��մϴ� (�ִ� 8������)
        public void Add(Component party)
        {
            if (parties.Count < 8)
            {
                parties.Add(party);
            }
            else
            {
                Debug.Log("���̵尡 ���� á���ϴ�. �� �̻� ��Ƽ�� �߰��� �� �����ϴ�.");
            }
        }

        // ���̵带 ǥ���ϴ� Display �޼��� ����
        public override void Display(int depth)
        {
            // ���̿� ���� �鿩���⸦ �߰��Ͽ� ���̵带 ǥ���մϴ�.
            Debug.Log(new string('-', depth) + " " + name + " (���̵�)");
            foreach (var party in parties)
            {
                party.Display(depth + 1); // ���̵� ��Ƽ�� ���̸� �� �ܰ� �� ��� ǥ���մϴ�.
            }
        }
    }

    // Client (Ŭ���̾�Ʈ)
    public class RaidGameManager : MonoBehaviour
    {
        void Start()
        {
            // ���� ��ü ����
            RaidTeam raid = new RaidTeam("���̵�");

            // ��Ƽ �� �÷��̾� ����
            for (int i = 1; i <= 8; i++)
            {
                Party party = new Party("��Ƽ " + i);
                for (int j = 1; j <= 5; j++)
                {
                    Character player = new Character("��Ƽ " + i + "�� �÷��̾� " + j);
                    party.Add(player);
                }
                raid.Add(party);
            }

            // ���� ��ü ǥ��
            raid.Display(0); // ���̵� ������ ǥ���մϴ�.
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CompositePattern
{
    // Component (기본 구성 요소)
    public abstract class Component
    {
        protected string name;

        public Component(string name)
        {
            this.name = name;
        }

        //  하위 클래스에 의해 구현될 Display 메서드
        public abstract void Display(int depth);
    }

    // Leaf (단일 객체)
    public class Character : Component
    {
        public Character(string name) : base(name) { }

        // 캐릭터를 표시하는 Display 메서드 구현
        public override void Display(int depth)
        {
            Debug.Log(new string('-', depth) + " " + name);
        }
    }

    // Composite (복합 객체)
    public class Party : Component
    {
        private List<Component> members = new List<Component>();

        public Party(string name) : base(name) { }

        // 파티에 멤버를 추가합니다 (최대 5명까지)
        public void Add(Component member)
        {
            if (members.Count < 5)
            {
                members.Add(member);
            }
            else
            {
                Debug.Log("파티가 가득 찼습니다. 더 이상 멤버를 추가할 수 없습니다.");
            }
        }

        // 파티를 표시하는 Display 메서드 구현
        public override void Display(int depth)
        {
            // 깊이에 따라 들여쓰기를 추가하여 파티를 표시합니다.
            Debug.Log(new string('-', depth) + " " + name + " (파티)");
            foreach (var member in members)
            {
                member.Display(depth + 1); // 파티 멤버의 깊이를 한 단계 더 깊게 표시합니다.
            }
        }
    }

    // Composite (복합 객체)
    public class RaidTeam : Component
    {
        private List<Component> parties = new List<Component>();

        public RaidTeam(string name) : base(name) { }

        // 레이드에 파티를 추가합니다 (최대 8개까지)
        public void Add(Component party)
        {
            if (parties.Count < 8)
            {
                parties.Add(party);
            }
            else
            {
                Debug.Log("레이드가 가득 찼습니다. 더 이상 파티를 추가할 수 없습니다.");
            }
        }

        // 레이드를 표시하는 Display 메서드 구현
        public override void Display(int depth)
        {
            // 깊이에 따라 들여쓰기를 추가하여 레이드를 표시합니다.
            Debug.Log(new string('-', depth) + " " + name + " (레이드)");
            foreach (var party in parties)
            {
                party.Display(depth + 1); // 레이드 파티의 깊이를 한 단계 더 깊게 표시합니다.
            }
        }
    }

    // Client (클라이언트)
    public class RaidGameManager : MonoBehaviour
    {
        void Start()
        {
            // 복합 객체 생성
            RaidTeam raid = new RaidTeam("레이드");

            // 파티 및 플레이어 생성
            for (int i = 1; i <= 8; i++)
            {
                Party party = new Party("파티 " + i);
                for (int j = 1; j <= 5; j++)
                {
                    Character player = new Character("파티 " + i + "의 플레이어 " + j);
                    party.Add(player);
                }
                raid.Add(party);
            }

            // 복합 객체 표시
            raid.Display(0); // 레이드 구조를 표시합니다.
        }
    }
}
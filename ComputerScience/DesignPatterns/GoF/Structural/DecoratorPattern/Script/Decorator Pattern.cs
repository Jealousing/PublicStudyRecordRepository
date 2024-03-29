using UnityEngine;

namespace DecoratorPattern
{
    // Component Interface
    public interface ICharacter
    {
        void DisplayStats(); // ĳ������ ���ݸ� ǥ���ϴ� �޼���
    }

    // ������ �� ĳ��������
    public class BasicCharacter : ICharacter
    {
        public void DisplayStats()
        {
            Debug.Log("�⺻ ĳ����: ü�� 100, ���ݷ� 10, ���� 5");
        }
    }

    // Decorator Base
    public abstract class CharacterDecorator : ICharacter
    {
        protected ICharacter character;

        public CharacterDecorator(ICharacter character)
        {
            this.character = character;
        }

        public virtual void DisplayStats()
        {
            character.DisplayStats();
        }
    }

    // ü���߰� ����
    public class HealthBoostDecorator : CharacterDecorator
    {
        public HealthBoostDecorator(ICharacter character) : base(character) { }

        public override void DisplayStats()
        {
            base.DisplayStats();
            Debug.Log("ü�� ���: +50");
        }
    }

    // ���ݷ� �߰� ����
    public class AttackBoostDecorator : CharacterDecorator
    {
        public AttackBoostDecorator(ICharacter character) : base(character) { }

        public override void DisplayStats()
        {
            base.DisplayStats();
            Debug.Log("���ݷ� ���: +20");
        }
    }

    public class GameManager : MonoBehaviour
    {
        void Start()
        {
            // �⺻ĳ���� ���� �� ���� ���
            ICharacter basicCharacter = new BasicCharacter();
            basicCharacter.DisplayStats();

            Debug.Log("---");

            // ü���� �߰��� ���� ���
            ICharacter characterWithHealthBoost = new HealthBoostDecorator(basicCharacter);
            characterWithHealthBoost.DisplayStats();

            Debug.Log("---");

            //���ݷ��� �߰��� ���� ���
            ICharacter characterWithAttackBoost = new AttackBoostDecorator(basicCharacter);
            characterWithAttackBoost.DisplayStats();

            Debug.Log("---");

            // ü�°� ���ݷ��� �߰��� ���� ���
            ICharacter characterWithBothBoosts = new AttackBoostDecorator(new HealthBoostDecorator(basicCharacter));
            characterWithBothBoosts.DisplayStats();
        }
    }
}
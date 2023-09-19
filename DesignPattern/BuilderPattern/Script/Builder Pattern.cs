using UnityEngine;

namespace BuilderPattern
{
    // ������ ���
    public enum Rarity
    {
        Common,
        Uncommon,
        Rare,
        Unique,
        Legendary,
        Mythic,
    }

    // ������ ����
    public class Item
    {
        public string name { get; set; }
        public Rarity rarity { get; set; }
        public string description { get; set; }

        public override string ToString()
        {
            return $"Name: {name}, Rarity: {rarity}, Description: {description}";
        }
    }

    // ������ ������ ���� ���� �������̽�
    public interface IItemBuilder
    {
        void SetName(string name);
        void SetRarity(Rarity rarity);
        void SetDescription(string description);
        Item Build();
    }

    // ������ ���� ���� Ŭ����
    public class ItemBuilder : IItemBuilder
    {
        private Item item = new Item();
        public void SetName(string name) => item.name = name;
        public void SetRarity(Rarity rarity) => item.rarity = rarity;
        public void SetDescription(string description) => item.description = description;
        public Item Build() => item;
    }

    // �׽�Ʈ��
    public class BuilderPatternExample : MonoBehaviour
    {
        void Start()
        {
            // ������ ����Ͽ� ������ ���� �� ����
            IItemBuilder itemBuilder = new ItemBuilder();
            itemBuilder.SetName("Sword");
            itemBuilder.SetRarity(Rarity.Rare);
            itemBuilder.SetDescription("����� Į.");

            Item sword = itemBuilder.Build();

            // ������ ������ ���� ���
            Debug.Log(sword);
        }
    }


}
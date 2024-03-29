using UnityEngine;

namespace BuilderPattern
{
    // 아이템 등급
    public enum Rarity
    {
        Common,
        Uncommon,
        Rare,
        Unique,
        Legendary,
        Mythic,
    }

    // 아이템 정보
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

    // 아이템 생성을 위한 빌더 인터페이스
    public interface IItemBuilder
    {
        void SetName(string name);
        void SetRarity(Rarity rarity);
        void SetDescription(string description);
        Item Build();
    }

    // 아이템 생성 빌더 클래스
    public class ItemBuilder : IItemBuilder
    {
        private Item item = new Item();
        public void SetName(string name) => item.name = name;
        public void SetRarity(Rarity rarity) => item.rarity = rarity;
        public void SetDescription(string description) => item.description = description;
        public Item Build() => item;
    }

    // 테스트용
    public class BuilderPatternExample : MonoBehaviour
    {
        void Start()
        {
            // 빌더를 사용하여 아이템 생성 및 설정
            IItemBuilder itemBuilder = new ItemBuilder();
            itemBuilder.SetName("Sword");
            itemBuilder.SetRarity(Rarity.Rare);
            itemBuilder.SetDescription("평범한 칼.");

            Item sword = itemBuilder.Build();

            // 생성된 아이템 정보 출력
            Debug.Log(sword);
        }
    }


}
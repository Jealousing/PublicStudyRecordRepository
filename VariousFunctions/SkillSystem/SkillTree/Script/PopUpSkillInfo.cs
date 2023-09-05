using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopUpSkillInfo : MonoBehaviour
{
    [SerializeField] private Image skillImage;
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillType;
    [SerializeField] private TextMeshProUGUI skilAttackBehaviorType;
    [SerializeField] private TextMeshProUGUI skillDescription;

    private static readonly Dictionary<AttackBehaviorType, string> attackTypeMap = new Dictionary<AttackBehaviorType, string>()
    {
        {AttackBehaviorType.Default, ""},
        {AttackBehaviorType.Fists, "Fist"},
        {AttackBehaviorType.SwordAndShield, "Sword&Shield"},
        {AttackBehaviorType.Bow, "Bow"},
        {AttackBehaviorType.Magic, "Magic"}
    };

    public void Setting(SkillInfo skillInfo)
    {
        skillImage.sprite = skillInfo.skillIcon;
        skillName.text = skillInfo.skillName;
        skillType.text = skillInfo.skillType.ToString()+" Skill";
        skilAttackBehaviorType.text = AttackBehaviorTypeString(skillInfo.attackBehavior);
        skillDescription.text = skillInfo.skillDescription;
    }
    private string AttackBehaviorTypeString(AttackBehaviorType type)
    {
        if (attackTypeMap.ContainsKey(type))
        {
            return attackTypeMap[type];
        }
        else
        {
            return "";
        }
    }
}

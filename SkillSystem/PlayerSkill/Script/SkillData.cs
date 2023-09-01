using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEditor;

public enum SkillType
{
    None,

    // 캐릭터를 보조하거나, 스킬을 보조하는 타입
    Passive,
    Assist,

    // 사용할 수 있는 스킬 타입
    Active,
    Toggle
}

[System.Serializable]
public class SkillInfo
{
    // 이름, 스킬레벨, 최대레벨, 스킬설명, 스킬아이콘, 스킬타입, 공격행동타입
    public string skillName;
    public int skillLV;
    public int skillDefaultLV;
    public int skillMaxLV;
    public string skillDescription;
    public Sprite skillIcon;
    public string skillIconPath;
    public SkillType skillType;
    public AttackBehaviorType attackBehavior;

    // 스킬 구현부 오브젝트, 스킬쿨타임
    [NonSerialized] public GameObject skillScript;
    public string skillScriptPath;
    public float SkillCooldownTime;

    

    public void OnBeforeSerialize()
    {
        if (skillIcon != null)
        {
            skillIconPath = AssetDatabase.GetAssetPath(skillIcon);
        }
        if(skillScript!= null)
        {
            skillScriptPath = AssetDatabase.GetAssetPath(skillScript);
        }
    }

    public void OnAfterDeserialize()
    {
        string resourcesPath;
        if (!string.IsNullOrEmpty(skillIconPath))
        {
            resourcesPath = Path.ChangeExtension(skillIconPath.Replace("\\", "/").Replace("Assets/Resources/", ""), null);
            skillIcon = Resources.Load<Sprite>(resourcesPath);
        }
        if (!string.IsNullOrEmpty(skillScriptPath))
        {
            resourcesPath = Path.ChangeExtension(skillScriptPath.Replace("\\", "/").Replace("Assets/Resources/", ""), null);
            skillScript = Resources.Load<GameObject>(resourcesPath);
        }
    }
}


[CreateAssetMenu(fileName = "New Skill", menuName = "Skills/Skill")]
public class SkillData : ScriptableObject 
{
    public SkillInfo skillInfo;
}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEditor;

public enum SkillType
{
    None,

    // ĳ���͸� �����ϰų�, ��ų�� �����ϴ� Ÿ��
    Passive,
    Assist,

    // ����� �� �ִ� ��ų Ÿ��
    Active,
    Toggle
}

[System.Serializable]
public class SkillInfo
{
    // �̸�, ��ų����, �ִ뷹��, ��ų����, ��ų������, ��ųŸ��, �����ൿŸ��
    public string skillName;
    public int skillLV;
    public int skillDefaultLV;
    public int skillMaxLV;
    public string skillDescription;
    public Sprite skillIcon;
    public string skillIconPath;
    public SkillType skillType;
    public AttackBehaviorType attackBehavior;

    // ��ų ������ ������Ʈ, ��ų��Ÿ��
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



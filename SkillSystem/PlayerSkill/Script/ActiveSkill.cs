using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class ActiveSkill : Skill
{
    protected float skillDamage;
    public float skillCooldownTime;
    public bool isAiming = false;
    public Action<int> LinkedSkillEvent;

    public bool isToggle = false;

    public SkillQuickSlot QuickSlot;

    public SkillData[] assistSkills;

    public abstract void Setting();
    public abstract void CheckRange();
    public abstract void ResetRange();
    public abstract void Activate();
    public abstract void Deactivate();

    protected IEnumerator SkillCooldownTimer()
    {
        skillCooldownTime = skillData.skillInfo.SkillCooldownTime;
        StartCoroutine(QuickSlot.CooldownTimer());
        while (skillCooldownTime >= 0)
        {
            skillCooldownTime -= Time.deltaTime;
            yield return null;
        }
    }
}

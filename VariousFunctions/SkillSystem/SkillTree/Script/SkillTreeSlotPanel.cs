using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillTreeSlotPanel : MonoBehaviour
{
    public Image skillImageSlot;
    public SkillInfo skillInfo;
    public int number;

    public void SetSkillData(SkillInfo addSkillPrefabInfo)
    {
        skillImageSlot.enabled = true;
        skillInfo = addSkillPrefabInfo;
        skillImageSlot.sprite = skillInfo.skillIcon;
    }

    public void ResetPanel()
    {
        skillImageSlot.enabled = false;
        skillInfo = null;
        skillImageSlot.sprite = null;
    }

    public void AddSkillData(SkillInfo addSkillData)
    {
        // ��ų������ �߰��۾��ؾߵ�
        SetSkillData(addSkillData);
        UIManager.GetInstance.SkillTree.SetUseSkill(number, addSkillData);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillQuickSlot : MonoBehaviour
{
    public SkillInfo quickSlotSkillInfo;
    public Image quickSlotSkillImage;
    public ActiveSkill activeSkill;
    public GameObject cooldownObj;
    public TextMeshProUGUI text;

    public void UpdateSlot(SkillInfo skillInfo)
    {
        if (skillInfo != null && skillInfo.skillLV>0)
        {
            quickSlotSkillInfo = skillInfo;
            quickSlotSkillImage.enabled = true;
            quickSlotSkillImage.sprite = quickSlotSkillInfo.skillIcon;
            ActiveSkill temp = skillInfo.skillScript.GetComponent<ActiveSkill>();
            gameObject.AddComponent(temp.GetType());

            activeSkill = GetComponent<ActiveSkill>();
            activeSkill.QuickSlot = this;
            activeSkill.skillData = temp.skillData;
            activeSkill.assistSkills = temp.assistSkills;
            activeSkill.Setting();
        }
    }

    public bool CheckCooldownTime()
    {
        if (activeSkill == null) return false;

        if (activeSkill.skillCooldownTime <= 0) return true;
        else return false;
    }

    public IEnumerator CooldownTimer()
    {
        yield return null;

        while (true)
        {
            if(activeSkill.skillData.skillInfo.skillType == SkillType.Toggle)
            {
                //활성화
                if(activeSkill.isToggle)
                {
                    if (!cooldownObj.activeSelf)
                    {
                        cooldownObj.SetActive(true);
                    }
                    text.text = "ON";
                    yield break;
                }
                //스킬쿨타임(비활성화시)
                else if(!activeSkill.isToggle && activeSkill.skillCooldownTime > 0)
                {
                    if (!cooldownObj.activeSelf)
                    {
                        cooldownObj.SetActive(true);
                    }
                    text.text = activeSkill.skillCooldownTime.ToString("F1");
                }
                //비활성화 완료
                else
                {
                    cooldownObj.SetActive(false);
                    yield break;
                }
            }
            else
            {
                if (activeSkill.skillCooldownTime > 0)
                {
                    if (!cooldownObj.activeSelf)
                    {
                        cooldownObj.SetActive(true);
                    }
                    text.text = activeSkill.skillCooldownTime.ToString("F1");
                }
                else
                {
                    cooldownObj.SetActive(false);
                    yield break;
                }
            }
            yield return null;
        }
    }

}

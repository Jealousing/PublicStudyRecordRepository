using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillQuickSlotSystem : MonoBehaviour
{
    /*
     퀵슬롯 스킬부분에서 중요한것은 이 퀵슬롯이 변경되는 타임이다.
    지금 토이프로젝트로 만들고 있는 시스템 자체가 무기를 바꾸면 스킬이 바뀌어야되기때문에
    무기를 변경시에 퀵슬롯에 저장된스킬이 무기에 맞게 변경해야된다

    또한 스킬트리에서 스킬을 찍어주고 등록하고 저장하고 나올시 전체 데이터 복사해주고(무기에따른 퀵슬롯스킬등록)
    들고잇는 무기에 따른 퀵슬롯update해야됨.
     */

    public AttackBehaviorType attackBehavior;
    public SkillQuickSlot[] skillSlot;
    Dictionary<int, float> keyInputTime = new Dictionary<int, float>();

    // 스킬 키설정
    Dictionary<KeyCode, int> dic = new Dictionary<KeyCode, int>()
    {
        {KeyCode.E, 0},
        {KeyCode.R, 1},
        {KeyCode.LeftShift, 2},
        {KeyCode.T, 3},
    };
    private void Start()
    {
        UIManager.GetInstance.SkillTree.LoadQuickSlotSkill();
        PlayerInfo.GetInstance.combatInfo.checkWeaponScript();

        InputManager.GetInstance.OnKeyDown += HandleKeyDown;
        InputManager.GetInstance.OnKeyUp += HandleKeyUp;
        InputManager.GetInstance.OnKeyPress += HandleKeyPress;
    }

    private void OnDestroy()
    {
        /*
        InputManager.GetInstance.OnKeyDown -= HandleKeyDown;
        InputManager.GetInstance.OnKeyUp -= HandleKeyUp;
        InputManager.GetInstance.OnKeyPress -= HandleKeyPress;
        */
    }

    void HandleKeyDown(KeyCode keyCode)
    {
        if(dic.ContainsKey(keyCode))
        {
            ActiveSkillKeyDown(dic[keyCode]);
        }
       
    }

    void HandleKeyUp(KeyCode keyCode)
    {
        if (dic.ContainsKey(keyCode))
        {
            ActiveSkillKeyUp(dic[keyCode]);
        }
    }
    void HandleKeyPress(KeyCode keyCode)
    {
        if (dic.ContainsKey(keyCode))
        {
            ActiveSkillKeyPress(dic[keyCode]);
        }
    }

    private void ActiveSkillKeyDown(int slotNum)
    {
        // 키가 눌리지 않거나 데이터가없으면 리턴
        SkillQuickSlot currentSlot = skillSlot[slotNum];
        bool hasValidSkill = currentSlot.quickSlotSkillInfo.skillScript != null && currentSlot.CheckCooldownTime()
            && currentSlot.activeSkill.skillData.skillInfo.skillLV >0;

        if (!hasValidSkill) return;

        if (keyInputTime.ContainsKey(slotNum))
            keyInputTime[slotNum] = 0.0f;
    }

    private void ActiveSkillKeyUp(int slotNum)
    {
        SkillQuickSlot currentSlot = skillSlot[slotNum];
        bool hasValidSkill = currentSlot.quickSlotSkillInfo.skillScript != null && currentSlot.CheckCooldownTime() 
            && PlayerInfo.GetInstance.combatInfo.IsCombat && currentSlot.activeSkill.skillData.skillInfo.skillLV > 0;

        if (!hasValidSkill) return;
        if (keyInputTime[slotNum] > 0.5f)
        {
            skillSlot[slotNum].activeSkill.isAiming= true; 
        }
        keyInputTime[slotNum] = 0.0f;
        skillSlot[slotNum].activeSkill.Activate();
    }

    private void ActiveSkillKeyPress(int slotNum)
    {
        SkillQuickSlot currentSlot = skillSlot[slotNum];
        bool hasValidSkill = currentSlot.quickSlotSkillInfo.skillScript != null && currentSlot.CheckCooldownTime()
            && currentSlot.activeSkill.skillData.skillInfo.skillLV > 0;

        if (!hasValidSkill) return;

        if (!keyInputTime.ContainsKey(slotNum))
        {
            keyInputTime.Add(slotNum, 0);
        }

        keyInputTime[slotNum] += Time.deltaTime;

        if (keyInputTime[slotNum] > 0.5f)
        {
            skillSlot[slotNum].activeSkill.CheckRange();
        }
    }

    public void UpdateSlot()
    {
        for (int i = 0; i < skillSlot.Length; i++)
        {
            skillSlot[i].UpdateSlot(UIManager.GetInstance.SkillTree.QuickSlotSkill[(int)attackBehavior - 1, i]);
        }
    }

    // 무기변경시 호출 -> 무기 타입 변경되면 슬롯 업데이트
    public void SetAttackBehavior(AttackBehaviorType attackBehaviorType)
    {
        if (attackBehavior != attackBehaviorType)
        {
            attackBehavior = attackBehaviorType;
            UpdateSlot();
        }
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillQuickSlotSystem : MonoBehaviour
{
    /*
     ������ ��ų�κп��� �߿��Ѱ��� �� �������� ����Ǵ� Ÿ���̴�.
    ���� ����������Ʈ�� ����� �ִ� �ý��� ��ü�� ���⸦ �ٲٸ� ��ų�� �ٲ��ߵǱ⶧����
    ���⸦ ����ÿ� �����Կ� ����Ƚ�ų�� ���⿡ �°� �����ؾߵȴ�

    ���� ��ųƮ������ ��ų�� ����ְ� ����ϰ� �����ϰ� ���ý� ��ü ������ �������ְ�(���⿡���� �����Խ�ų���)
    ����մ� ���⿡ ���� ������update�ؾߵ�.
     */

    public AttackBehaviorType attackBehavior;
    public SkillQuickSlot[] skillSlot;
    Dictionary<int, float> keyInputTime = new Dictionary<int, float>();

    // ��ų Ű����
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
        // Ű�� ������ �ʰų� �����Ͱ������� ����
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

    // ���⺯��� ȣ�� -> ���� Ÿ�� ����Ǹ� ���� ������Ʈ
    public void SetAttackBehavior(AttackBehaviorType attackBehaviorType)
    {
        if (attackBehavior != attackBehaviorType)
        {
            attackBehavior = attackBehaviorType;
            UpdateSlot();
        }
    }


}

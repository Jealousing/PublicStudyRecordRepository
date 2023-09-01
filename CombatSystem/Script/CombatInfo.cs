using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CombatInfo : MonoBehaviour
{
    [NonSerialized] public PlayerInfo playerInfo;
    [NonSerialized] public bool IsCombat = false;
    [NonSerialized] public Animator animator;
    [NonSerialized] public bool isAnimator;
    [NonSerialized] public int aniHashChangeCombat;
    [NonSerialized] public int aniHashCombat;
    [NonSerialized] public int aniHashAttack;
    [NonSerialized] public int aniHashAniMult;

    public CombatAbstract curScript;
    string path = "CombatOverrideAniController/";
    public GameObject Crosshair;
    private void Awake()
    {
        CombatAniHashSet();
    }
    void CombatAniHashSet()
    {
        aniHashChangeCombat = Animator.StringToHash("ChangeCombat");
        aniHashCombat = Animator.StringToHash("Combat");
        aniHashAttack = Animator.StringToHash("Attack");
        aniHashAniMult = Animator.StringToHash("AniMult");
     
    }

    void Start()
    {
        playerInfo = GetComponent<PlayerInfo>();
        isAnimator = TryGetComponent(out animator);
        animator.SetFloat(aniHashAniMult, 1f);
    }

    void SetAniOverrideController(string name)
    {
        AnimatorOverrideController changeController = Resources.Load(path+name) as AnimatorOverrideController;
        animator.runtimeAnimatorController = changeController;
    }

    public void checkWeaponScript()
    {
        /*
        1) ���ָ� ( ��մ� ���� x)
        2) �˰� ����
        3) ��
        4) ���и�
        5) ����
        6) Ȱ
        7) ����
         */

        // ��մ� �������
        if (playerInfo.leftWeapon == null &&
            playerInfo.rightWeapon == null)
        {
            // �Ǽ� ��ũ��Ʈ ��� 
            if (!this.gameObject.GetComponent<BasicCombat>())
            {
                if (curScript != null) Destroy(curScript);
                curScript = this.gameObject.AddComponent<BasicCombat>();
                UIManager.GetInstance.QuickSlot.SkillSlot.SetAttackBehavior(AttackBehaviorType.Fists);
                SetAniOverrideController("BasicCombat");
            }

        }
        // ��մ� ���Ⱑ �ִ� ���
        else if (playerInfo.leftWeapon != null &&
            playerInfo.rightWeapon != null)
        {
            WeaponInfo leftWeaponInfo = playerInfo.leftWeapon.GetComponent<WeaponInfo>();
            WeaponInfo rightWeaponInfo = playerInfo.rightWeapon.GetComponent<WeaponInfo>();

            // �˰� ����
            if (leftWeaponInfo.weaponType == WeaponType.SHIELD &&
                rightWeaponInfo.weaponType == WeaponType.ONEHAND_SWORD)
            {
                // �˰� �ǵ带 ����ϴ� ��ũ��Ʈ ���
                if (!this.gameObject.GetComponent<SwordShieldCombat>())
                {
                    if (curScript != null) Destroy(curScript);
                    curScript= this.gameObject.AddComponent<SwordShieldCombat>();
                    UIManager.GetInstance.QuickSlot.SkillSlot.SetAttackBehavior(AttackBehaviorType.SwordAndShield);
                    SetAniOverrideController("SwordShieldCombat");
                }
                    
            }
            // Ȱ
            else if (leftWeaponInfo.weaponType == WeaponType.BOW)
            {
                // Ȱ�� ����ϴ� ��ũ��Ʈ ���
                if (!this.gameObject.GetComponent<BowCombat>())
                {
                    if (curScript != null) Destroy(curScript);
                    curScript = this.gameObject.AddComponent<BowCombat>();
                    UIManager.GetInstance.QuickSlot.SkillSlot.SetAttackBehavior(AttackBehaviorType.Bow);
                    SetAniOverrideController("BowCombat");
                }

            }
            else if (leftWeaponInfo.weaponType == WeaponType.TWOHAND_AXE)
            {

            }
            else if (leftWeaponInfo.weaponType == WeaponType.TWOHAND_SWORD)
            {

            }
        }
        // �޼� ���⸸ ����ϴ� ���
        else if (playerInfo.leftWeapon != null)
        {
            WeaponInfo leftWeaponInfo = playerInfo.leftWeapon.GetComponent<WeaponInfo>();
            // �ǵ�
            if (leftWeaponInfo.weaponType == WeaponType.SHIELD)
            {

            }
        }
        // ������ ���⸸ ����ϴ� ���
        else if (playerInfo.rightWeapon != null)
        {
            WeaponInfo rightWeaponInfo = playerInfo.rightWeapon.GetComponent<WeaponInfo>();
            // �Ѽյ���
            if (rightWeaponInfo.weaponType == WeaponType.ONEHAND_AXE)
            {

            }
        }

    }


}

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
        1) 맨주먹 ( 양손다 무기 x)
        2) 검과 방패
        3) 검
        4) 방패만
        5) 도끼
        6) 활
        7) 마법
         */

        // 양손다 무기없음
        if (playerInfo.leftWeapon == null &&
            playerInfo.rightWeapon == null)
        {
            // 맨손 스크립트 사용 
            if (!this.gameObject.GetComponent<BasicCombat>())
            {
                if (curScript != null) Destroy(curScript);
                curScript = this.gameObject.AddComponent<BasicCombat>();
                UIManager.GetInstance.QuickSlot.SkillSlot.SetAttackBehavior(AttackBehaviorType.Fists);
                SetAniOverrideController("BasicCombat");
            }

        }
        // 양손다 무기가 있는 경우
        else if (playerInfo.leftWeapon != null &&
            playerInfo.rightWeapon != null)
        {
            WeaponInfo leftWeaponInfo = playerInfo.leftWeapon.GetComponent<WeaponInfo>();
            WeaponInfo rightWeaponInfo = playerInfo.rightWeapon.GetComponent<WeaponInfo>();

            // 검과 방패
            if (leftWeaponInfo.weaponType == WeaponType.SHIELD &&
                rightWeaponInfo.weaponType == WeaponType.ONEHAND_SWORD)
            {
                // 검과 실드를 사용하는 스크립트 사용
                if (!this.gameObject.GetComponent<SwordShieldCombat>())
                {
                    if (curScript != null) Destroy(curScript);
                    curScript= this.gameObject.AddComponent<SwordShieldCombat>();
                    UIManager.GetInstance.QuickSlot.SkillSlot.SetAttackBehavior(AttackBehaviorType.SwordAndShield);
                    SetAniOverrideController("SwordShieldCombat");
                }
                    
            }
            // 활
            else if (leftWeaponInfo.weaponType == WeaponType.BOW)
            {
                // 활을 사용하는 스크립트 사용
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
        // 왼손 무기만 사용하는 경우
        else if (playerInfo.leftWeapon != null)
        {
            WeaponInfo leftWeaponInfo = playerInfo.leftWeapon.GetComponent<WeaponInfo>();
            // 실드
            if (leftWeaponInfo.weaponType == WeaponType.SHIELD)
            {

            }
        }
        // 오른손 무기만 사용하는 경우
        else if (playerInfo.rightWeapon != null)
        {
            WeaponInfo rightWeaponInfo = playerInfo.rightWeapon.GetComponent<WeaponInfo>();
            // 한손도끼
            if (rightWeaponInfo.weaponType == WeaponType.ONEHAND_AXE)
            {

            }
        }

    }


}

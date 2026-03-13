using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordShieldCombat : CombatAbstract
{

    WeaponInfo shieldInfo;
    WeaponInfo swordInfo;

    int aniHashIsShield; 

    void Start()
    {
        shieldInfo = combatInfo.playerInfo.leftWeapon.GetComponent<WeaponInfo>();
        swordInfo = combatInfo.playerInfo.rightWeapon.GetComponent<WeaponInfo>();
        aniHashIsShield = Animator.StringToHash("Shield");
    }


    void Update()
    {
        BasicAttackSystem();
    }

    protected override void BasicAttackSystem()
    {
        if (combatInfo.playerInfo.actionLock) return;
        else if (combatInfo.IsCombat && Input.GetMouseButtonDown(0))
        {
            combatInfo.playerInfo.actionLock = true;
            combatInfo.playerInfo.movementInfo.inputLock = true;

            if (attackCoroutine != null)
            {
                StopCoroutine(attackCoroutine);
            }

            StartCoroutine(attackCoroutine = attackAni());
        }
        else if (combatInfo.IsCombat )
        {
            if( Input.GetMouseButtonDown(1))
            {
                combatInfo.animator.SetBool(aniHashIsShield, true);

                // 보는방향으로 회전 추후 방어 추가
                Vector3 camForward = combatInfo.playerInfo.cameraInfo.cameraObj.transform.forward;
                camForward.y = 0;
                Quaternion newRotation = Quaternion.LookRotation(camForward);
                transform.rotation = newRotation;

                combatInfo.playerInfo.movementInfo.inputLock = true;
            }
            if (Input.GetMouseButton(1))
            {
/*
                // 보는방향으로 회전 추후 방어 추가
                Vector3 camForward = combatInfo.playerInfo.cameraInfo.cameraTransform.forward;
                camForward.y = 0;
                Quaternion newRotation = Quaternion.LookRotation(camForward);
                transform.rotation = newRotation;
*/
            }
            if(Input.GetMouseButtonUp(1))
            {
                combatInfo.animator.SetBool(aniHashIsShield, false);
                combatInfo.playerInfo.movementInfo.inputLock = false;
            }
            
        }
    }

    IEnumerator attackAni()
    {
        // 공격 트리거 활성화
        combatInfo.animator.SetTrigger(combatInfo.aniHashAttack);

        aniName = "Attack" + attackCount;

        // 실행될때까지 대기
        while (!combatInfo.animator.GetCurrentAnimatorStateInfo(1).IsName(aniName))
        {
            combatInfo.animator.SetTrigger(combatInfo.aniHashAttack);
            yield return null;
        }
        combatInfo.animator.ResetTrigger(combatInfo.aniHashAttack);

        
        
        attackCount++;
        if (attackCount >= maxComboAttack)
            attackCount = 0;

        while (combatInfo.animator.GetCurrentAnimatorStateInfo(1).IsName(aniName) &&
           combatInfo.animator.GetCurrentAnimatorStateInfo(1).normalizedTime <= 0.35f)
        {
            yield return null;
        }

        swordInfo.capsuleCollider.enabled = true;
        swordInfo.trilRenderer.enabled = true;

        while (combatInfo.animator.GetCurrentAnimatorStateInfo(1).IsName(aniName) &&
            combatInfo.animator.GetCurrentAnimatorStateInfo(1).normalizedTime <= 0.6f)
        {
            yield return null;
        }
        swordInfo.trilRenderer.enabled = false;
        swordInfo.capsuleCollider.enabled = false;
        combatInfo.playerInfo.actionLock = false;

        while (combatInfo.animator.GetCurrentAnimatorStateInfo(1).IsName(aniName) &&
            combatInfo.animator.GetCurrentAnimatorStateInfo(1).normalizedTime <= 1f)
        {
            yield return null;
        }
        
        combatInfo.playerInfo.movementInfo.inputLock = false;
        attackCount = 0;

        yield return null;
    }
}

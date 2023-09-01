using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatStateSystem : MonoBehaviour
{
    /*
     캐릭터가 비전투 상태에서 전투상태로 변경할때
     들고 있는 무기를 확인-> 무기에 맞는 스크립트 및 Animator Override Controller 추가 -> 
     무기에 따른 전투상태로 변경 애니메이션 출력
     */

    CombatInfo combatInfo;
    string aniName;

    void Start()
    {
        combatInfo = GetComponent<CombatInfo>();
    }


    void Update()
    {
        // 탭키를 누르거나 평타키를 눌렀을때 전투상태가 아닐경우 전투상태로 변경
        if ((Input.GetKeyDown(KeyCode.Tab)) ||
            Input.GetMouseButtonDown(0) && !combatInfo.IsCombat)
        {
            combatInfo.checkWeaponScript();
            changeCombat();
            return;
        }
    }
    void changeCombat()
    {
        if (combatInfo.playerInfo.actionLock) return;
        combatInfo.playerInfo.actionLock = true;
        if (combatInfo.IsCombat)
        {
            combatInfo.IsCombat = false;

        }
        else
        {
            combatInfo.IsCombat = true;
        }
        StartCoroutine(ChangeCombatAni());
    }

    IEnumerator ChangeCombatAni()
    {
        // 전투상태 변경 트리거 활성화
        combatInfo.animator.SetTrigger(combatInfo.aniHashChangeCombat);
        combatInfo.animator.SetBool(combatInfo.aniHashCombat, combatInfo.IsCombat);

        combatInfo.playerInfo.movementInfo.inputLock = true;
        aniName = "changeCombat" + combatInfo.IsCombat.ToString();


        // 실행될때까지 대기
        while (!combatInfo.animator.GetCurrentAnimatorStateInfo(1).IsName(aniName))
        {
            yield return null;
        } 
        while (combatInfo.animator.GetCurrentAnimatorStateInfo(1).IsName(aniName) &&
            combatInfo.animator.GetCurrentAnimatorStateInfo(1).normalizedTime <= 0.5f)
        {
            yield return null;
        }

        // 전투상태로 진입 : 등에서 -> 손으로
        if (combatInfo.IsCombat)
        {
            if (combatInfo.playerInfo.leftWeapon !=null && combatInfo.playerInfo.leftWeapon.GetComponent<WeaponInfo>().wearDirLeft)
            {
                WeaponInfo leftWeapon = combatInfo.playerInfo.leftWeapon.GetComponent<WeaponInfo>();
                combatInfo.playerInfo.leftWeapon.transform.SetParent(combatInfo.playerInfo.movementInfo.handIK.leftHand.transform);
                leftWeapon.transform.localPosition = leftWeapon.combatTr.pos;
                leftWeapon.transform.localRotation = Quaternion.Euler(leftWeapon.combatTr.rot);
            }
                
            if (combatInfo.playerInfo.rightWeapon != null && !combatInfo.playerInfo.rightWeapon.GetComponent<WeaponInfo>().wearDirLeft)
            {
                WeaponInfo rightWeapon = combatInfo.playerInfo.rightWeapon.GetComponent<WeaponInfo>();
                combatInfo.playerInfo.rightWeapon.transform.SetParent(combatInfo.playerInfo.movementInfo.handIK.rightHand.transform);
                rightWeapon.transform.localPosition = rightWeapon.combatTr.pos;
                rightWeapon.transform.localRotation = Quaternion.Euler(rightWeapon.combatTr.rot);
            }
        }
        // 비전투상태로 진입 : 손에서 -> 등으로
        else
        {
            if (combatInfo.playerInfo.leftWeapon != null && combatInfo.playerInfo.leftWeapon.GetComponent<WeaponInfo>().wearDirLeft)
            {
                WeaponInfo leftWeapon = combatInfo.playerInfo.leftWeapon.GetComponent<WeaponInfo>();
                combatInfo.playerInfo.leftWeapon.transform.SetParent(combatInfo.playerInfo.wearPoint.transform);
                leftWeapon.transform.localPosition = leftWeapon.wearTr.pos;
                leftWeapon.transform.localRotation = Quaternion.Euler(leftWeapon.wearTr.rot);
            }
                
            if (combatInfo.playerInfo.rightWeapon != null && !combatInfo.playerInfo.rightWeapon.GetComponent<WeaponInfo>().wearDirLeft)
            {
                WeaponInfo rightWeapon = combatInfo.playerInfo.rightWeapon.GetComponent<WeaponInfo>();
                combatInfo.playerInfo.rightWeapon.transform.SetParent(combatInfo.playerInfo.wearPoint.transform);
                rightWeapon.transform.localPosition = rightWeapon.wearTr.pos;
                rightWeapon.transform.localRotation = Quaternion.Euler(rightWeapon.wearTr.rot);
            }
                
        }

        while (combatInfo.animator.GetCurrentAnimatorStateInfo(1).IsName(aniName) &&
            combatInfo.animator.GetCurrentAnimatorStateInfo(1).normalizedTime <= 1f)
        {
            yield return null;
        }
        combatInfo.playerInfo.actionLock = false;
        combatInfo.playerInfo.movementInfo.inputLock = false;
        yield return null;
    }
}

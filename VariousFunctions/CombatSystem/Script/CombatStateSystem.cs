using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatStateSystem : MonoBehaviour
{
    /*
     ĳ���Ͱ� ������ ���¿��� �������·� �����Ҷ�
     ��� �ִ� ���⸦ Ȯ��-> ���⿡ �´� ��ũ��Ʈ �� Animator Override Controller �߰� -> 
     ���⿡ ���� �������·� ���� �ִϸ��̼� ���
     */

    CombatInfo combatInfo;
    string aniName;

    void Start()
    {
        combatInfo = GetComponent<CombatInfo>();
    }


    void Update()
    {
        // ��Ű�� �����ų� ��ŸŰ�� �������� �������°� �ƴҰ�� �������·� ����
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
        // �������� ���� Ʈ���� Ȱ��ȭ
        combatInfo.animator.SetTrigger(combatInfo.aniHashChangeCombat);
        combatInfo.animator.SetBool(combatInfo.aniHashCombat, combatInfo.IsCombat);

        combatInfo.playerInfo.movementInfo.inputLock = true;
        aniName = "changeCombat" + combatInfo.IsCombat.ToString();


        // ����ɶ����� ���
        while (!combatInfo.animator.GetCurrentAnimatorStateInfo(1).IsName(aniName))
        {
            yield return null;
        } 
        while (combatInfo.animator.GetCurrentAnimatorStateInfo(1).IsName(aniName) &&
            combatInfo.animator.GetCurrentAnimatorStateInfo(1).normalizedTime <= 0.5f)
        {
            yield return null;
        }

        // �������·� ���� : ��� -> ������
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
        // ���������·� ���� : �տ��� -> ������
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

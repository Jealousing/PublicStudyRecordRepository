using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BasicCombat : CombatAbstract
{
 

    float attackDamage = 50.0f;
    float attackRange = 0.5f;
    Transform attackPoint = null;
    Transform[] attackPointlist = new Transform [3];
   

    private void Start()
    {
        Setting();
    }

    void Setting()
    {
        if (attackPointlist[0] != null) return;

        attackPointlist[0] = combatInfo.playerInfo.movementInfo.handIK.leftHand.transform;
        attackPointlist[1] = combatInfo.playerInfo.movementInfo.handIK.rightHand.transform;
        attackPointlist[2] = combatInfo.playerInfo.movementInfo.footIK.rightFootTransform;
    }

    void Update()
    {
        BasicAttackSystem();
    }

    private void OnDrawGizmos()
    {
        if (attackPointlist[0] == null) return;
        if (attackPoint == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    protected override void BasicAttackSystem()
    {
        if (combatInfo.playerInfo.actionLock) return;
        else if(combatInfo.IsCombat && Input.GetMouseButtonDown(0))
        {
            combatInfo.playerInfo.actionLock = true;
            combatInfo.playerInfo.movementInfo.inputLock = true;

            if (attackCoroutine != null)
            {
                StopCoroutine(attackCoroutine);
            }

            StartCoroutine(attackCoroutine = attackAni());
        }
       
    }


   

    IEnumerator attackAni()
    {
        // ���� Ʈ���� Ȱ��ȭ
        combatInfo.animator.SetTrigger(combatInfo.aniHashAttack);

        aniName = "Attack" + attackCount;

        // ����ɶ����� ���
        while (!combatInfo.animator.GetCurrentAnimatorStateInfo(1).IsName(aniName))
        {
            combatInfo.animator.SetTrigger(combatInfo.aniHashAttack);
            yield return null;
        }
        combatInfo.animator.ResetTrigger(combatInfo.aniHashAttack);

        attackPoint = attackPointlist[attackCount++];
        if (attackCount >= maxComboAttack)
            attackCount = 0;

        while (combatInfo.animator.GetCurrentAnimatorStateInfo(1).IsName(aniName) &&
            combatInfo.animator.GetCurrentAnimatorStateInfo(1).normalizedTime <= 0.5f)
        {
            yield return null;
        }

        // attackPoint ��ġ�� �������� attackRange �ݰ� ���� Collider�� ��� ������
        Collider[] hitColliders = Physics.OverlapSphere(attackPoint.position, attackRange);

        // ������ Collider �߿��� "Monster" �±׸� ���� ��ü�� ���ؼ��� ������� ����
        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                hitCollider.GetComponent<EnemyInfo>().TakeDamage(attackDamage);
            }
        }

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ȱ ��ų�� �� ����, ȭ���� 3���� �߻��ϴ� ��Ƽ�� ��ų.
/// </summary>
public class TripleShot : ActiveSkill
{
    Damage[] damage = new Damage[3];
    BowCombat bowInfo;
    int[] rotData = new int[] { -30,0,30 };
    float[] arrowSpacingOffsets = new float[] {-0.25f, 0, +0.25f };
    [HideInInspector] public int autoTrackingArrowCount = 2;

    public override void Setting()
    {
        bowInfo = (BowCombat)PlayerInfo.GetInstance.combatInfo.curScript;

        if (assistSkills != null)
        {
            for (int i = 0; i < assistSkills.Length; i++)
            {
                assistSkills[i].skillInfo.skillScript.GetComponent<AssistSkill>().Assist(this);
            }
        }
    }
    public override void CheckRange()
    {
        for(int i=0;i<3;i++)
        {
            RangeSet(i);
        }
    }

    public override void ResetRange()
    {
        if (damage[0] == null) return;

        for (int i = damage.Length - 1; i >= 0; i--)
        {
            damage[i].StopDraw();
        }
        damage = new Damage[3];
    }

    public override void Activate()
    {
        StartCoroutine(UseActiveSkill());
    }

    public override void Deactivate()
    {
    }

    private void RangeSet(int num)
    {
        Vector3 pos = PlayerInfo.GetInstance.transform.position + new Vector3(0, 0.001f, 0);
        Quaternion rot = PlayerInfo.GetInstance.transform.rotation;

        if (damage[num] ==null)
        {
            damage[num] = DamageManager.GetInstance.CallDamageRange(DamageRangeType.CUBE, true, 0, 0, LayerMask.GetMask("Enemy"),
           pos, rot * Quaternion.Euler(new Vector3(0, rotData[num], 0)), 0.5f, 7.0f, 0.5f);
        }
        else 
        {
            damage[num].transform.position = pos;
            damage[num].transform.rotation = rot * Quaternion.Euler(new Vector3(0, rotData[num], 0));
        }
    }

    IEnumerator UseActiveSkill()
    {
        // ���� ���� �Ǿ��ִ��� Ȯ�� �� ����, ��ũ ��ų ȣ��, ��Ÿ�� ������ ���
        ResetRange();
        LinkedSkillEvent?.Invoke(autoTrackingArrowCount);
        StartCoroutine(SkillCooldownTimer());

        // �Է����� �� �ִϸ��̼� ���� ���
        PlayerInfo.GetInstance.movementInfo.inputLock = true;
        PlayerInfo.GetInstance.combatInfo.animator.SetTrigger(bowInfo.aniHashBowZoom);
        bowInfo.bowAni.SetTrigger(bowInfo.aniHashBowString);

        // ���� ���¿� ���� Ÿ�� ������� ���� ( ���� �������� ������� ��� ����� ���� Ÿ������ ��� ȸ�� �� �߻� )
        if(!isAiming)
        {
            PlayerInfo.GetInstance.CheckAndRotateToTarget(10.0f);
        }
        isAiming = false;

        // �ִϸ��̼� ���������� ���
        while (!PlayerInfo.GetInstance.combatInfo.animator.GetCurrentAnimatorStateInfo(1).IsName("DrawArrow (BowCombat)"))
        {
            yield return null;
        }
       
        // �޽� ������ ���� ��ġ �� ���� ���� �� ȭ�� �ʱ�ȭ
        Vector3 pos = PlayerInfo.GetInstance.transform.position + new Vector3(0, 0.001f, 0);
        Quaternion rot = PlayerInfo.GetInstance.transform.rotation;
        ArrowController[] arrow= new ArrowController[3];

        // ��ų������ ����
        skillDamage = skillData.skillInfo.skillLV * PlayerInfo.GetInstance.LV * 130.0f;
        
        // ������ ���� �� �ִϸ��̼� ����
        for (int i = 0; i < 3; i++)
        {
            DamageManager.GetInstance.CallDamageRange(DamageRangeType.CUBE, false, 0.25f, skillDamage, LayerMask.GetMask("Enemy"),
          pos, rot * Quaternion.Euler(new Vector3(0, rotData[i], 0)), 0.5f, 7.0f, 0.5f);
        }
        PlayerInfo.GetInstance.combatInfo.animator.SetTrigger(PlayerInfo.GetInstance.combatInfo.aniHashAttack);
        PlayerInfo.GetInstance.combatInfo.animator.SetFloat(PlayerInfo.GetInstance.combatInfo.aniHashAniMult, 2f);
        while (!PlayerInfo.GetInstance.combatInfo.animator.GetCurrentAnimatorStateInfo(1).IsName("ShotArrow (BowCombat)"))
        {
            yield return null;
        }

        // ȭ�� ������ƮǮ���� �������� �� ����, �߻�
        Vector3 dir;
        for (int i=0; i < 3; i++)
        {
            arrow[i] = bowInfo.arrowPool.Get();
            arrow[i].transform.parent = arrow[i].objectPoolParent.transform;
            arrow[i].trailRenderer.enabled = true;
            arrow[i].arrowRigidbody.isKinematic = false;
            arrow[i].arrowRigidbody.freezeRotation = true;
            arrow[i].releaseTime = 0.25f;
            arrow[i].IsTrigger = false;
            arrow[i].transform.position = bowInfo.arrowPrefab.transform.position;
            arrow[i].transform.position += PlayerInfo.GetInstance.transform.right * arrowSpacingOffsets[i];

            dir = Quaternion.Euler(new Vector3(0, rotData[i], 0)) * PlayerInfo.GetInstance.transform.forward;
            arrow[i].arrowRigidbody.velocity = dir * 30f;
            arrow[i].dirSet();
        }

        // �ݵ�
        PlayerInfo.GetInstance.movementInfo.player_rigidbody.AddForce(-PlayerInfo.GetInstance.transform.forward * 3.0f, ForceMode.Impulse);

        // ������ ����
        yield return new WaitForSeconds(0.15f);
        PlayerInfo.GetInstance.movementInfo.inputLock = false;

    }

}

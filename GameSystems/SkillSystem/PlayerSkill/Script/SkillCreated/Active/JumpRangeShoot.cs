using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 활 스킬의 한 종류, 점프해서 범위 공격화살을 발사하는 액티브 스킬.
/// </summary>
public class JumpRangeShoot : ActiveSkill
{
    Damage damage;
    BowCombat bowinfo;
    public float maxDistance;
    Vector3 hitPlace = Vector3.zero;

    public override void Setting()
    {
        bowinfo = (BowCombat)PlayerInfo.GetInstance.combatInfo.curScript;
    }

    public override void CheckRange()
    {
        /*
         
        범위 확인에서 해야될일 : 
        범위 지정 , 발사할 위치 지정

        카메라가 보는 방향으로 raycast 진행 이때 플레이어와 무시해야되는 레이어마스크 무시하도록 설정
        이때 바닥 또는 몬스터 또는 벽에 raycast가 hit되면 그 바닥으로 레이를 하나더 쏴서 범위가 바닥에 그려지도록 변경

        만약 허공을 바라봐서 레이가 맞은게 없을경우 최대거리에서 아래로 레이를 쏘게해서 그곳을 지정하도록 만들기.
         
        이제 hitpoint가 생기면 그 곳에 범위지정 생성 및 위치조정

         */

        int layerMask = ~(1 << LayerMask.NameToLayer("Player") | 
            1 << LayerMask.NameToLayer("Ignore Raycast"));

        Camera mainCamera = PlayerInfo.GetInstance.cameraInfo.cameraObj;

        Vector3 cameraDirection = mainCamera.transform.forward;
        maxDistance = Vector3.Distance(mainCamera.transform.position, PlayerInfo.GetInstance.transform.position) + 7.5f;

        RaycastHit hit;
        Vector3 hitPoint =Vector3.zero;


        if (Physics.Raycast(mainCamera.transform.position, cameraDirection, out hit,
            maxDistance, layerMask))
        {
            hitPoint = hit.point;
            
            // 바닥을 가르키도록 설정
            if (Physics.Raycast(hitPoint, Vector3.down, out hit, maxDistance, layerMask))
            {
                hitPoint = hit.point;
            }
        }
        else if (Physics.Raycast(mainCamera.transform.position + cameraDirection * maxDistance,
        Vector3.down, out hit, maxDistance, layerMask))
        {
            hitPoint = hit.point;
        }


        if(hitPoint != Vector3.zero)
        {

            Vector3 pos = hitPoint + new Vector3(0, 0.001f, 0);
            hitPlace = pos;
            Quaternion rot = PlayerInfo.GetInstance.transform.rotation;

            if (damage == null)
            {
                damage = DamageManager.GetInstance.CallDamageRange(DamageRangeType.DONUT
                    , true, 0, 0, LayerMask.GetMask("Enemy"), pos, 0, 3.0f);
            }
            else
            {
                damage.transform.position = pos;
                damage.transform.rotation = rot;
            }
        }
        else
        {
            hitPlace = Vector3.zero;
        }
    }

    public override void ResetRange()
    {
        if (damage == null) return;

        damage.StopDraw();
        damage = null;
    }

    public override void Activate()
    {
        StartCoroutine(UseActiveSkill());
    }


    public override void Deactivate()
    {

    }
    IEnumerator UseActiveSkill()
    {
        /*

      설정된 hitPlace가 있으면 그 곳을 향해 회전 , 아니라면 근처 타겟으로 회전
      및 뒷 점프 이후 화살발사

       */
        ResetRange();
        PlayerInfo.GetInstance.movementInfo.inputLock = true;
        LinkedSkillEvent?.Invoke(2);
        StartCoroutine(SkillCooldownTimer());
        // 회전
        if (hitPlace != Vector3.zero)
        {
            Vector3 direction = hitPlace - PlayerInfo.GetInstance.transform.position;
            float targetRotation = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            Quaternion targetQuaternion = Quaternion.Euler(0, targetRotation, 0);
            PlayerInfo.GetInstance.transform.rotation = targetQuaternion;

        }
        else
        {
            PlayerInfo.GetInstance.CheckAndRotateToTarget(10.0f);
            hitPlace = PlayerInfo.GetInstance.transform.position + new Vector3(0, 0.001f, 0) + PlayerInfo.GetInstance.transform.forward * 5.0f;
        }

        // 타겟방향으로 화살 발사
        ArrowController arrow;
        arrow = bowinfo.arrowPool.Get();
        arrow.trailRenderer.enabled = false;
        arrow.arrowRigidbody.isKinematic = true;
        arrow.arrowRigidbody.freezeRotation = false;
        arrow.IsTrigger = false;
        arrow.transform.parent = PlayerInfo.GetInstance.combatInfo.playerInfo.leftWeapon.transform;
        arrow.transform.localPosition = bowinfo.defaultArrowPos;
        arrow.transform.localRotation = Quaternion.Euler(bowinfo.defaultArrowRot);
        PlayerInfo.GetInstance.combatInfo.animator.SetTrigger(bowinfo.aniHashBowZoom);
        bowinfo.bowAni.SetTrigger(bowinfo.aniHashBowString);
        while (!PlayerInfo.GetInstance.combatInfo.animator.GetCurrentAnimatorStateInfo(1).IsName("DrawArrow (BowCombat)"))
        {
            yield return null;
        }

        // 점프
        Vector3 jumpDir = -PlayerInfo.GetInstance.transform.forward * 2f + Vector3.up * 4.5f;
        PlayerInfo.GetInstance.movementInfo.player_rigidbody.AddForce(jumpDir, ForceMode.Impulse);

        yield return new WaitForSeconds(0.25f);

        PlayerInfo.GetInstance.combatInfo.animator.SetTrigger(PlayerInfo.GetInstance.combatInfo.aniHashAttack);
        PlayerInfo.GetInstance.combatInfo.animator.SetFloat(PlayerInfo.GetInstance.combatInfo.aniHashAniMult, 2f);
        while (!PlayerInfo.GetInstance.combatInfo.animator.GetCurrentAnimatorStateInfo(1).IsName("ShotArrow (BowCombat)"))
        {
            yield return null;
        }
        jumpDir = Vector3.up * 2.5f;
        PlayerInfo.GetInstance.movementInfo.player_rigidbody.AddForce(jumpDir, ForceMode.Impulse);

        arrow.transform.parent = arrow.objectPoolParent.transform;
        arrow.trailRenderer.enabled = true;
        arrow.arrowRigidbody.isKinematic = false;
        arrow.arrowRigidbody.freezeRotation = true;
        arrow.arrowRigidbody.useGravity = true;

        // 이동부분
        while (Vector3.Distance(hitPlace + new Vector3(0, 0.25f, 0), arrow.transform.position) > 0.15f)
        {
            Vector3 dir = (hitPlace + new Vector3(0, 0.25f, 0) - arrow.transform.position).normalized;
            arrow.arrowRigidbody.velocity = dir * 15f;
            arrow.dirSet();
            yield return null;
        }

        skillDamage = skillData.skillInfo.skillLV * PlayerInfo.GetInstance.LV * 250.0f;

        DamageManager.GetInstance.CallDamageRange(DamageRangeType.DONUT 
            , false, 0, skillDamage, LayerMask.GetMask("Enemy"), hitPlace, 0, 3.0f);

        yield return null;
        arrow.Remove();
        hitPlace = Vector3.zero;
        PlayerInfo.GetInstance.movementInfo.inputLock = false;
    }

}

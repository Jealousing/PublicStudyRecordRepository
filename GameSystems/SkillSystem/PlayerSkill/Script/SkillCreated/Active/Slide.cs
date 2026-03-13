using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 활 스킬의 한 종류, 회피이동(슬라이드)을 위한 액티브 스킬.
/// </summary>
public class Slide : ActiveSkill
{
    int aniHashSlide;
    public override void Setting()
    {
        aniHashSlide = Animator.StringToHash("Slide");
    }

    public override void CheckRange()
    {
        bool hasValidSkill = skillCooldownTime <= 0 
           && PlayerInfo.GetInstance.combatInfo.IsCombat;

        if (!hasValidSkill) return;

        StartCoroutine(UseActiveSkill());
    }

    public override void ResetRange()
    {
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
        // 링크스킬 호출 및 쿨타임 돌도록 명령
        LinkedSkillEvent?.Invoke(3);
        StartCoroutine(SkillCooldownTimer());

        // 입력제한 및 애니메이션 실행 
        PlayerInfo.GetInstance.movementInfo.inputLock = true;
        PlayerInfo.GetInstance.combatInfo.animator.SetTrigger(aniHashSlide);

        // 키입력 여부 확인 및 해당 방향으로 회전
        Vector2 moveVec = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        float targetRotation = Mathf.Atan2(moveVec.x, moveVec.y) * Mathf.Rad2Deg +
            PlayerInfo.GetInstance.cameraInfo.cameraObj.transform.eulerAngles.y;
        PlayerInfo.GetInstance.transform.rotation = Quaternion.Euler(0.0f, targetRotation, 0.0f);

        // 애니메이션 실행확인
        while (!PlayerInfo.GetInstance.animator.GetCurrentAnimatorStateInfo(1).IsName("Slide"))
        {
            yield return null;
        }

        // 이동초기화 및 보고있는 방향으로 빠르게 이동
        PlayerInfo.GetInstance.movementInfo.player_rigidbody.velocity = Vector3.zero;
        PlayerInfo.GetInstance.movementInfo.player_rigidbody.
            AddForce(PlayerInfo.GetInstance.transform.forward * 12.5f, ForceMode.Impulse);

        // 슬라이드가 끝날때까지 대기
        while (PlayerInfo.GetInstance.animator.GetCurrentAnimatorStateInfo(1).IsName("Sliede") &&
          PlayerInfo.GetInstance.animator.GetCurrentAnimatorStateInfo(1).normalizedTime <= 0.95f)
        {
            yield return null;
        }

        // 후딜 및 속도 초기화
        yield return new WaitForSeconds (0.25f);
        PlayerInfo.GetInstance.movementInfo.inputLock = false;
        PlayerInfo.GetInstance.movementInfo.player_rigidbody.velocity = Vector3.zero;
    }


  

}

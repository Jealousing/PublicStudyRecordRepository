using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ȱ ��ų�� �� ����, ȸ���̵�(�����̵�)�� ���� ��Ƽ�� ��ų.
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
        // ��ũ��ų ȣ�� �� ��Ÿ�� ������ ���
        LinkedSkillEvent?.Invoke(3);
        StartCoroutine(SkillCooldownTimer());

        // �Է����� �� �ִϸ��̼� ���� 
        PlayerInfo.GetInstance.movementInfo.inputLock = true;
        PlayerInfo.GetInstance.combatInfo.animator.SetTrigger(aniHashSlide);

        // Ű�Է� ���� Ȯ�� �� �ش� �������� ȸ��
        Vector2 moveVec = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        float targetRotation = Mathf.Atan2(moveVec.x, moveVec.y) * Mathf.Rad2Deg +
            PlayerInfo.GetInstance.cameraInfo.cameraObj.transform.eulerAngles.y;
        PlayerInfo.GetInstance.transform.rotation = Quaternion.Euler(0.0f, targetRotation, 0.0f);

        // �ִϸ��̼� ����Ȯ��
        while (!PlayerInfo.GetInstance.animator.GetCurrentAnimatorStateInfo(1).IsName("Slide"))
        {
            yield return null;
        }

        // �̵��ʱ�ȭ �� �����ִ� �������� ������ �̵�
        PlayerInfo.GetInstance.movementInfo.player_rigidbody.velocity = Vector3.zero;
        PlayerInfo.GetInstance.movementInfo.player_rigidbody.
            AddForce(PlayerInfo.GetInstance.transform.forward * 12.5f, ForceMode.Impulse);

        // �����̵尡 ���������� ���
        while (PlayerInfo.GetInstance.animator.GetCurrentAnimatorStateInfo(1).IsName("Sliede") &&
          PlayerInfo.GetInstance.animator.GetCurrentAnimatorStateInfo(1).normalizedTime <= 0.95f)
        {
            yield return null;
        }

        // �ĵ� �� �ӵ� �ʱ�ȭ
        yield return new WaitForSeconds (0.25f);
        PlayerInfo.GetInstance.movementInfo.inputLock = false;
        PlayerInfo.GetInstance.movementInfo.player_rigidbody.velocity = Vector3.zero;
    }


  

}

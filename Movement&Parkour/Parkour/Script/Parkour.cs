using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// ���� Ŭ����
/// </summary>
[Serializable]
public class Parkour : MonoBehaviour
{
    // ������ ����
    MovementInfo movementInfo;

    [NonSerialized] public bool IsParkour = false;
    private float parkourTime;
    private float chosenParkourMoveTime;

    /* Climbing */
    [NonSerialized] public bool IsClimb = false;
    private float correctionValue = 0.35f;

    /* Vault */
    [NonSerialized] public bool IsVault = false;
    private float vaultTime= 0.5f;
    private float maxVaultDistance = 1.3f;
    private float minVaultDistance = 0.1f;
    private Vector3 moveStartPosition;
    private Vector3 moveEndPosition;

    /* WallRun */
    [NonSerialized] public bool IsWallRun = false;

    private void Start()
    {
        movementInfo=GetComponent<MovementInfo>();
    }

    #region Vault 
    /// <summary>
    /// ������ �ѱ� �����ϴ� �Լ�
    /// </summary>
    public bool StartVault(float distance, Vector3 VaultEndPos)
    {

        // �޾ƿ� �� ����
        moveStartPosition = this.transform.position;
        
        IsVault = false;

        // ���� �� �ִ� �Ÿ����� Ȯ��
        if ((distance )<= maxVaultDistance)
        {
            if (distance < 0.35f)
                distance *= 2f;

            Vector3 yPos = Vector3.zero;
            yPos.y = VaultEndPos.y;
            moveEndPosition = transform.position + transform.forward * distance * 2.5f + yPos;
            IsParkour = true;
            chosenParkourMoveTime = vaultTime;
            StartCoroutine(InputLockAniStart(movementInfo.aniHashVault, "Vault"));
            StartCoroutine(Vault());
            //cameraAnimator.CrossFade("Vault", 0.1f);
            return true;
        }
        else if (distance >= minVaultDistance)
        {
            Vector3 yPos = Vector3.zero;
            yPos.y = VaultEndPos.y;
            moveEndPosition = transform.position + transform.forward * 0.5f + yPos;
            IsParkour = true;
            chosenParkourMoveTime = vaultTime;
            StartCoroutine(InputLockAniStart(movementInfo.aniHashVault, "Vault"));
            StartCoroutine(Vault());
            return true;
        }
        return false;
    }

    /// <summary>
    /// ������ ���� �ڷ�ƾ�Լ�
    /// </summary>
    private IEnumerator Vault()
    {
        // ����ġ �ʱ�ȭ
        movementInfo.handIK.leftPositionWeight = 0.0f;
        movementInfo.handIK.rightPositionWeight = 0.0f;

        // ���ϸ��̼� ��������ߴ��� Ȯ��
        while (!movementInfo.animator.GetCurrentAnimatorStateInfo(0).IsName("Vault"))
        {
            yield return null;
        }

        // ��IK�� Vault ���·� ����, ����ġ ����, �� ���̼���
        movementInfo.handIK.ChangeState(HandIkState.VAULT);
        movementInfo.handIK.leftPositionWeight = 1.0f;
        movementInfo.handIK.leftHandIKPositionTarget.y = 
            movementInfo.detectObject.detectVault.DetectCollder.bounds.max.y + 0.05f;

        while (IsParkour && parkourTime < 1f)
        {
            yield return null;

            // ���� �� ����
            while (movementInfo.animator.GetCurrentAnimatorStateInfo(0).IsName("Vault") && movementInfo.animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.2f)
            {
                transform.position = new Vector3(transform.position.x, Mathf.Lerp(moveStartPosition.y, moveEndPosition.y, 0.3f), transform.position.z);
                moveStartPosition = transform.position;
                yield return null;
            }

            parkourTime += Time.deltaTime / chosenParkourMoveTime;
            //�̵��κ�
            transform.position = Vector3.Lerp(moveStartPosition, moveEndPosition, parkourTime);

           
            //����
            if (parkourTime >= 1f)
            {
                parkourTime = 0f;

                while (movementInfo.animator.GetCurrentAnimatorStateInfo(0).IsName("Vault") && movementInfo.animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.6f)
                {
                    yield return null;
                }

                // ����ġ �ʱ�ȭ �� HandIK���� ����
                movementInfo.handIK.leftPositionWeight = 0.0f;
                movementInfo.handIK.ChangeState(HandIkState.NORMAL);

                // Ik�÷��׸� ���� �ڿ������� ���� ����
                while (movementInfo.animator.GetCurrentAnimatorStateInfo(0).IsName("Vault") && movementInfo.animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.8f)
                {
                    yield return null;
                }
                movementInfo.footIK.FootIKFlag = true;
                while (movementInfo.animator.GetCurrentAnimatorStateInfo(0).IsName("Vault") && movementInfo.animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f)
                {
                    yield return null;
                }
                movementInfo.footIK.FootIKFlag = false;
            }
        }
        yield return null;
    }
    #endregion

    #region ClimbUp
    /// <summary>
    /// ���� �� ���� �ö� �� �ִ� �� �ö󰡴� ���� ���� �Լ�
    /// </summary>
    public bool StartClimbUp()
    {
        IsClimb = false;
        IsParkour = true;

        StartCoroutine(ClimbUp());
        return true;
    }

    /// <summary>
    /// ClimbUp ���� �ڷ�ƾ �Լ�
    /// </summary>
    private IEnumerator ClimbUp()
    {
        // �߰��Է� ���� �� ������ �� �ʱ�ȭ, �߷¿�������
        movementInfo.inputLock = true;
        movementInfo.moveVec = Vector2.zero;
        movementInfo.player_rigidbody.useGravity = false;

        // �ִϸ��̼� ���� (�Ŵ޸���)
        if (movementInfo.isAnimator)
        {
            movementInfo.animator.SetTrigger(movementInfo.aniHashClimbing);
        }
        // ����Ȯ��
        while (!movementInfo.animator.GetCurrentAnimatorStateInfo(0).IsName("Climbing Start"))
        {
            yield return null;
        }

        // ���� �ö󰡴´ܰ�
        StartCoroutine(ClimbUpCoroutine1(0.5f));

        while (movementInfo.animator.GetCurrentAnimatorStateInfo(0).IsName("Climbing Start")
            && movementInfo.animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.7f)
        {
            yield return null;
        }

        // �ִϸ��̼� ���� (�ö󰡱�)
        if (movementInfo.isAnimator)
        {
            movementInfo.animator.SetTrigger(movementInfo.aniHashClimbingEndUp);
        }
        // ����Ȯ��
        while (!movementInfo.animator.GetCurrentAnimatorStateInfo(0).IsName("Climbing End Up"))
        {
            yield return null;
        }

        // ������������ �̵�
        StartCoroutine(ClimbUpCoroutine2());

        // ����Ȯ��
        while (movementInfo.animator.GetCurrentAnimatorStateInfo(0).IsName("Climbing End Up")
            && movementInfo.animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f)
        {
            yield return null;
        }

        // �߷�����, ��������, �Է����� ����
        movementInfo.player_rigidbody.useGravity = true;
        IsParkour = false;
        movementInfo.inputLock = false;
    }

    /// <summary>
    /// ���� ���̿� ���� �÷��̾� ���� ���� �ڷ�ƾ
    /// </summary>
    private IEnumerator ClimbUpCoroutine1(float UpTime)
    {
        WaitForSeconds time = new WaitForSeconds(0.025f);
        Vector3 myTransPos = transform.position;
        Vector3 tempTransPos = transform.position;
        tempTransPos.y = movementInfo.detectObject.detectClimb.DetectCollder.bounds.max.y - 1.2f;

        Vector3 targetPosition = new Vector3(movementInfo.detectObject.detectClimb.DetectObject.transform.position.x, transform.position.y, movementInfo.detectObject.detectClimb.DetectObject.transform.position.z);

        transform.LookAt(targetPosition);

        for (int i = 0; i < 20; i++)
        {
            transform.position = Vector3.Lerp(myTransPos, tempTransPos, i * 0.05f);
            yield return time;
        }
    }

    /// <summary>
    /// �ö� ��ġ�� �̵��ϴ� �ڷ�ƾ
    /// </summary>
    private IEnumerator ClimbUpCoroutine2()
    {
        Vector3 lerpStartPos = transform.position;
        Vector3 lerpEndPos = Vector3.zero;

        Vector3 yPos = Vector3.zero;
        yPos.y = movementInfo.detectObject.detectClimb.DetectCollder.bounds.max.y - transform.position.y;
        lerpEndPos = transform.position + transform.forward * 0.4f + yPos;

        // �ִϸ��̼� ����� �ִϸ��̼� ���൵�� ���� ����
        while (movementInfo.animator.GetCurrentAnimatorStateInfo(0).IsName("Climbing End Up")
           && movementInfo.animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f)
        {
            transform.position = Vector3.Lerp(lerpStartPos, lerpEndPos,
                movementInfo.animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
            yield return null;
        }
    }

    #endregion

    #region ClimbingMove
    /// <summary>
    /// ���� �� ���� �ö� �� ���� ���� ���� �Ŵ޷��� ����ϰ��ϴ� �Լ�
    /// </summary>
    public bool StartClimb()
    {
        IsClimb = false;
        IsParkour = true;

        // �ִϸ��̼� ���� �� �Է����� , �����ӻ��¸� ClimbingMovement�� ����
        StartCoroutine(InputLockAniStart(movementInfo.aniHashClimbing, "Climbing Start"));
        movementInfo.moveState.ChangeState(MoveState.ClimbingMovement);
        if (movementInfo.isAnimator)
        {
            movementInfo.animator.ResetTrigger(movementInfo.aniHashClimbingEndJump);
        }
        return true;
    }
    #endregion

    /// <summary>
    /// �Է¹��� �ִϸ��̼� ���� ���������� �Է��� �����ϴ� �Լ�
    /// </summary>
    /// <param name="hashCode"> �ִϸ��̼� �ؽ��ڵ� </param>
    /// <param name="aniStateName"> �ִϸ��̼� �̸� </param>
    private IEnumerator InputLockAniStart(int hashCode, string aniStateName)
    {
        // �Է� ���� �� �Է¹��� �����Ӱ� �ʱ�ȭ
        movementInfo.inputLock = true;
        movementInfo.moveVec = Vector2.zero;

        // �ִϸ��̼� ���� �� ���� Ȯ��
        if (movementInfo.isAnimator)
        {
            movementInfo.animator.SetTrigger(hashCode);
        }
        while (!movementInfo.animator.GetCurrentAnimatorStateInfo(0).IsName(aniStateName))
        {
            yield return null;
        }
        while (movementInfo.animator.GetCurrentAnimatorStateInfo(0).IsName(aniStateName) && 
            movementInfo.animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f)
        {
            yield return null;
        }

        // ���� ���� �� �Է� ���� ����
        IsParkour = false;
        movementInfo.inputLock = false;
    }

    /// <summary>
    /// ���鿡 ���� �ִ��� Ȯ�� (�Ӹ� ���̿��� üũ��)
    /// </summary>
    public bool FrontWallCheck()
    {
        return Physics.Raycast(CustomGetPosition(0, 0), this.transform.forward, 0.8f, LayerMask.NameToLayer("Player"));
    }
    
    /// <summary>
    /// �÷��̾��� Y(�Ӹ�) ��ǥ ���ϱ�
    /// </summary>
    public Vector3 CustomGetPosition(float addValueX, float addValueY)
    {
        Vector3 returnVector = this.transform.position;
        returnVector -= this.transform.forward * movementInfo.player_collider.radius;
        returnVector += transform.right * addValueX / 4;
        returnVector.y += movementInfo.player_collider.height + addValueY;
        //�Ӹ����� ���ϱ����� ������
        returnVector.y -= correctionValue;

        return returnVector;
    }

}

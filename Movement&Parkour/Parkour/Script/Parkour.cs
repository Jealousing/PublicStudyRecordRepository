using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 파쿠르 클래스
/// </summary>
[Serializable]
public class Parkour : MonoBehaviour
{
    // 움직임 정보
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
    /// 낮은벽 넘기 시작하는 함수
    /// </summary>
    public bool StartVault(float distance, Vector3 VaultEndPos)
    {

        // 받아온 값 저장
        moveStartPosition = this.transform.position;
        
        IsVault = false;

        // 넘을 수 있는 거리인지 확인
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
    /// 낮은벽 파쿠르 코루틴함수
    /// </summary>
    private IEnumerator Vault()
    {
        // 가중치 초기화
        movementInfo.handIK.leftPositionWeight = 0.0f;
        movementInfo.handIK.rightPositionWeight = 0.0f;

        // 에니메이션 실행시작했는지 확인
        while (!movementInfo.animator.GetCurrentAnimatorStateInfo(0).IsName("Vault"))
        {
            yield return null;
        }

        // 손IK을 Vault 상태로 변경, 가중치 증가, 손 높이설정
        movementInfo.handIK.ChangeState(HandIkState.VAULT);
        movementInfo.handIK.leftPositionWeight = 1.0f;
        movementInfo.handIK.leftHandIKPositionTarget.y = 
            movementInfo.detectObject.detectVault.DetectCollder.bounds.max.y + 0.05f;

        while (IsParkour && parkourTime < 1f)
        {
            yield return null;

            // 높이 선 보정
            while (movementInfo.animator.GetCurrentAnimatorStateInfo(0).IsName("Vault") && movementInfo.animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.2f)
            {
                transform.position = new Vector3(transform.position.x, Mathf.Lerp(moveStartPosition.y, moveEndPosition.y, 0.3f), transform.position.z);
                moveStartPosition = transform.position;
                yield return null;
            }

            parkourTime += Time.deltaTime / chosenParkourMoveTime;
            //이동부분
            transform.position = Vector3.Lerp(moveStartPosition, moveEndPosition, parkourTime);

           
            //종료
            if (parkourTime >= 1f)
            {
                parkourTime = 0f;

                while (movementInfo.animator.GetCurrentAnimatorStateInfo(0).IsName("Vault") && movementInfo.animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.6f)
                {
                    yield return null;
                }

                // 가중치 초기화 및 HandIK상태 변경
                movementInfo.handIK.leftPositionWeight = 0.0f;
                movementInfo.handIK.ChangeState(HandIkState.NORMAL);

                // Ik플레그를 통한 자연스러운 착지 연출
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
    /// 점프 한 번에 올라갈 수 있는 벽 올라가는 파쿠르 시작 함수
    /// </summary>
    public bool StartClimbUp()
    {
        IsClimb = false;
        IsParkour = true;

        StartCoroutine(ClimbUp());
        return true;
    }

    /// <summary>
    /// ClimbUp 파쿠르 코루틴 함수
    /// </summary>
    private IEnumerator ClimbUp()
    {
        // 추가입력 방지 및 움직임 값 초기화, 중력영향제한
        movementInfo.inputLock = true;
        movementInfo.moveVec = Vector2.zero;
        movementInfo.player_rigidbody.useGravity = false;

        // 애니메이션 실행 (매달리기)
        if (movementInfo.isAnimator)
        {
            movementInfo.animator.SetTrigger(movementInfo.aniHashClimbing);
        }
        // 실행확인
        while (!movementInfo.animator.GetCurrentAnimatorStateInfo(0).IsName("Climbing Start"))
        {
            yield return null;
        }

        // 위로 올라가는단계
        StartCoroutine(ClimbUpCoroutine1(0.5f));

        while (movementInfo.animator.GetCurrentAnimatorStateInfo(0).IsName("Climbing Start")
            && movementInfo.animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.7f)
        {
            yield return null;
        }

        // 애니메이션 실행 (올라가기)
        if (movementInfo.isAnimator)
        {
            movementInfo.animator.SetTrigger(movementInfo.aniHashClimbingEndUp);
        }
        // 실행확인
        while (!movementInfo.animator.GetCurrentAnimatorStateInfo(0).IsName("Climbing End Up"))
        {
            yield return null;
        }

        // 도착지점까지 이동
        StartCoroutine(ClimbUpCoroutine2());

        // 종료확인
        while (movementInfo.animator.GetCurrentAnimatorStateInfo(0).IsName("Climbing End Up")
            && movementInfo.animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f)
        {
            yield return null;
        }

        // 중력적용, 파쿠르종료, 입력제한 해제
        movementInfo.player_rigidbody.useGravity = true;
        IsParkour = false;
        movementInfo.inputLock = false;
    }

    /// <summary>
    /// 벽의 높이에 따른 플레이어 높이 조정 코루틴
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
    /// 올라갈 위치로 이동하는 코루틴
    /// </summary>
    private IEnumerator ClimbUpCoroutine2()
    {
        Vector3 lerpStartPos = transform.position;
        Vector3 lerpEndPos = Vector3.zero;

        Vector3 yPos = Vector3.zero;
        yPos.y = movementInfo.detectObject.detectClimb.DetectCollder.bounds.max.y - transform.position.y;
        lerpEndPos = transform.position + transform.forward * 0.4f + yPos;

        // 애니메이션 진행시 애니메이션 진행도에 맞춰 보간
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
    /// 점프 한 번에 올라갈 수 없는 곳은 벽에 매달려서 대기하게하는 함수
    /// </summary>
    public bool StartClimb()
    {
        IsClimb = false;
        IsParkour = true;

        // 애니메이션 실행 및 입력제한 , 움직임상태를 ClimbingMovement로 변경
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
    /// 입력받은 애니메이션 진행 끝날때까지 입력을 제한하는 함수
    /// </summary>
    /// <param name="hashCode"> 애니메이션 해시코드 </param>
    /// <param name="aniStateName"> 애니메이션 이름 </param>
    private IEnumerator InputLockAniStart(int hashCode, string aniStateName)
    {
        // 입력 제한 및 입력받은 움직임값 초기화
        movementInfo.inputLock = true;
        movementInfo.moveVec = Vector2.zero;

        // 애니메이션 실행 및 종료 확인
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

        // 파쿠르 종료 및 입력 제한 해제
        IsParkour = false;
        movementInfo.inputLock = false;
    }

    /// <summary>
    /// 정면에 벽이 있는지 확인 (머리 높이에서 체크함)
    /// </summary>
    public bool FrontWallCheck()
    {
        return Physics.Raycast(CustomGetPosition(0, 0), this.transform.forward, 0.8f, LayerMask.NameToLayer("Player"));
    }
    
    /// <summary>
    /// 플레이어의 Y(머리) 좌표 구하기
    /// </summary>
    public Vector3 CustomGetPosition(float addValueX, float addValueY)
    {
        Vector3 returnVector = this.transform.position;
        returnVector -= this.transform.forward * movementInfo.player_collider.radius;
        returnVector += transform.right * addValueX / 4;
        returnVector.y += movementInfo.player_collider.height + addValueY;
        //머리높이 구하기위한 보정값
        returnVector.y -= correctionValue;

        return returnVector;
    }

}

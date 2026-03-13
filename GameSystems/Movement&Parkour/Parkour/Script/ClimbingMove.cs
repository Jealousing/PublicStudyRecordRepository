using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 벽에 매달려서 움직이는 상태의 클래스
/// </summary>
public class ClimbingMove : MonoBehaviour
{
    // 움직임 정보와 파쿠르정보 참조
    MovementInfo movementInfo;
    Parkour parkour;

    // 이동속도, 오차값, 속도 변경 속도
    private float moveSpeed = 3.0f;
    private float speedOffset = 0.2f;
    private float speedChangeRate = 10.0f;

    // 벽을 확인하기위한 변수
    private RaycastHit wallCheckRayhit;
    // 레이길이
    private float maxDistance = 1.0f;
    // 보정값
    private float correctionValue = 0.35f;


    private void Start()
    {
        movementInfo=GetComponent<MovementInfo>();
        parkour = GetComponent<Parkour>();
    }

    // 상태 진입시 중력 미적용
    public void ClimbingMove_Enter()
    {
        movementInfo.player_rigidbody.useGravity = false;
    }

    // 상태 퇴장시 중력적용
    public void ClimbingMove_Exit()
    {
        movementInfo.player_rigidbody.useGravity = true;
    }

    public void ClimbingMove_FixedUpdate()
    {
        Move();
    }
    public void ClimbingMove_Update()
    {
        ClimbingMovement_KeyboardInput();
    }
    public void ClimbingMove_LateUpdate()
    {

    }


    // input 키보드
    void ClimbingMovement_KeyboardInput()
    {
        if (movementInfo.inputLock)
            return;

        // 행동모션 속도 증가
        if (Input.GetKey(KeyCode.LeftShift))
        {
            movementInfo.animator.SetFloat(movementInfo.aniHashClimbingSpeed, 1.5f);
        }
        else
        {
            movementInfo.animator.SetFloat(movementInfo.aniHashClimbingSpeed, 1.0f);
        }

        // 벽점프
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (parkour.FrontWallCheck())
            {
                if (movementInfo.moveState.GetCurrentMoveState() == MoveState.ClimbingMovement)
                {
                    if (movementInfo.isAnimator)
                    {
                        movementInfo.animator.SetTrigger(movementInfo.aniHashClimbingEndJump);
                        movementInfo.animator.ResetTrigger(movementInfo.aniHashClimbing);
                    }
                    StartCoroutine(ClimbingEndJump());
                }
            }
        }
    }

    // Climbing 모드 움직임 관리
    private void Move()
    {
        float targetSpeed = moveSpeed;
        bool IsMoveing;

        // 입력없으면 속도0
        if (movementInfo.moveVec == Vector2.zero) { targetSpeed = 0.0f; }

        // 목표속도로 감속 또는 가속하는 부분
        if (movementInfo.currentSpeed < targetSpeed - speedOffset || movementInfo.currentSpeed > targetSpeed + speedOffset)
        {
            // 소수점이하 3자리 반올림
            movementInfo.currentSpeed = Mathf.Lerp(movementInfo.currentSpeed, targetSpeed, Time.deltaTime * speedChangeRate / (movementInfo.currentSpeed + speedOffset));
            movementInfo.currentSpeed = Mathf.Round(movementInfo.currentSpeed * 1000f) / 1000f;
        }
        else
        {
            movementInfo.currentSpeed = targetSpeed;
        }

        // 이동 할 수 있는지 확인
        if (IsMoveing = CheckClimbingIsMove())
        {
            // 이동방향 결정 및 이동
            Vector3 inputDirection = transform.up * movementInfo.moveVec.y + transform.right * movementInfo.moveVec.x;
            movementInfo.player_rigidbody.MovePosition(this.gameObject.transform.position + inputDirection.normalized* 1.2f * Time.deltaTime);

            // 애니메이션 설정
            if (movementInfo.isAnimator)
            {
                movementInfo.animator.SetFloat(movementInfo.aniHashHAxis, movementInfo.moveVec.x);
                movementInfo.animator.SetFloat(movementInfo.aniHashVAxis, movementInfo.moveVec.y);
                movementInfo.animator.SetFloat(movementInfo.aniHashSpeed, movementInfo.currentSpeed);
            }
        }
        else
        {
            if (movementInfo.isAnimator)
            {
                movementInfo.animator.SetFloat(movementInfo.aniHashHAxis, 0);
                movementInfo.animator.SetFloat(movementInfo.aniHashVAxis, 0);
                movementInfo.animator.SetFloat(movementInfo.aniHashSpeed, 0);
            }
        }

        // 벽 확인
        Physics.Raycast(parkour.CustomGetPosition(0, 0), this.transform.forward, out wallCheckRayhit, maxDistance, LayerMask.NameToLayer("Player"));

        // 입력있고 움직일 수 있으면 실행
        if (movementInfo.moveVec != Vector2.zero && IsMoveing)
        {
            // 벽이 있으면 벽을 바라보고 벽에 밀착
            if (wallCheckRayhit.collider != null)
            {
                // 보정된 좌표 수정하기
                Vector3 vec = wallCheckRayhit.point;
                vec.y -= movementInfo.player_collider.height;
                vec.y += correctionValue;

                Vector3 dir = vec - this.transform.position;
                
                //방향 조정
                this.transform.rotation = Quaternion.LookRotation(-wallCheckRayhit.normal);

                // 붙어있는 거리 조정
                if ( 0.35f < Vector3.Distance(vec, this.transform.position) || Vector3.Distance(vec, this.transform.position) <0.23f)
                {
                    movementInfo.player_rigidbody.MovePosition(vec - dir.normalized * 0.32f);
                }
            }
            else
            {
                if (movementInfo.isAnimator)
                {
                    movementInfo.animator.SetTrigger(movementInfo.aniHashClimbingEndUp);
                }
               movementInfo.moveState.ChangeState(MoveState.DefaultMovement);
            }
        }
    }

    // Climbing 진행가능 여부 체크 및 종료
    private bool CheckClimbingIsMove()
    {
        bool returnValue;

        // 좌우 확인
        Debug.DrawRay(parkour.CustomGetPosition(movementInfo.moveVec.x, 0), this.transform.forward * maxDistance, Color.magenta);
        if (Physics.Raycast(parkour.CustomGetPosition(movementInfo.moveVec.x, 0), transform.forward, maxDistance, LayerMask.NameToLayer("Player")))
        {
            returnValue = true;
        }
        else
        {
            movementInfo.moveVec.x = 0;
            returnValue = false;
        }

        // 위아래 확인
        if (movementInfo.moveVec.y != 0)
        {
            Debug.DrawRay(parkour.CustomGetPosition(0, movementInfo.moveVec.y * correctionValue / 2), this.transform.forward * maxDistance, Color.magenta);
            if (!Physics.Raycast(parkour.CustomGetPosition(0, movementInfo.moveVec.y * correctionValue / 2), transform.forward, maxDistance, LayerMask.NameToLayer("Player")))
            {
                if (movementInfo.moveVec.y == 1)
                {
                    StartCoroutine(ClimbingEndup());
                }
                movementInfo.moveVec.y = 0;
            }

            movementInfo.GroundedCheck();

            if (movementInfo.isGround)
            {
                if (movementInfo.moveVec.y == -1)
                {
                    StartCoroutine(ClimbingEndDown(movementInfo.aniHashClimbingEndDown, "Climbing End Down"));
                    movementInfo.moveVec.y = 0;
                }
            }
        }

        return returnValue;
    }

    // 위로 올라가면서 종료
    private IEnumerator ClimbingEndup()
    {
        movementInfo.inputLock = true;
        movementInfo.moveVec = Vector2.zero;

       // 애니메이션 실행 및 HandIK 가중치, 위치 설정
        if (movementInfo.isAnimator)
        {
            movementInfo.animator.SetTrigger(movementInfo.aniHashClimbingEndUp);
        }
        while (!movementInfo.animator.GetCurrentAnimatorStateInfo(0).IsName("Climbing End Up"))
        {
            yield return null;
        }
        movementInfo.handIK.leftPositionWeight = 0.25f;
        movementInfo.handIK.leftHandIKPositionTarget = movementInfo.handIK.leftHand.transform.position;
        movementInfo.handIK.rightPositionWeight = 0.25f;
        movementInfo.handIK.leftHandIKPositionTarget = movementInfo.handIK.rightHand.transform.position;

        // 종료 위치 설정
        Vector3 startPos = transform.position;
        Vector3 endPos = Vector3.zero;
        if(movementInfo.detectObject.detectClimb.IsDetect)
        {
            Vector3 yPos = Vector3.zero;
            if (movementInfo.detectObject.detectClimb.IsDetect)
            {
                yPos.y = movementInfo.detectObject.detectClimb.DetectCollder.bounds.max.y -transform.position.y;
            }
            endPos = transform.position + transform.forward * 0.4f + yPos;
        }
       
        // 애니메이션 진행중 진행 상황에 맞춰 보간이동
        while (movementInfo.animator.GetCurrentAnimatorStateInfo(0).IsName("Climbing End Up") &&
            movementInfo.animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f)
        {
            transform.position = Vector3.Lerp(startPos, endPos,
                movementInfo.animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
            yield return null;
        }

        // 애니메이션 종료 후 변수들 초기화 및 움직임 상태 변경
        movementInfo.handIK.leftPositionWeight = 0f;
        movementInfo.handIK.rightPositionWeight = 0f;
        parkour.IsParkour = false;
        movementInfo.inputLock = false;
        movementInfo.moveState.ChangeState(MoveState.DefaultMovement);
    }

    // 아래로 내려가면서 종료
    private IEnumerator ClimbingEndDown(int hashCode, string aniStateName)
    {
        // 입력 제한 및 입력백터 초기화
        movementInfo.inputLock = true;
        movementInfo.moveVec = Vector2.zero;

        //애니메이션 실행
        if (movementInfo.isAnimator)
        {
            movementInfo.animator.SetTrigger(hashCode);
            yield return null;
        }
        while (!movementInfo.animator.GetCurrentAnimatorStateInfo(0).IsName(aniStateName))
        {
            yield return null;
        }
        while (movementInfo.animator.GetCurrentAnimatorStateInfo(0).IsName(aniStateName) &&
            movementInfo.animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.15f)
        {
            yield return null;
        }

        // 움직임 상태 변경 -> 기본이동으로 변경
        movementInfo.moveState.ChangeState(MoveState.DefaultMovement);
        
        // 종료이후 입력제한 해제
        while (movementInfo.animator.GetCurrentAnimatorStateInfo(0).IsName(aniStateName) &&
            movementInfo.animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f)
        {
            yield return null;
        }
        parkour.IsParkour = false;
        movementInfo.inputLock = false;
    }

    // 점프하면서 종료
    private IEnumerator ClimbingEndJump()
    {
        // 낙하타임 재설정
        movementInfo.fallTimeoutDelta = movementInfo.fallTimeout;
        movementInfo.fallTime = 0.0f;

        // 입력 제한 및 입력벡터 초기화
        movementInfo.inputLock = true;
        movementInfo.moveVec = Vector2.zero;

        // 시작 방향과 종료 방향 설정
        Vector3 startRotate = this.transform.forward;
        Vector3 endRotate = -this.transform.forward + transform.right*0.05f;

        // 애니메이션 루트 모션사용
        movementInfo.animator.applyRootMotion = true; 
        
        if (movementInfo.isAnimator)
        {
            movementInfo.animator.SetBool(movementInfo.aniHashGrounded, false);
            movementInfo.animator.SetFloat(movementInfo.aniHashFallTime, movementInfo.fallTime);
        }

        while (!movementInfo.animator.GetCurrentAnimatorStateInfo(0).IsName("Climbing End Jump"))
        {
            yield return null;
        }

        // 진행상황에 맞춰 회전
        while (movementInfo.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.9f)
        {
            transform.rotation = Quaternion.Lerp(Quaternion.LookRotation(startRotate), Quaternion.LookRotation(endRotate),
                movementInfo.animator.GetCurrentAnimatorStateInfo(0).normalizedTime-0.1f);

            yield return null;
        }

        while (movementInfo.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            // 낙하적용시간
            if (movementInfo.fallTimeoutDelta >= 0.0f)
            {
                movementInfo.fallTimeoutDelta -= Time.deltaTime;
            }
            else
            {
                movementInfo.fallTime += Time.deltaTime;
            }
            yield return null;
        }

        // 입력제한 해제 및 움직임 상태 변경
        movementInfo.inputLock = false;
        movementInfo.moveState.ChangeState(MoveState.DefaultMovement);

        // 낙하적용시간 추가
        if (movementInfo.fallTimeoutDelta >= 0.0f)
        {
            movementInfo.fallTimeoutDelta -= Time.deltaTime;
        }
        else
        {
            movementInfo.animator.SetBool(movementInfo.aniHashFall, true);
            movementInfo.fallTime += Time.deltaTime;
            if (movementInfo.isAnimator)
            {
                movementInfo.animator.SetFloat(movementInfo.aniHashFallTime, movementInfo.fallTime);
            }
        }
        // 애니메이션 루트모션 off
        yield return null;
        movementInfo.animator.applyRootMotion = false;
    }

}

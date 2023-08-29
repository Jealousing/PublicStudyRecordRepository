using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 기본 움직임 클래스
/// </summary>
public class DefaultMove : MonoBehaviour
{
    // 움직임 정보와 파쿠르정보 참조
    MovementInfo movementInfo;
    Parkour parkourInfo;

    [Header("Movement")]
    [Tooltip("기본 이동속도")] public float moveSpeed = 3.0f;
    [Tooltip("달리기 속도")] public float sprintSpeed = 6.0f;
    [Tooltip("오차값")] public float speedOffset = 0.2f;
    [Tooltip("속도 변경 속도")] public float speedChangeRate = 10.0f;
    private bool isRun;

    [Header("Jump")]
    public float jumpTimeout = 0.3f;            // 재점프시간
    private float jumpTimeoutDelta;
    public float jumpPower = 4.5f;
    private bool IsJump;

    [Header("Rotation")]
    [Range(0.0f, 0.3f)]
    public float rotationSmoothTime = 0.12f;
    private float targetRotation = 0.0f;
    private float rotationVelocity;

    private void Start()
    {
        movementInfo = GetComponent<MovementInfo>();
        parkourInfo = GetComponent<Parkour>();
        jumpTimeoutDelta = jumpTimeout;
    }

    public void DefaultMove_Enter()
    {

    }

    public void DefaultMove_FixedUpdate()
    {
        if (LandingCheck()) return;
        Move();
    }
    public void DefaultMove_Update()
    {
        KeyInput();
        movementInfo.GroundedCheck();
        JumpAndFall();
    }

    public void DefaultMove_LateUpdate()
    {

    }

    public void DefaultMove_Exit()
    {

    }


    // 착지 관리 함수
    private bool LandingCheck()
    {
        if (!movementInfo.isGround) return false;

        if (movementInfo.animator.GetCurrentAnimatorStateInfo(0).IsTag(movementInfo.LandingTag))
        {
            // 착지중에 움직임 방지(구르기만)
            if (movementInfo.animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == movementInfo.LandingTag)
            {
                movementInfo.animator.SetFloat(movementInfo.aniHashSpeed, 0);
                movementInfo.currentSpeed = 0;
                movementInfo.player_rigidbody.velocity = Vector3.zero;
                movementInfo.fallTime = 0;
                if (movementInfo.isAnimator)
                {
                    movementInfo.animator.SetFloat(movementInfo.aniHashFallTime, movementInfo.fallTime);
                }
                return true;
            }

            //나머지
            if (movementInfo.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.1f)
            {
                movementInfo.fallTime = 0;
                if (movementInfo.isAnimator)
                {
                    movementInfo.animator.SetFloat(movementInfo.aniHashFallTime, movementInfo.fallTime);
                }
                return false;
            }
            else { }
        }
        return false;
    }

    // 움직임을 관리하는 함수
    private void Move()
    {
        // 달리고 있는지 확인
        float targetSpeed = isRun ? sprintSpeed : moveSpeed;

        // 입력없으면 속도0
        if (movementInfo.moveVec == Vector2.zero) { targetSpeed = 0.0f; }

        // 목표속도로 감속 또는 가속하는 부분
        if (movementInfo.currentSpeed < targetSpeed - speedOffset || movementInfo.currentSpeed > targetSpeed + speedOffset)
        {
            movementInfo.currentSpeed = Mathf.Lerp(movementInfo.currentSpeed, targetSpeed, Time.deltaTime * speedChangeRate / (movementInfo.currentSpeed + speedOffset));
            // 소수점이하 3자리 반올림
            movementInfo.currentSpeed = Mathf.Round(movementInfo.currentSpeed * 1000f) / 1000f;
        }
        else
        {
            movementInfo.currentSpeed = targetSpeed;
        }

        // 이동방향
        Vector3 inputDirection = new Vector3(movementInfo.moveVec.x, 0.0f, movementInfo.moveVec.y).normalized;

        // 이동중에만 회전
        if (movementInfo.moveVec != Vector2.zero && !movementInfo.inputLock)
        {
            targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + movementInfo.mainCamera.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity, rotationSmoothTime);

            // 캐릭터를 입력 방향으로 회전
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }

        // 이동방향 및 이동처리
        Vector3 targetDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;
        movementInfo.player_rigidbody.MovePosition(this.gameObject.transform.position + targetDirection * movementInfo.currentSpeed * Time.deltaTime);
        if (movementInfo.isAnimator)
        {
            movementInfo.animator.SetFloat(movementInfo.aniHashSpeed, movementInfo.currentSpeed);
        }
    }

    // 키입력
    private void KeyInput()
    {
        // 락걸려있으면 진행x
        if (movementInfo.inputLock)
            return;

        // 달리기
        if (Input.GetKey(KeyCode.LeftShift) && movementInfo.currentSpeed >= moveSpeed)
        {
            isRun = true;
        }
        else
        {
            isRun = false;
        }

        // 파쿠르 및 점프
        if (Input.GetKey(KeyCode.Space))
        {
            //moveVec = Vector2.zero;
            if (ParkourSystem())
            {

            }
            else
            {
                if (movementInfo.isGround&&!IsJump)
                    IsJump = true;
            }
        }
        
        // 대쉬 예정?
        if (Input.GetKeyDown(KeyCode.H))
        {
            //StartCoroutine(DashCoroutine());
        }
    }

    // 점프 및 낙하 관리
    private void JumpAndFall()
    {
        if (movementInfo.isGround)
        {
            // 낙하타임 재설정
            movementInfo.fallTimeoutDelta = movementInfo.fallTimeout;
            // 점프, 낙하 false
            if (movementInfo.isAnimator)
            {
                movementInfo.animator.SetBool(movementInfo.aniHashJump, false);
                movementInfo.animator.SetBool(movementInfo.aniHashFall, false);
                movementInfo.animator.SetFloat(movementInfo.aniHashFallTime, movementInfo.fallTime);
            }

            // 점프입력
            if (IsJump && jumpTimeoutDelta <= 0.0f && !movementInfo.animator.GetCurrentAnimatorStateInfo(0).IsTag("Landing"))
            {
                jumpTimeoutDelta = jumpTimeout;
                IsJump = false;

                movementInfo.player_rigidbody.AddForce(Vector3.up* jumpPower, ForceMode.Impulse);
                movementInfo.isGround = false;
                if (movementInfo.isAnimator)
                {
                    movementInfo.animator.SetBool(movementInfo.aniHashJump, true);
                }
            }

            // 재점프시간
            if (jumpTimeoutDelta >= 0.0f)
            {
                jumpTimeoutDelta -= Time.deltaTime;
            }

        }
        else
        {
            // 재점프시간 재설정
            jumpTimeoutDelta = jumpTimeout;

            // 낙하적용시간
            if (movementInfo.fallTimeoutDelta >= 0.0f)
            {
                movementInfo.fallTimeoutDelta -= Time.deltaTime;
            }
            else
            {
                if (movementInfo.isAnimator)
                {
                    movementInfo.animator.SetBool(movementInfo.aniHashFall, true);
                    movementInfo.fallTime += Time.deltaTime;

                    if (movementInfo.isAnimator)
                    {
                        movementInfo.animator.SetFloat(movementInfo.aniHashFallTime, movementInfo.fallTime);
                    }
                }
            }
            // 재점프 방지
            IsJump = false;
        }
    }

    // 파쿠르 시스템
    private bool ParkourSystem()
    {
        // vault 낮은벽 넘기
        if (movementInfo.detectObject.detectVault.IsDetect && !movementInfo.detectObject.detectVaultLimit.IsDetect 
            && !parkourInfo.IsParkour   && movementInfo.animator.GetCurrentAnimatorStateInfo(0).IsName("MoveBlend")
            && Input.GetAxisRaw("Vertical") != 0f)
        {
            parkourInfo.IsVault = true;
        }

        if (parkourInfo.IsVault)
        {
            Vector3 detectObjCenterPos = movementInfo.detectObject.detectVault.DetectCollder.bounds.center;
            Vector3 VaultEndPos = detectObjCenterPos + (detectObjCenterPos - this.transform.position);
            float distance;

            if (( distance = GetVaultDistance(Vector3.Distance(this.transform.position, detectObjCenterPos))) != 0)
            {
                if (parkourInfo.StartVault(distance, VaultEndPos))
                {
                    return true;
                }
            }
            parkourInfo.IsVault = false;
        }
        
        // Climbing 높은벽타기
        if (movementInfo.detectObject.detectClimb.IsDetect && !parkourInfo.IsParkour &&
            parkourInfo.FrontWallCheck() &&
           movementInfo.animator.GetCurrentAnimatorStateInfo(0).IsName("MoveBlend"))
        {
            parkourInfo.IsClimb = true;
        }

        if (parkourInfo.IsClimb)
        {
            // 매달린후 올라가기
            if (!movementInfo.detectObject.detectClimbLimit.IsDetect)
            {
                if(parkourInfo.StartClimbUp())
                {
                    return true;
                }
            }
          
            //그냥 매달리기
            if (movementInfo.moveState.GetCurrentMoveState() != MoveState.ClimbingMovement)
            {
                if (parkourInfo.StartClimb())
                {
                    return true;
                }
                //cameraAnimator.CrossFade("Climb", 0.1f);
            }
        }
        
        // WallRun 벽 달리기
        if ((movementInfo.detectObject.detectWallL.IsDetect || movementInfo.detectObject.detectWallR.IsDetect) && !parkourInfo.IsParkour && !parkourInfo.IsWallRun
            && !movementInfo.detectObject.detectClimb.IsDetect && !movementInfo.detectObject.detectVault.IsDetect)
        {
            parkourInfo.IsWallRun = true;
        }

        if (parkourInfo.IsWallRun)
        {
            parkourInfo.IsParkour = true;
            parkourInfo.IsWallRun = false;
            movementInfo.moveState.ChangeState(MoveState.WallRunMovement);
            return true;
        }
        return false;
    }

    private float GetVaultDistance(float distance)
    {
        RaycastHit forwardRay;
        RaycastHit backwardRay;

        Vector3 heightCorrection = new Vector3(0, 0.2f, 0);
        float distanceCorrection = 0.5f;

        if (Physics.Raycast(transform.position + (-transform.forward * distanceCorrection) + heightCorrection, transform.forward, out forwardRay, distance + distanceCorrection, LayerMask.NameToLayer("Player")))
        {

            for (int i = 0; i < 25; i++)
            {
                if (Physics.Raycast(transform.position + transform.forward * distance * 0.1f * i + heightCorrection, -transform.forward, out backwardRay, distance + distanceCorrection, LayerMask.NameToLayer("Player")))
                {
                    if (forwardRay.transform.gameObject == backwardRay.transform.gameObject)
                    {
                        return Vector3.Distance(forwardRay.point, backwardRay.point);
                    }
                }
            }
        }

        return 0;
    }
}

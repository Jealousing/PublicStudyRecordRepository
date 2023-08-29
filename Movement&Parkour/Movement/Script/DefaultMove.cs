using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// �⺻ ������ Ŭ����
/// </summary>
public class DefaultMove : MonoBehaviour
{
    // ������ ������ �������� ����
    MovementInfo movementInfo;
    Parkour parkourInfo;

    [Header("Movement")]
    [Tooltip("�⺻ �̵��ӵ�")] public float moveSpeed = 3.0f;
    [Tooltip("�޸��� �ӵ�")] public float sprintSpeed = 6.0f;
    [Tooltip("������")] public float speedOffset = 0.2f;
    [Tooltip("�ӵ� ���� �ӵ�")] public float speedChangeRate = 10.0f;
    private bool isRun;

    [Header("Jump")]
    public float jumpTimeout = 0.3f;            // �������ð�
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


    // ���� ���� �Լ�
    private bool LandingCheck()
    {
        if (!movementInfo.isGround) return false;

        if (movementInfo.animator.GetCurrentAnimatorStateInfo(0).IsTag(movementInfo.LandingTag))
        {
            // �����߿� ������ ����(�����⸸)
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

            //������
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

    // �������� �����ϴ� �Լ�
    private void Move()
    {
        // �޸��� �ִ��� Ȯ��
        float targetSpeed = isRun ? sprintSpeed : moveSpeed;

        // �Է¾����� �ӵ�0
        if (movementInfo.moveVec == Vector2.zero) { targetSpeed = 0.0f; }

        // ��ǥ�ӵ��� ���� �Ǵ� �����ϴ� �κ�
        if (movementInfo.currentSpeed < targetSpeed - speedOffset || movementInfo.currentSpeed > targetSpeed + speedOffset)
        {
            movementInfo.currentSpeed = Mathf.Lerp(movementInfo.currentSpeed, targetSpeed, Time.deltaTime * speedChangeRate / (movementInfo.currentSpeed + speedOffset));
            // �Ҽ������� 3�ڸ� �ݿø�
            movementInfo.currentSpeed = Mathf.Round(movementInfo.currentSpeed * 1000f) / 1000f;
        }
        else
        {
            movementInfo.currentSpeed = targetSpeed;
        }

        // �̵�����
        Vector3 inputDirection = new Vector3(movementInfo.moveVec.x, 0.0f, movementInfo.moveVec.y).normalized;

        // �̵��߿��� ȸ��
        if (movementInfo.moveVec != Vector2.zero && !movementInfo.inputLock)
        {
            targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + movementInfo.mainCamera.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity, rotationSmoothTime);

            // ĳ���͸� �Է� �������� ȸ��
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }

        // �̵����� �� �̵�ó��
        Vector3 targetDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;
        movementInfo.player_rigidbody.MovePosition(this.gameObject.transform.position + targetDirection * movementInfo.currentSpeed * Time.deltaTime);
        if (movementInfo.isAnimator)
        {
            movementInfo.animator.SetFloat(movementInfo.aniHashSpeed, movementInfo.currentSpeed);
        }
    }

    // Ű�Է�
    private void KeyInput()
    {
        // ���ɷ������� ����x
        if (movementInfo.inputLock)
            return;

        // �޸���
        if (Input.GetKey(KeyCode.LeftShift) && movementInfo.currentSpeed >= moveSpeed)
        {
            isRun = true;
        }
        else
        {
            isRun = false;
        }

        // ���� �� ����
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
        
        // �뽬 ����?
        if (Input.GetKeyDown(KeyCode.H))
        {
            //StartCoroutine(DashCoroutine());
        }
    }

    // ���� �� ���� ����
    private void JumpAndFall()
    {
        if (movementInfo.isGround)
        {
            // ����Ÿ�� �缳��
            movementInfo.fallTimeoutDelta = movementInfo.fallTimeout;
            // ����, ���� false
            if (movementInfo.isAnimator)
            {
                movementInfo.animator.SetBool(movementInfo.aniHashJump, false);
                movementInfo.animator.SetBool(movementInfo.aniHashFall, false);
                movementInfo.animator.SetFloat(movementInfo.aniHashFallTime, movementInfo.fallTime);
            }

            // �����Է�
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

            // �������ð�
            if (jumpTimeoutDelta >= 0.0f)
            {
                jumpTimeoutDelta -= Time.deltaTime;
            }

        }
        else
        {
            // �������ð� �缳��
            jumpTimeoutDelta = jumpTimeout;

            // ��������ð�
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
            // ������ ����
            IsJump = false;
        }
    }

    // ���� �ý���
    private bool ParkourSystem()
    {
        // vault ������ �ѱ�
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
        
        // Climbing ������Ÿ��
        if (movementInfo.detectObject.detectClimb.IsDetect && !parkourInfo.IsParkour &&
            parkourInfo.FrontWallCheck() &&
           movementInfo.animator.GetCurrentAnimatorStateInfo(0).IsName("MoveBlend"))
        {
            parkourInfo.IsClimb = true;
        }

        if (parkourInfo.IsClimb)
        {
            // �Ŵ޸��� �ö󰡱�
            if (!movementInfo.detectObject.detectClimbLimit.IsDetect)
            {
                if(parkourInfo.StartClimbUp())
                {
                    return true;
                }
            }
          
            //�׳� �Ŵ޸���
            if (movementInfo.moveState.GetCurrentMoveState() != MoveState.ClimbingMovement)
            {
                if (parkourInfo.StartClimb())
                {
                    return true;
                }
                //cameraAnimator.CrossFade("Climb", 0.1f);
            }
        }
        
        // WallRun �� �޸���
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

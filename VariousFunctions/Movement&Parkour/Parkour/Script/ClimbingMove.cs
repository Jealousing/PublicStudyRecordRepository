using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� �Ŵ޷��� �����̴� ������ Ŭ����
/// </summary>
public class ClimbingMove : MonoBehaviour
{
    // ������ ������ �������� ����
    MovementInfo movementInfo;
    Parkour parkour;

    // �̵��ӵ�, ������, �ӵ� ���� �ӵ�
    private float moveSpeed = 3.0f;
    private float speedOffset = 0.2f;
    private float speedChangeRate = 10.0f;

    // ���� Ȯ���ϱ����� ����
    private RaycastHit wallCheckRayhit;
    // ���̱���
    private float maxDistance = 1.0f;
    // ������
    private float correctionValue = 0.35f;


    private void Start()
    {
        movementInfo=GetComponent<MovementInfo>();
        parkour = GetComponent<Parkour>();
    }

    // ���� ���Խ� �߷� ������
    public void ClimbingMove_Enter()
    {
        movementInfo.player_rigidbody.useGravity = false;
    }

    // ���� ����� �߷�����
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


    // input Ű����
    void ClimbingMovement_KeyboardInput()
    {
        if (movementInfo.inputLock)
            return;

        // �ൿ��� �ӵ� ����
        if (Input.GetKey(KeyCode.LeftShift))
        {
            movementInfo.animator.SetFloat(movementInfo.aniHashClimbingSpeed, 1.5f);
        }
        else
        {
            movementInfo.animator.SetFloat(movementInfo.aniHashClimbingSpeed, 1.0f);
        }

        // ������
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

    // Climbing ��� ������ ����
    private void Move()
    {
        float targetSpeed = moveSpeed;
        bool IsMoveing;

        // �Է¾����� �ӵ�0
        if (movementInfo.moveVec == Vector2.zero) { targetSpeed = 0.0f; }

        // ��ǥ�ӵ��� ���� �Ǵ� �����ϴ� �κ�
        if (movementInfo.currentSpeed < targetSpeed - speedOffset || movementInfo.currentSpeed > targetSpeed + speedOffset)
        {
            // �Ҽ������� 3�ڸ� �ݿø�
            movementInfo.currentSpeed = Mathf.Lerp(movementInfo.currentSpeed, targetSpeed, Time.deltaTime * speedChangeRate / (movementInfo.currentSpeed + speedOffset));
            movementInfo.currentSpeed = Mathf.Round(movementInfo.currentSpeed * 1000f) / 1000f;
        }
        else
        {
            movementInfo.currentSpeed = targetSpeed;
        }

        // �̵� �� �� �ִ��� Ȯ��
        if (IsMoveing = CheckClimbingIsMove())
        {
            // �̵����� ���� �� �̵�
            Vector3 inputDirection = transform.up * movementInfo.moveVec.y + transform.right * movementInfo.moveVec.x;
            movementInfo.player_rigidbody.MovePosition(this.gameObject.transform.position + inputDirection.normalized* 1.2f * Time.deltaTime);

            // �ִϸ��̼� ����
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

        // �� Ȯ��
        Physics.Raycast(parkour.CustomGetPosition(0, 0), this.transform.forward, out wallCheckRayhit, maxDistance, LayerMask.NameToLayer("Player"));

        // �Է��ְ� ������ �� ������ ����
        if (movementInfo.moveVec != Vector2.zero && IsMoveing)
        {
            // ���� ������ ���� �ٶ󺸰� ���� ����
            if (wallCheckRayhit.collider != null)
            {
                // ������ ��ǥ �����ϱ�
                Vector3 vec = wallCheckRayhit.point;
                vec.y -= movementInfo.player_collider.height;
                vec.y += correctionValue;

                Vector3 dir = vec - this.transform.position;
                
                //���� ����
                this.transform.rotation = Quaternion.LookRotation(-wallCheckRayhit.normal);

                // �پ��ִ� �Ÿ� ����
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

    // Climbing ���డ�� ���� üũ �� ����
    private bool CheckClimbingIsMove()
    {
        bool returnValue;

        // �¿� Ȯ��
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

        // ���Ʒ� Ȯ��
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

    // ���� �ö󰡸鼭 ����
    private IEnumerator ClimbingEndup()
    {
        movementInfo.inputLock = true;
        movementInfo.moveVec = Vector2.zero;

       // �ִϸ��̼� ���� �� HandIK ����ġ, ��ġ ����
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

        // ���� ��ġ ����
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
       
        // �ִϸ��̼� ������ ���� ��Ȳ�� ���� �����̵�
        while (movementInfo.animator.GetCurrentAnimatorStateInfo(0).IsName("Climbing End Up") &&
            movementInfo.animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f)
        {
            transform.position = Vector3.Lerp(startPos, endPos,
                movementInfo.animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
            yield return null;
        }

        // �ִϸ��̼� ���� �� ������ �ʱ�ȭ �� ������ ���� ����
        movementInfo.handIK.leftPositionWeight = 0f;
        movementInfo.handIK.rightPositionWeight = 0f;
        parkour.IsParkour = false;
        movementInfo.inputLock = false;
        movementInfo.moveState.ChangeState(MoveState.DefaultMovement);
    }

    // �Ʒ��� �������鼭 ����
    private IEnumerator ClimbingEndDown(int hashCode, string aniStateName)
    {
        // �Է� ���� �� �Է¹��� �ʱ�ȭ
        movementInfo.inputLock = true;
        movementInfo.moveVec = Vector2.zero;

        //�ִϸ��̼� ����
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

        // ������ ���� ���� -> �⺻�̵����� ����
        movementInfo.moveState.ChangeState(MoveState.DefaultMovement);
        
        // �������� �Է����� ����
        while (movementInfo.animator.GetCurrentAnimatorStateInfo(0).IsName(aniStateName) &&
            movementInfo.animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f)
        {
            yield return null;
        }
        parkour.IsParkour = false;
        movementInfo.inputLock = false;
    }

    // �����ϸ鼭 ����
    private IEnumerator ClimbingEndJump()
    {
        // ����Ÿ�� �缳��
        movementInfo.fallTimeoutDelta = movementInfo.fallTimeout;
        movementInfo.fallTime = 0.0f;

        // �Է� ���� �� �Էº��� �ʱ�ȭ
        movementInfo.inputLock = true;
        movementInfo.moveVec = Vector2.zero;

        // ���� ����� ���� ���� ����
        Vector3 startRotate = this.transform.forward;
        Vector3 endRotate = -this.transform.forward + transform.right*0.05f;

        // �ִϸ��̼� ��Ʈ ��ǻ��
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

        // �����Ȳ�� ���� ȸ��
        while (movementInfo.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.9f)
        {
            transform.rotation = Quaternion.Lerp(Quaternion.LookRotation(startRotate), Quaternion.LookRotation(endRotate),
                movementInfo.animator.GetCurrentAnimatorStateInfo(0).normalizedTime-0.1f);

            yield return null;
        }

        while (movementInfo.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            // ��������ð�
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

        // �Է����� ���� �� ������ ���� ����
        movementInfo.inputLock = false;
        movementInfo.moveState.ChangeState(MoveState.DefaultMovement);

        // ��������ð� �߰�
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
        // �ִϸ��̼� ��Ʈ��� off
        yield return null;
        movementInfo.animator.applyRootMotion = false;
    }

}

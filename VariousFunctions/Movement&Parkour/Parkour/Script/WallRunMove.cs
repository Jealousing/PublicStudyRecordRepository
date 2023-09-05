using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� Ÿ�� �޸��� �����̴� ������ Ŭ����
/// </summary>
public class WallRunMove : MonoBehaviour
{
    // ������ ������ �������� ����
    MovementInfo movementInfo;
    Parkour parkour;

    private float wallRunSpeed = 5.0f;
    private float wallCheckDistance = 2f;
    private float speedChangeRate = 10.0f;
    private float speedOffset = 0.2f;

    // ���� �ִ��� Ȯ��
    private bool wallLeft;
    private bool wallRight;

    [Tooltip("Ż �� �ִ� ���� ���̾�")] public LayerMask wallLayerMask;
    private RaycastHit leftWallhit;
    private RaycastHit rightWallhit;

    // ó�� ���� Ÿ�� ������ �� �� �� ���� �ö� ������.
    private float wallRunTime;
    private float wallRunUpTime = 3.0f;

    // ó�� ���ʴ� ���� ������� �޸� �� �ֵ��� �����ϴ� ����
    private float exceptionTimer = 0.0f;
    private float setTime = 1.5f;
    private bool exceptionFlag = false;

    // ���� ���� ���⺤��
    private Vector3 previousDirVec;

    string aniTag = "WallRunTag";

    private void Start()
    {
        movementInfo = GetComponent<MovementInfo>();
        parkour = GetComponent<Parkour>();
    }


    public void WallRunMovement_Enter()
    {
        // �ʱ�ȭ �� �߷����� x
        exceptionFlag = false;
        exceptionTimer = 0.0f;
        previousDirVec = this.transform.forward;
        movementInfo.player_rigidbody.useGravity = false;
        movementInfo.player_rigidbody.velocity = Vector3.zero;

        // �ִϸ��̼� ����
        StartCoroutine(WallRunStart());
    }

    public void WallRunMovement_FixedUpdate()
    {
        Move();
    }

    public void WallRunMovement_Update()
    {

        // ���� ó���� ���ѹ��
        if(!exceptionFlag && exceptionTimer <= setTime)
        {
            exceptionTimer += Time.deltaTime;
        }
        else
        {
            exceptionFlag = true;
        }

        // ���� �ð������� ��Ȯ��
        if (parkour.IsWallRun && exceptionFlag)
        {
            movementInfo.GroundedCheck();
        }

        // Ű�Է� �ޱ�
        WallRunMovement_KeyboardInput();
    }

    public void WallRunMovement_LateUpdate()
    {
    }

    public void WallRunMovement_Exit()
    {
        // �ʱ�ȭ �� �߷� ����
        exceptionTimer = 0.0f;
        exceptionFlag = false;
        movementInfo.player_rigidbody.useGravity = true;

        movementInfo.player_rigidbody.velocity = Vector3.zero;
        wallRunTime = 0.0f;

        // ���޸��� ���� �ִϸ��̼� ���� �� ���� �˸�
        StartCoroutine(WallRunEnd());
        parkour.IsWallRun = false;
        parkour.IsParkour = false;
    }

    // input Ű����
    void WallRunMovement_KeyboardInput()
    {
        if (movementInfo.inputLock)
            return;

    }

    /// <summary>
    /// ���� ������ Ȯ�� �� �ش� ������ �� Ÿ�� �ִϸ��̼� ���� �Լ�
    /// </summary>
    private IEnumerator WallRunStart()
    {
        if (!CheckWallRun())
            yield break;

        string aniMatorName = null;
        movementInfo.isGround = false;

        if (movementInfo.isAnimator)
        {
            if (movementInfo.detectObject.detectWallR.IsDetect)
            {
                movementInfo.animator.SetTrigger(movementInfo.aniHashWallRunStartR);
                aniMatorName = "Wall Run Start R";
            }

            else if (movementInfo.detectObject.detectWallL.IsDetect)
            {
                movementInfo.animator.SetTrigger(movementInfo.aniHashWallRunStartL);
                aniMatorName = "Wall Run Start L";
            }
            movementInfo.animator.ResetTrigger(movementInfo.aniHashWallRunEnd);
        }

        while (!movementInfo.animator.GetCurrentAnimatorStateInfo(0).IsName(aniMatorName))
        {
            yield return null;
        }
        while (movementInfo.animator.GetCurrentAnimatorStateInfo(0).IsName(aniMatorName) &&
            movementInfo.animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f)
        {
            yield return null;
        }
        parkour.IsWallRun = true;
    }

    /// <summary>
    /// WallRun ���� �ڷ�ƾ �Լ�
    /// </summary>
    private IEnumerator WallRunEnd()
    {
        movementInfo.inputLock = true;
        if (movementInfo.isAnimator)
        {
            movementInfo.animator.SetTrigger(movementInfo.aniHashWallRunEnd);
        }
        yield return null;

        if(wallRight)
        {
            movementInfo.player_rigidbody.AddForce(transform.forward * 6
                + -transform.right * 1.5f
            , ForceMode.Impulse);
        }
        else if(wallLeft)
        {
            movementInfo.player_rigidbody.AddForce(transform.forward * 6
                + transform.right * 1.5f
            , ForceMode.Impulse);
        }
        else
        {
            movementInfo.player_rigidbody.AddForce(transform.forward *  4
            , ForceMode.Impulse);
        }

        movementInfo.currentSpeed = 0;

        float endtime = 1.5f;
        float timer = 0.0f;
        bool flag = false;

        while(timer < endtime)
        {
            timer += Time.deltaTime;
            yield return null;

            //Ÿ�̸Ӱ� ���ư��� ������ �������ٸ� �÷��� on
            if(!movementInfo.isGround)
            {
                flag = true;
            }

            // �÷��װ� �����ְ� ���� ����ִٸ� ���� 
            if(flag && movementInfo.isGround)
            {
                break;
            }

        }
        movementInfo.inputLock = false;
    }

    // �̵��κ�
    private void Move()
    {
        if (!movementInfo.animator.GetCurrentAnimatorStateInfo(0).IsTag(aniTag)) return;

        float targetSpeed = wallRunSpeed;

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

        if (movementInfo.isAnimator)
        {
            movementInfo.animator.SetFloat(movementInfo.aniHashSpeed, movementInfo.currentSpeed);
        }
        // ���˻�
        CheckWallRun();

        // ���� ����
        Vector3 wallNormal = wallRight ? rightWallhit.normal : leftWallhit.normal;
        Vector3 wallHitPoint = wallRight ? rightWallhit.point : leftWallhit.point;
        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

        // �̵� �� ȸ�� �κ�
        if ((transform.forward - wallForward).magnitude > (transform.forward - -wallForward).magnitude)
            wallForward = -wallForward;


        // ���̶� ��������
        if (wallHitPoint != Vector3.zero &&(wallLeft || wallRight ) )
        {
            movementInfo.player_rigidbody.MovePosition(wallHitPoint + (wallNormal * 0.3f));
        }

        // ���� ������ ���� ��
        float angle = Mathf.Acos(Vector3.Dot(previousDirVec.normalized, wallForward.normalized)) * Mathf.Rad2Deg;

        // ������ �����̻��̸� ����
        if (angle > 60.0f)
        {
            movementInfo.moveState.ChangeState(MoveState.DefaultMovement);
            return;
        }
        // ��������
        previousDirVec = wallForward;

        // �̵�
        movementInfo.player_rigidbody.AddForce(wallForward.normalized *  (movementInfo.currentSpeed * Time.deltaTime) , ForceMode.Impulse) ;
      
        // ȸ��
        if (wallForward != Vector3.zero)
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(wallForward.normalized), 1);

        // ���� ����
        if (wallRunUpTime > wallRunTime)
        {
            float upForce = 25f;
            wallRunTime += Time.deltaTime;
            movementInfo.player_rigidbody.AddForce(this.transform.up * upForce * Time.deltaTime);
        }
        else
        {
            float downForce = 55f;
            movementInfo.player_rigidbody.AddForce(-this.transform.up * downForce * Time.deltaTime);
        }
    }

    /// <summary>
    /// ���� �ִ¹��� üũ�ϴ� �Լ�
    /// </summary>
    private bool CheckWallRun()
    {

        if (wallRight = Physics.Raycast(transform.position, transform.right, out rightWallhit, wallCheckDistance, wallLayerMask)) { }
        else wallRight = Physics.Raycast(transform.position + new Vector3(0, movementInfo.player_collider.height/4, 0), transform.right, out rightWallhit, wallCheckDistance, wallLayerMask);
        if (wallLeft = Physics.Raycast(transform.position, -transform.right, out leftWallhit, wallCheckDistance, wallLayerMask)) { }
        else wallLeft = Physics.Raycast(transform.position + new Vector3(0, movementInfo.player_collider.height/4, 0), -transform.right, out leftWallhit, wallCheckDistance, wallLayerMask);

        // ������� : ������ �̵��� Ǯ���ų�, �ڷ��̵��� ������� , ���� ����� ��� , �¿쿡 ���� �������
        // ���� �ʱ⿡�� ���� ��Ƶ� ��Ұ� �ȵǰ� �ؾߵ� ��� �翷�� ���� ������ ���
        if (!exceptionFlag && (!wallLeft && !wallRight))
        {
            movementInfo.moveState.ChangeState(MoveState.DefaultMovement);
            return false;
        }
        // ���� �ð��� ������� 
        else if (exceptionFlag &&  ( movementInfo.moveVec.y <= 0 || movementInfo.isGround || (!wallLeft && !wallRight) ))
        {
            movementInfo.moveState.ChangeState(MoveState.DefaultMovement);
            return false;
        }

        return true;
    }




}

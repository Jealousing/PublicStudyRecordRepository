using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 벽을 타고 달리는 움직이는 상태의 클래스
/// </summary>
public class WallRunMove : MonoBehaviour
{
    // 움직임 정보와 파쿠르정보 참조
    MovementInfo movementInfo;
    Parkour parkour;

    private float wallRunSpeed = 5.0f;
    private float wallCheckDistance = 2f;
    private float speedChangeRate = 10.0f;
    private float speedOffset = 0.2f;

    // 벽이 있는지 확인
    private bool wallLeft;
    private bool wallRight;

    [Tooltip("탈 수 있는 벽의 레이어")] public LayerMask wallLayerMask;
    private RaycastHit leftWallhit;
    private RaycastHit rightWallhit;

    // 처음 벽을 타기 시작할 때 몇 초 동안 올라갈 것인지.
    private float wallRunTime;
    private float wallRunUpTime = 3.0f;

    // 처음 몇초는 땅과 가까워도 달릴 수 있도록 설정하는 변수
    private float exceptionTimer = 0.0f;
    private float setTime = 1.5f;
    private bool exceptionFlag = false;

    // 이전 진행 방향벡터
    private Vector3 previousDirVec;

    string aniTag = "WallRunTag";

    private void Start()
    {
        movementInfo = GetComponent<MovementInfo>();
        parkour = GetComponent<Parkour>();
    }


    public void WallRunMovement_Enter()
    {
        // 초기화 및 중력적용 x
        exceptionFlag = false;
        exceptionTimer = 0.0f;
        previousDirVec = this.transform.forward;
        movementInfo.player_rigidbody.useGravity = false;
        movementInfo.player_rigidbody.velocity = Vector3.zero;

        // 애니메이션 실행
        StartCoroutine(WallRunStart());
    }

    public void WallRunMovement_FixedUpdate()
    {
        Move();
    }

    public void WallRunMovement_Update()
    {

        // 예외 처리를 위한방법
        if(!exceptionFlag && exceptionTimer <= setTime)
        {
            exceptionTimer += Time.deltaTime;
        }
        else
        {
            exceptionFlag = true;
        }

        // 일정 시간지나야 땅확인
        if (parkour.IsWallRun && exceptionFlag)
        {
            movementInfo.GroundedCheck();
        }

        // 키입력 받기
        WallRunMovement_KeyboardInput();
    }

    public void WallRunMovement_LateUpdate()
    {
    }

    public void WallRunMovement_Exit()
    {
        // 초기화 및 중력 적용
        exceptionTimer = 0.0f;
        exceptionFlag = false;
        movementInfo.player_rigidbody.useGravity = true;

        movementInfo.player_rigidbody.velocity = Vector3.zero;
        wallRunTime = 0.0f;

        // 벽달리기 종료 애니메이션 실행 및 종료 알림
        StartCoroutine(WallRunEnd());
        parkour.IsWallRun = false;
        parkour.IsParkour = false;
    }

    // input 키보드
    void WallRunMovement_KeyboardInput()
    {
        if (movementInfo.inputLock)
            return;

    }

    /// <summary>
    /// 벽의 방향을 확인 후 해당 방향의 벽 타는 애니메이션 실행 함수
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
    /// WallRun 종료 코루틴 함수
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

            //타이머가 돌아갈때 땅에서 떨어진다면 플레그 on
            if(!movementInfo.isGround)
            {
                flag = true;
            }

            // 플레그가 켜져있고 땅을 밞고있다면 종료 
            if(flag && movementInfo.isGround)
            {
                break;
            }

        }
        movementInfo.inputLock = false;
    }

    // 이동부분
    private void Move()
    {
        if (!movementInfo.animator.GetCurrentAnimatorStateInfo(0).IsTag(aniTag)) return;

        float targetSpeed = wallRunSpeed;

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

        if (movementInfo.isAnimator)
        {
            movementInfo.animator.SetFloat(movementInfo.aniHashSpeed, movementInfo.currentSpeed);
        }
        // 벽검사
        CheckWallRun();

        // 방향 설정
        Vector3 wallNormal = wallRight ? rightWallhit.normal : leftWallhit.normal;
        Vector3 wallHitPoint = wallRight ? rightWallhit.point : leftWallhit.point;
        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

        // 이동 및 회전 부분
        if ((transform.forward - wallForward).magnitude > (transform.forward - -wallForward).magnitude)
            wallForward = -wallForward;


        // 벽이랑 간격유지
        if (wallHitPoint != Vector3.zero &&(wallLeft || wallRight ) )
        {
            movementInfo.player_rigidbody.MovePosition(wallHitPoint + (wallNormal * 0.3f));
        }

        // 이전 계산과의 각도 비교
        float angle = Mathf.Acos(Vector3.Dot(previousDirVec.normalized, wallForward.normalized)) * Mathf.Rad2Deg;

        // 각도가 일정이상이면 종료
        if (angle > 60.0f)
        {
            movementInfo.moveState.ChangeState(MoveState.DefaultMovement);
            return;
        }
        // 방향저장
        previousDirVec = wallForward;

        // 이동
        movementInfo.player_rigidbody.AddForce(wallForward.normalized *  (movementInfo.currentSpeed * Time.deltaTime) , ForceMode.Impulse) ;
      
        // 회전
        if (wallForward != Vector3.zero)
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(wallForward.normalized), 1);

        // 높이 조절
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
    /// 벽이 있는방향 체크하는 함수
    /// </summary>
    private bool CheckWallRun()
    {

        if (wallRight = Physics.Raycast(transform.position, transform.right, out rightWallhit, wallCheckDistance, wallLayerMask)) { }
        else wallRight = Physics.Raycast(transform.position + new Vector3(0, movementInfo.player_collider.height/4, 0), transform.right, out rightWallhit, wallCheckDistance, wallLayerMask);
        if (wallLeft = Physics.Raycast(transform.position, -transform.right, out leftWallhit, wallCheckDistance, wallLayerMask)) { }
        else wallLeft = Physics.Raycast(transform.position + new Vector3(0, movementInfo.player_collider.height/4, 0), -transform.right, out leftWallhit, wallCheckDistance, wallLayerMask);

        // 취소조건 : 앞으로 이동이 풀리거나, 뒤로이동을 누를경우 , 땅을 밞았을 경우 , 좌우에 벽이 없을경우
        // 시작 초기에는 땅을 밞아도 취소가 안되게 해야됨 대신 양옆에 벽이 없으면 취소
        if (!exceptionFlag && (!wallLeft && !wallRight))
        {
            movementInfo.moveState.ChangeState(MoveState.DefaultMovement);
            return false;
        }
        // 일정 시간이 지난경우 
        else if (exceptionFlag &&  ( movementInfo.moveVec.y <= 0 || movementInfo.isGround || (!wallLeft && !wallRight) ))
        {
            movementInfo.moveState.ChangeState(MoveState.DefaultMovement);
            return false;
        }

        return true;
    }




}

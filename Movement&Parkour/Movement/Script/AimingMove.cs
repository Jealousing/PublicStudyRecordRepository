using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimingMove : MonoBehaviour
{ 
    // 움직임 정보
    MovementInfo movementInfo;
    // 이동속도, 오차값, 속도 변경 속도
    [Tooltip("기본 이동속도")] public float moveSpeed = 3.0f;
    [Tooltip("달리기 속도")] public float sprintSpeed = 6.0f;
    private bool isRun;
    private float speedOffset = 0.2f;
    private float speedChangeRate = 10.0f;
    void Start()
    {
        movementInfo = GetComponent<MovementInfo>();
    }

    public void AimingMove_Enter()
    {
    }

    public void AimingMove_Exit()
    {
    }

    public void AimingMove_FixedUpdate()
    {
        Move();
    }
    public void AimingMove_Update()
    {
        AimingMovement_KeyboardInput();
    }
    public void AimingMove_LateUpdate()
    {

    }
    
    // input 키보드
    void AimingMovement_KeyboardInput()
    {
        if (movementInfo.inputLock)
            return;
    }

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
        Vector3 inputDirection = transform.forward * movementInfo.moveVec.y + transform.right * movementInfo.moveVec.x;

        // 이동방향 및 이동처리
        movementInfo.player_rigidbody.MovePosition(this.gameObject.transform.position + inputDirection.normalized * movementInfo.currentSpeed * Time.deltaTime);
        if (movementInfo.isAnimator)
        {
            movementInfo.animator.SetFloat(movementInfo.aniHashSpeed, movementInfo.currentSpeed);
        }


    }
}

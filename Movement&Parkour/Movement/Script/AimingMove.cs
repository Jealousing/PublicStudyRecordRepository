using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimingMove : MonoBehaviour
{ 
    // ������ ����
    MovementInfo movementInfo;
    // �̵��ӵ�, ������, �ӵ� ���� �ӵ�
    [Tooltip("�⺻ �̵��ӵ�")] public float moveSpeed = 3.0f;
    [Tooltip("�޸��� �ӵ�")] public float sprintSpeed = 6.0f;
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
    
    // input Ű����
    void AimingMovement_KeyboardInput()
    {
        if (movementInfo.inputLock)
            return;
    }

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
        Vector3 inputDirection = transform.forward * movementInfo.moveVec.y + transform.right * movementInfo.moveVec.x;

        // �̵����� �� �̵�ó��
        movementInfo.player_rigidbody.MovePosition(this.gameObject.transform.position + inputDirection.normalized * movementInfo.currentSpeed * Time.deltaTime);
        if (movementInfo.isAnimator)
        {
            movementInfo.animator.SetFloat(movementInfo.aniHashSpeed, movementInfo.currentSpeed);
        }


    }
}

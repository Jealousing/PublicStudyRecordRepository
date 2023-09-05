using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
///  Player�� ������ ���¸� ���¸ӽ����� �����ϴ� Ŭ����
/// </summary>
public class MoveStateSystem : MonoBehaviour
{
    // �׼Ǹ���Ʈ�� Ȱ���� ���¸ӽ�
    MoveState startControlType = MoveState.DefaultMovement;
    MoveState movePreviousState;
    [Header ("���� ������ ����")]
    public MoveState moveCurrentState;
    Action<StateFlow> movePreviousCallFN = null;
    Action<StateFlow> moveCurrentCallFN = null;
    List<Action<StateFlow>> moveCallFNList = new List<Action<StateFlow>>();

    // ������ ������ ���� Ŭ��������
    DefaultMove defaultMove;
    ClimbingMove climbingMove;
    WallRunMove wallRunMove;
    AimingMove aimingMove;


    // ����Ƽ �⺻ �̺�Ʈ �Լ�
    private void Awake()
    {
        defaultMove = GetComponent<DefaultMove>();
        climbingMove = GetComponent<ClimbingMove>();
        wallRunMove = GetComponent<WallRunMove>();
        aimingMove =GetComponent<AimingMove>();
    }

    private void Start()
    {
        // �׼Ǹ���Ʈ ����
        for (int i = 0; i < ((int)(MoveState.TempState)) + 1; i++)
        {
            moveCallFNList.Add(null);
        }
        moveCallFNList[(int)MoveState.DefaultMovement] = DefaultMovementState;
        moveCallFNList[(int)MoveState.ClimbingMovement] = ClimbingMovementState;
        moveCallFNList[(int)MoveState.WallRunMovement] = WallRunMovementState;
        moveCallFNList[(int)MoveState.AimingMovement] = AimingMovementState;
        moveCallFNList[(int)MoveState.NotMovement] = NotMovementState;
        //movementCallFNList[(int)MoveStateSystem.NotMovement] = testMovementState;

        // ���� ���� ���� �� Enter ȣ��
        moveCurrentCallFN = moveCallFNList[(int)startControlType];
        moveCurrentCallFN(StateFlow.ENTER);
    }

    private void FixedUpdate()
    {
        //action?.Invoke ��� ������ �ݹ��� null�� �ƴҶ��� �����ϵ��� ����.
        moveCurrentCallFN?.Invoke(StateFlow.FIXEDUPDATE);
    }

    void Update()
    {
        moveCurrentCallFN?.Invoke(StateFlow.UPDATE);
    }

    void LateUpdate()
    {
        moveCurrentCallFN?.Invoke(StateFlow.LATEUPDATE);
    }

    /// <summary>
    /// ���� �÷��̾��� ������ ���¸� �����´�.
    /// </summary>
    /// <returns> enum ������ ���� ������ ���� </returns>
    public MoveState GetCurrentMoveState()
    {
        return moveCurrentState;
    }

    /// <summary>
    /// ���� �÷��̾��� ������ ���¸� �����Ѵ�.
    /// </summary>
    /// <param name="nextState"> ������ ���� ������ ���� </param>
    public void ChangeState(MoveState nextState)
    {
        //�������� ���¿� ������ ���°� �ٸ���츸 ����
        if (moveCurrentState == nextState)
            return;

        //���� ���� ����
        movePreviousState = moveCurrentState;
        movePreviousCallFN = moveCurrentCallFN;
        //������� ����
        moveCurrentState = nextState;
        moveCurrentCallFN = moveCallFNList[(int)nextState];

        //�������̴� ���� exit ����
        movePreviousCallFN(StateFlow.EXIT);

        //���� ���� �˸�
        moveCurrentCallFN(StateFlow.ENTER);
    }

    /*
            �� ������ ���¿� ���� ����
                                                        */
    private void DefaultMovementState(StateFlow stateFlow)
    {
        switch (stateFlow)
        {
            case StateFlow.ENTER:
                defaultMove.enabled = true;
                defaultMove.DefaultMove_Enter();
                break;
            case StateFlow.FIXEDUPDATE:
                defaultMove.DefaultMove_FixedUpdate();
                break;
            case StateFlow.UPDATE:
                defaultMove.DefaultMove_Update();
                break;
            case StateFlow.LATEUPDATE:
                defaultMove.DefaultMove_LateUpdate();
                break;
            case StateFlow.EXIT:
                defaultMove.enabled = false;
                defaultMove.DefaultMove_Exit();
                break;
        }
    }

    private void ClimbingMovementState(StateFlow stateFlow)
    {
        switch (stateFlow)
        {
            case StateFlow.ENTER:
                climbingMove.enabled = true;
                climbingMove.ClimbingMove_Enter();
                break;
            case StateFlow.FIXEDUPDATE:
                climbingMove.ClimbingMove_FixedUpdate();
                break;
            case StateFlow.UPDATE:
                climbingMove.ClimbingMove_Update();
                break;
            case StateFlow.LATEUPDATE:
                climbingMove.ClimbingMove_LateUpdate();
                break;
            case StateFlow.EXIT:
                climbingMove.enabled = false;
                climbingMove.ClimbingMove_Exit();
                break;
        }
    }

    private void WallRunMovementState(StateFlow stateFlow)
    {
        switch (stateFlow)
        {
            case StateFlow.ENTER:
                wallRunMove.enabled = true;
                wallRunMove.WallRunMovement_Enter();
                break;
            case StateFlow.FIXEDUPDATE:
                wallRunMove.WallRunMovement_FixedUpdate();
                break;
            case StateFlow.UPDATE:
                wallRunMove.WallRunMovement_Update();
                break;
            case StateFlow.LATEUPDATE:
                wallRunMove.WallRunMovement_LateUpdate();
                break;
            case StateFlow.EXIT:
                wallRunMove.enabled = false;    
                wallRunMove.WallRunMovement_Exit();
                break;
        }
    }

    private void AimingMovementState(StateFlow stateFlow)
    {
        switch (stateFlow)
        {
            case StateFlow.ENTER:
                aimingMove.enabled = true;
                aimingMove.AimingMove_Enter();
                break;
            case StateFlow.FIXEDUPDATE:
                aimingMove.AimingMove_FixedUpdate();
                break;
            case StateFlow.UPDATE:
                aimingMove.AimingMove_Update();
                break;
            case StateFlow.LATEUPDATE:
                aimingMove.AimingMove_LateUpdate();
                break;
            case StateFlow.EXIT:
                aimingMove.enabled = false;
                aimingMove.AimingMove_Exit();
                break;
        }
    }

    private void NotMovementState(StateFlow stateFlow)
    {
        switch (stateFlow)
        {
            case StateFlow.ENTER:
                break;
            case StateFlow.FIXEDUPDATE:
                break;
            case StateFlow.UPDATE:
                break;
            case StateFlow.LATEUPDATE:
                break;
            case StateFlow.EXIT:
                break;
        }
    }
}

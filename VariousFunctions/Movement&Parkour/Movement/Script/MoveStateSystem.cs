using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
///  Player의 움직임 상태를 상태머신으로 관리하는 클래스
/// </summary>
public class MoveStateSystem : MonoBehaviour
{
    // 액션리스트을 활용한 상태머신
    MoveState startControlType = MoveState.DefaultMovement;
    MoveState movePreviousState;
    [Header ("현재 움직임 상태")]
    public MoveState moveCurrentState;
    Action<StateFlow> movePreviousCallFN = null;
    Action<StateFlow> moveCurrentCallFN = null;
    List<Action<StateFlow>> moveCallFNList = new List<Action<StateFlow>>();

    // 움직임 종류에 따른 클래스정보
    DefaultMove defaultMove;
    ClimbingMove climbingMove;
    WallRunMove wallRunMove;
    AimingMove aimingMove;


    // 유니티 기본 이벤트 함수
    private void Awake()
    {
        defaultMove = GetComponent<DefaultMove>();
        climbingMove = GetComponent<ClimbingMove>();
        wallRunMove = GetComponent<WallRunMove>();
        aimingMove =GetComponent<AimingMove>();
    }

    private void Start()
    {
        // 액션리스트 셋팅
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

        // 시작 상태 설정 및 Enter 호출
        moveCurrentCallFN = moveCallFNList[(int)startControlType];
        moveCurrentCallFN(StateFlow.ENTER);
    }

    private void FixedUpdate()
    {
        //action?.Invoke 사용 이유는 콜백이 null이 아닐때만 실행하도록 설정.
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
    /// 현재 플레이어의 움직임 상태를 가져온다.
    /// </summary>
    /// <returns> enum 형태의 현재 움직임 상태 </returns>
    public MoveState GetCurrentMoveState()
    {
        return moveCurrentState;
    }

    /// <summary>
    /// 현재 플레이어의 움직임 상태를 변경한다.
    /// </summary>
    /// <param name="nextState"> 변경할 다음 움직임 상태 </param>
    public void ChangeState(MoveState nextState)
    {
        //진행중인 상태와 변경할 상태가 다를경우만 진행
        if (moveCurrentState == nextState)
            return;

        //이전 상태 저장
        movePreviousState = moveCurrentState;
        movePreviousCallFN = moveCurrentCallFN;
        //현재상태 변경
        moveCurrentState = nextState;
        moveCurrentCallFN = moveCallFNList[(int)nextState];

        //진행중이던 상태 exit 실행
        movePreviousCallFN(StateFlow.EXIT);

        //상태 진입 알림
        moveCurrentCallFN(StateFlow.ENTER);
    }

    /*
            각 움직임 상태에 따른 설정
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

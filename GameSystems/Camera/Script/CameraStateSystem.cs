using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player의 카메라 상태를 상태머신으로 관리하는 클래스
/// </summary>
public class CameraStateSystem : MonoBehaviour
{
    // 액션리스트을 활용한 상태머신
    CameraState startCameraType = CameraState.BackView;
    CameraState cameraPreviousState;
    [Header("현재 움직임 상태")]
    CameraState cameraCurrentState;
    Action<StateFlow> cameraPreviousCallFN = null;
    Action<StateFlow> cameraCurrentCallFN = null;
    List<Action<StateFlow>> cameraCallFNList = new List<Action<StateFlow>>();

    // 카메라 종류에 따른 클래스정보
    BackViewCamera backViewCamera;
    BowZoomCamera bowZoomCamera;
    private void Awake()
    {
        backViewCamera = GetComponent<BackViewCamera>();
        bowZoomCamera = GetComponent<BowZoomCamera>();
    }

    private void Start()
    {
        // 액션리스트 셋팅
        for (int i = 0; i < ((int)(CameraState.TempState)) + 1; i++)
        {
            cameraCallFNList.Add(null);
        }
        cameraCallFNList[(int)CameraState.BackView] = BackViewState;
        cameraCallFNList[(int)CameraState.BowZoom] = BowZoomState;
        //cameraCallFNList[(int)CameraStateSystem.FirstPersonView] = ;

        // 시작 상태 설정 및 Enter 호출
        cameraCurrentCallFN = cameraCallFNList[(int)startCameraType];
        cameraCurrentCallFN(StateFlow.ENTER);
    }

    private void FixedUpdate()
    {
        //action?.Invoke 사용 이유는 콜백이 null이 아닐때만 실행하도록 설정.
        cameraCurrentCallFN?.Invoke(StateFlow.FIXEDUPDATE);
    }

    void Update()
    {
        cameraCurrentCallFN?.Invoke(StateFlow.UPDATE);
    }

    void LateUpdate()
    {
        cameraCurrentCallFN?.Invoke(StateFlow.LATEUPDATE);
    }

    /// <summary>
    /// 현재 플레이어의 카메라 상태(정보)를 가져온다
    /// </summary>
    public CameraState GetCurrentMoveState()
    {
        return cameraCurrentState;
    }

    /// <summary>
    /// 현재 플레이어의 카메라 상태(정보)를 변경한다.
    /// </summary>
    /// <param name="nextState"> 변경할 다음 카메라 상태 </param>
    public void ChangeState(CameraState nextState)
    {
        //진행중인 상태와 변경할 상태가 다를경우만 진행
        if (cameraCurrentState == nextState)
            return;

        //이전 상태 저장
        cameraPreviousState = cameraCurrentState;
        cameraPreviousCallFN = cameraCurrentCallFN;
        //현재상태 변경
        cameraCurrentState = nextState;
        cameraCurrentCallFN = cameraCallFNList[(int)nextState];

        //진행중이던 상태 exit 실행
        cameraPreviousCallFN(StateFlow.EXIT);

        //상태 진입 알림
        cameraCurrentCallFN(StateFlow.ENTER);
    }

    /*
         각 카메라 상태에 따른 설정
                                                     */
    private void BackViewState(StateFlow stateFlow)
    {
        switch (stateFlow)
        {
            case StateFlow.ENTER:
                backViewCamera.BackViewCamera_Enter();
                break;
            case StateFlow.FIXEDUPDATE:
                backViewCamera.BackViewCamera_FixedUpdate();
                break;
            case StateFlow.UPDATE:
                backViewCamera.BackViewCamera_Update();
                break;
            case StateFlow.LATEUPDATE:
                backViewCamera.BackViewCamera_LateUpdate();
                break;
            case StateFlow.EXIT:
                backViewCamera.BackViewCamera_Exit();
                break;
        }
    }
    private void BowZoomState(StateFlow stateFlow)
    {
        switch (stateFlow)
        {
            case StateFlow.ENTER:
                bowZoomCamera.BowZoomCamera_Enter();
                break;
            case StateFlow.FIXEDUPDATE:
                bowZoomCamera.BowZoomCamera_FixedUpdate();
                break;
            case StateFlow.UPDATE:
                bowZoomCamera.BowZoomCamera_Update();
                break;
            case StateFlow.LATEUPDATE:
                bowZoomCamera.BowZoomCamera_LateUpdate();
                break;
            case StateFlow.EXIT:
                bowZoomCamera.BowZoomCamera_Exit();
                break;
        }
    }
}

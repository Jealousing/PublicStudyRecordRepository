using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player�� ī�޶� ���¸� ���¸ӽ����� �����ϴ� Ŭ����
/// </summary>
public class CameraStateSystem : MonoBehaviour
{
    // �׼Ǹ���Ʈ�� Ȱ���� ���¸ӽ�
    CameraState startCameraType = CameraState.BackView;
    CameraState cameraPreviousState;
    [Header("���� ������ ����")]
    CameraState cameraCurrentState;
    Action<StateFlow> cameraPreviousCallFN = null;
    Action<StateFlow> cameraCurrentCallFN = null;
    List<Action<StateFlow>> cameraCallFNList = new List<Action<StateFlow>>();

    // ī�޶� ������ ���� Ŭ��������
    BackViewCamera backViewCamera;
    BowZoomCamera bowZoomCamera;
    private void Awake()
    {
        backViewCamera = GetComponent<BackViewCamera>();
        bowZoomCamera = GetComponent<BowZoomCamera>();
    }

    private void Start()
    {
        // �׼Ǹ���Ʈ ����
        for (int i = 0; i < ((int)(CameraState.TempState)) + 1; i++)
        {
            cameraCallFNList.Add(null);
        }
        cameraCallFNList[(int)CameraState.BackView] = BackViewState;
        cameraCallFNList[(int)CameraState.BowZoom] = BowZoomState;
        //cameraCallFNList[(int)CameraStateSystem.FirstPersonView] = ;

        // ���� ���� ���� �� Enter ȣ��
        cameraCurrentCallFN = cameraCallFNList[(int)startCameraType];
        cameraCurrentCallFN(StateFlow.ENTER);
    }

    private void FixedUpdate()
    {
        //action?.Invoke ��� ������ �ݹ��� null�� �ƴҶ��� �����ϵ��� ����.
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
    /// ���� �÷��̾��� ī�޶� ����(����)�� �����´�
    /// </summary>
    public CameraState GetCurrentMoveState()
    {
        return cameraCurrentState;
    }

    /// <summary>
    /// ���� �÷��̾��� ī�޶� ����(����)�� �����Ѵ�.
    /// </summary>
    /// <param name="nextState"> ������ ���� ī�޶� ���� </param>
    public void ChangeState(CameraState nextState)
    {
        //�������� ���¿� ������ ���°� �ٸ���츸 ����
        if (cameraCurrentState == nextState)
            return;

        //���� ���� ����
        cameraPreviousState = cameraCurrentState;
        cameraPreviousCallFN = cameraCurrentCallFN;
        //������� ����
        cameraCurrentState = nextState;
        cameraCurrentCallFN = cameraCallFNList[(int)nextState];

        //�������̴� ���� exit ����
        cameraPreviousCallFN(StateFlow.EXIT);

        //���� ���� �˸�
        cameraCurrentCallFN(StateFlow.ENTER);
    }

    /*
         �� ī�޶� ���¿� ���� ����
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

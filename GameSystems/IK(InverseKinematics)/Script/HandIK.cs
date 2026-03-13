using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

/// <summary>
/// 플레이어 손IK(역운동학) 클래스
/// </summary>
public class HandIK : MonoBehaviour
{
    
    Animator playerAnimator;

    // 현재값
    Vector3 leftHandIKPositionBuffer;
    Vector3 rightHandIKPositionBuffer;
    Vector3 leftHandIKRotationBuffer;
    Vector3 rightHandIKRotationBuffer;

    // 목표값
    [NonSerialized]
    public Vector3 leftHandIKPositionTarget;
    [NonSerialized]
    public Vector3 rightHandIKPositionTarget;
    [NonSerialized]
    public Vector3 leftHandIKRotationTarget;
    [NonSerialized]
    public Vector3 rightHandIKRotationTarget;

    // SmoothDamp용 ref 키워드사용할 변수
    Vector3 leftHandPositionLerpVelocity;
    Vector3 rightHandPositionLerpVelocity;
    Vector3 leftHandRotationLerpVelocity;
    Vector3 rightHandRotationLerpVelocity;

    // 목표에 도달하는 데 걸리는 대략적인 시간
    public float smoothTime;

    // 가중치 range(0, 1)
    [NonSerialized]
    public float leftPositionWeight;
    [NonSerialized]
    public float rightPositionWeight;
    [NonSerialized]
    public float leftRotationWeight;
    [NonSerialized]
    public float rightRotationWeight;

    // 상태에 따른 가중치 조절
    float curGlobalWeight;


    // 작동 여부
    [SerializeField]
    private bool IsIKPosition;
    [SerializeField]
    private bool IsIKRotation;

    public bool IsBodyIK;

    // 손오브젝트
    public GameObject leftHand;
    public GameObject rightHand;

    [NonSerialized]
    public bool isLeftHandReady;
    [NonSerialized]
    public bool isRightHandReady;

    // 높이 차이
    float differenceHeight;

    // 상태에 따른 변화 
    private HandIkState state = HandIkState.NORMAL;

    // 초기값 저장변수
    Transform leftHandDefaultTransform;
    Transform rightHandDefaultTransform;

    // 초기화 Flag
    bool isInitialize = false;


    void Start()
    {
        CreateOrientationReference();
    }

    void Update()
    {
        UpdateBuffer();

        if (state == HandIkState.NORMAL)
        {
            leftHandIKPositionTarget = leftHandDefaultTransform.position;
            leftHandIKPositionBuffer = leftHandDefaultTransform.position;
            rightHandIKPositionTarget = rightHandDefaultTransform.position;
            rightHandIKPositionBuffer = rightHandDefaultTransform.position;
        }
    }

    /// <summary>
    /// 손 상태 변경
    /// </summary>
    public void ChangeState(HandIkState handstate)
    {
        if(state== handstate) return;

        if (handstate == HandIkState.NORMAL)
        {
            leftHandIKPositionTarget = leftHandDefaultTransform.position;
            leftHandIKPositionBuffer= leftHandDefaultTransform.position;
            rightHandIKPositionTarget = rightHandDefaultTransform.position;
            rightHandIKPositionBuffer = rightHandDefaultTransform.position;

        }
            state = handstate;
    }

    // 손의 회전을 자체적으로 하지 못해서 뼈 방향을 추적하기 위한 함수
    private void CreateOrientationReference()
    {
        //재호출 대비
        if (leftHandDefaultTransform != null)
        {
            Destroy(leftHandDefaultTransform);
        }
        if (rightHandDefaultTransform != null)
        {
            Destroy(rightHandDefaultTransform);
        }

        // 손의 새로운 오브젝트를 만들어서 기본 방향값을 유지시킨다.

        leftHandDefaultTransform = new GameObject("LeftHand_OrientationReference").transform;
        rightHandDefaultTransform = new GameObject("RightHand_OrientationReference").transform;

        leftHandDefaultTransform.position = leftHand.transform.position;
        rightHandDefaultTransform.position = rightHand.transform.position;

        leftHandDefaultTransform.SetParent(leftHand.transform);
        rightHandDefaultTransform.SetParent(rightHand.transform);

    }

    // 변수 초기화
    private void InitializeVariables()
    {
        playerAnimator = GetComponent<Animator>();
        // 첫 번째 프레임이 자연스럽게 보이도록 설정.
        leftHandIKPositionBuffer = playerAnimator.GetIKPosition(AvatarIKGoal.LeftHand);
        rightHandIKPositionBuffer = playerAnimator.GetIKPosition(AvatarIKGoal.RightHand);
        leftHandIKPositionTarget = playerAnimator.GetIKPosition(AvatarIKGoal.LeftHand);
        rightHandIKPositionTarget = playerAnimator.GetIKPosition(AvatarIKGoal.RightHand);
    }


    // 손 IK의 좌표값과 각도를 자연스럽게 변화시키는 함수
    private void UpdateBuffer()
    {

        if(state == HandIkState.GRAPPLINGHOOKMOVE)
        {
            leftHandIKPositionBuffer = leftHandIKPositionTarget;
            rightHandIKPositionBuffer = rightHandIKPositionTarget;
            leftHandIKRotationBuffer = leftHandIKRotationTarget;
            rightHandIKRotationBuffer = rightHandIKRotationTarget;

        }
        else
        {

            leftHandIKPositionBuffer =
            Vector3.SmoothDamp(leftHandIKPositionBuffer, leftHandIKPositionTarget, ref leftHandPositionLerpVelocity, smoothTime);

            rightHandIKPositionBuffer =
                Vector3.SmoothDamp(rightHandIKPositionBuffer, rightHandIKPositionTarget, ref rightHandPositionLerpVelocity, smoothTime);

            leftHandIKRotationBuffer =
             Vector3.SmoothDamp(leftHandIKRotationBuffer, leftHandIKRotationTarget, ref leftHandRotationLerpVelocity, smoothTime);

            rightHandIKRotationBuffer =
                Vector3.SmoothDamp(rightHandIKRotationBuffer, rightHandIKRotationTarget, ref rightHandRotationLerpVelocity, smoothTime);

        }

        if (Vector3.Distance(leftHandIKPositionBuffer, leftHandIKPositionTarget) < 0.6f)
        {
            isLeftHandReady = true;
        }
        else
        {
            isLeftHandReady = false;
        }

        if (Vector3.Distance(rightHandIKPositionBuffer, rightHandIKPositionTarget) < 0.6f)
        {
            isRightHandReady = true;
        }
        else
        {
            isRightHandReady = false;
        }



    }
    private void OnAnimatorIK()
    {
        if(! isInitialize)
        {
            isInitialize = true;
            // 변수 초기화
            InitializeVariables();
        }
        ApplyHandIK();
        ApplyBodyIK();
    }

    // 축에 따른 선형보간 진행
    private void LerpByAxis(Vector3 currentPosition, ref Vector3 resultPosition, bool IsAxisX, bool IsAxisY, bool IsAxisZ)
    {
        // IsAxis 값이 true면 그 축은 resultPosition으로 선형보간 false면 current값으로 설정
        resultPosition = new Vector3
        (
            Mathf.Lerp(currentPosition.x, resultPosition.x, Convert.ToInt32(IsAxisX)),
            Mathf.Lerp(currentPosition.y, resultPosition.y, Convert.ToInt32(IsAxisY)),
            Mathf.Lerp(currentPosition.z, resultPosition.z, Convert.ToInt32(IsAxisZ))
        );
    }


    // 손의 IK정보 업데이트
    private void ApplyHandIK()
    {
        // 위치 값 적용 현재는 왼손만 사용 나중에 나눠서 구분
        // IK에 있는 발의 위치 정보를 토대로 버퍼에 담겨있는 위치 정보를 수정한다.
        if (state == HandIkState.VAULT)
        {
            LerpByAxis(playerAnimator.GetIKPosition(AvatarIKGoal.LeftHand), ref leftHandIKPositionBuffer, false, true, false);
            LerpByAxis(playerAnimator.GetIKPosition(AvatarIKGoal.RightHand), ref rightHandIKPositionBuffer, false, true, false);
        }

        // globalWeight(가중치)가 1에 가까울수록 IK가 더욱 강하게 적용된다.
        playerAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, leftPositionWeight);
        playerAnimator.SetIKPositionWeight(AvatarIKGoal.RightHand, rightPositionWeight);

        if (IsIKPosition == true)
        {
            // 버퍼에 저장된 위치값을 적용
            playerAnimator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandIKPositionBuffer);
            playerAnimator.SetIKPosition(AvatarIKGoal.RightHand, rightHandIKPositionBuffer);
        }
        Quaternion leftHandRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        Quaternion rightHandRotation = Quaternion.Euler(new Vector3(0, 0, 0));

        // 회전 값 적용
        if (leftHandIKRotationTarget != Vector3.zero)
            leftHandRotation = Quaternion.LookRotation(leftHandIKRotationTarget) * Quaternion.Euler(new Vector3(-90,0,0));

        if (rightHandIKRotationTarget != Vector3.zero)
             rightHandRotation = Quaternion.LookRotation(rightHandIKRotationTarget) * Quaternion.Euler(new Vector3(-90, 0, 0));

        playerAnimator.SetIKRotationWeight(AvatarIKGoal.LeftHand, leftRotationWeight);
        playerAnimator.SetIKRotationWeight(AvatarIKGoal.RightHand, rightRotationWeight);

        if (IsIKRotation == true)
        {
            // 회전 값 적용
            playerAnimator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandRotation);
            playerAnimator.SetIKRotation(AvatarIKGoal.RightHand, rightHandRotation);
        }

    }

    

    // 몸의 IK정보 업데이트
    private void ApplyBodyIK()
    {
        if (IsBodyIK == false)
        {
            return;
        }

        if(state == HandIkState.VAULT)
        {
            curGlobalWeight = Mathf.Lerp(curGlobalWeight, leftPositionWeight, Time.deltaTime * 10f);
            // 손이 더 낮은 곳에 위치할 경우
            if (leftHand.transform.position.y < leftHandIKPositionTarget.y)
            {
                differenceHeight = (leftHand.transform.position.y - leftHandIKPositionTarget.y);
            }
            else
            {
                differenceHeight = (leftHandIKPositionTarget.y - leftHand.transform.position.y);
            }
        }
        else
        {
            curGlobalWeight = 0;
            differenceHeight = 0.0f;
        }

        // 낮은 위치에 있는 발을 기준으로 몸의 y좌표값 수정
        playerAnimator.bodyPosition = 
            Vector3.Lerp(playerAnimator.bodyPosition, 
                                  new Vector3(playerAnimator.bodyPosition.x,  playerAnimator.bodyPosition.y + differenceHeight, playerAnimator.bodyPosition.z),
                                  curGlobalWeight);


    }
}

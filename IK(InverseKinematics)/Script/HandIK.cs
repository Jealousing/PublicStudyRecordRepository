using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

/// <summary>
/// �÷��̾� ��IK(�����) Ŭ����
/// </summary>
public class HandIK : MonoBehaviour
{
    
    Animator playerAnimator;

    // ���簪
    Vector3 leftHandIKPositionBuffer;
    Vector3 rightHandIKPositionBuffer;
    Vector3 leftHandIKRotationBuffer;
    Vector3 rightHandIKRotationBuffer;

    // ��ǥ��
    [NonSerialized]
    public Vector3 leftHandIKPositionTarget;
    [NonSerialized]
    public Vector3 rightHandIKPositionTarget;
    [NonSerialized]
    public Vector3 leftHandIKRotationTarget;
    [NonSerialized]
    public Vector3 rightHandIKRotationTarget;

    // SmoothDamp�� ref Ű�������� ����
    Vector3 leftHandPositionLerpVelocity;
    Vector3 rightHandPositionLerpVelocity;
    Vector3 leftHandRotationLerpVelocity;
    Vector3 rightHandRotationLerpVelocity;

    // ��ǥ�� �����ϴ� �� �ɸ��� �뷫���� �ð�
    public float smoothTime;

    // ����ġ range(0, 1)
    [NonSerialized]
    public float leftPositionWeight;
    [NonSerialized]
    public float rightPositionWeight;
    [NonSerialized]
    public float leftRotationWeight;
    [NonSerialized]
    public float rightRotationWeight;

    // ���¿� ���� ����ġ ����
    float curGlobalWeight;


    // �۵� ����
    [SerializeField]
    private bool IsIKPosition;
    [SerializeField]
    private bool IsIKRotation;

    public bool IsBodyIK;

    // �տ�����Ʈ
    public GameObject leftHand;
    public GameObject rightHand;

    [NonSerialized]
    public bool isLeftHandReady;
    [NonSerialized]
    public bool isRightHandReady;

    // ���� ����
    float differenceHeight;

    // ���¿� ���� ��ȭ 
    private HandIkState state = HandIkState.NORMAL;

    // �ʱⰪ ���庯��
    Transform leftHandDefaultTransform;
    Transform rightHandDefaultTransform;

    // �ʱ�ȭ Flag
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
    /// �� ���� ����
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

    // ���� ȸ���� ��ü������ ���� ���ؼ� �� ������ �����ϱ� ���� �Լ�
    private void CreateOrientationReference()
    {
        //��ȣ�� ���
        if (leftHandDefaultTransform != null)
        {
            Destroy(leftHandDefaultTransform);
        }
        if (rightHandDefaultTransform != null)
        {
            Destroy(rightHandDefaultTransform);
        }

        // ���� ���ο� ������Ʈ�� ���� �⺻ ���Ⱚ�� ������Ų��.

        leftHandDefaultTransform = new GameObject("LeftHand_OrientationReference").transform;
        rightHandDefaultTransform = new GameObject("RightHand_OrientationReference").transform;

        leftHandDefaultTransform.position = leftHand.transform.position;
        rightHandDefaultTransform.position = rightHand.transform.position;

        leftHandDefaultTransform.SetParent(leftHand.transform);
        rightHandDefaultTransform.SetParent(rightHand.transform);

    }

    // ���� �ʱ�ȭ
    private void InitializeVariables()
    {
        playerAnimator = GetComponent<Animator>();
        // ù ��° �������� �ڿ������� ���̵��� ����.
        leftHandIKPositionBuffer = playerAnimator.GetIKPosition(AvatarIKGoal.LeftHand);
        rightHandIKPositionBuffer = playerAnimator.GetIKPosition(AvatarIKGoal.RightHand);
        leftHandIKPositionTarget = playerAnimator.GetIKPosition(AvatarIKGoal.LeftHand);
        rightHandIKPositionTarget = playerAnimator.GetIKPosition(AvatarIKGoal.RightHand);
    }


    // �� IK�� ��ǥ���� ������ �ڿ������� ��ȭ��Ű�� �Լ�
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
            // ���� �ʱ�ȭ
            InitializeVariables();
        }
        ApplyHandIK();
        ApplyBodyIK();
    }

    // �࿡ ���� �������� ����
    private void LerpByAxis(Vector3 currentPosition, ref Vector3 resultPosition, bool IsAxisX, bool IsAxisY, bool IsAxisZ)
    {
        // IsAxis ���� true�� �� ���� resultPosition���� �������� false�� current������ ����
        resultPosition = new Vector3
        (
            Mathf.Lerp(currentPosition.x, resultPosition.x, Convert.ToInt32(IsAxisX)),
            Mathf.Lerp(currentPosition.y, resultPosition.y, Convert.ToInt32(IsAxisY)),
            Mathf.Lerp(currentPosition.z, resultPosition.z, Convert.ToInt32(IsAxisZ))
        );
    }


    // ���� IK���� ������Ʈ
    private void ApplyHandIK()
    {
        // ��ġ �� ���� ����� �޼ո� ��� ���߿� ������ ����
        // IK�� �ִ� ���� ��ġ ������ ���� ���ۿ� ����ִ� ��ġ ������ �����Ѵ�.
        if (state == HandIkState.VAULT)
        {
            LerpByAxis(playerAnimator.GetIKPosition(AvatarIKGoal.LeftHand), ref leftHandIKPositionBuffer, false, true, false);
            LerpByAxis(playerAnimator.GetIKPosition(AvatarIKGoal.RightHand), ref rightHandIKPositionBuffer, false, true, false);
        }

        // globalWeight(����ġ)�� 1�� �������� IK�� ���� ���ϰ� ����ȴ�.
        playerAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, leftPositionWeight);
        playerAnimator.SetIKPositionWeight(AvatarIKGoal.RightHand, rightPositionWeight);

        if (IsIKPosition == true)
        {
            // ���ۿ� ����� ��ġ���� ����
            playerAnimator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandIKPositionBuffer);
            playerAnimator.SetIKPosition(AvatarIKGoal.RightHand, rightHandIKPositionBuffer);
        }
        Quaternion leftHandRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        Quaternion rightHandRotation = Quaternion.Euler(new Vector3(0, 0, 0));

        // ȸ�� �� ����
        if (leftHandIKRotationTarget != Vector3.zero)
            leftHandRotation = Quaternion.LookRotation(leftHandIKRotationTarget) * Quaternion.Euler(new Vector3(-90,0,0));

        if (rightHandIKRotationTarget != Vector3.zero)
             rightHandRotation = Quaternion.LookRotation(rightHandIKRotationTarget) * Quaternion.Euler(new Vector3(-90, 0, 0));

        playerAnimator.SetIKRotationWeight(AvatarIKGoal.LeftHand, leftRotationWeight);
        playerAnimator.SetIKRotationWeight(AvatarIKGoal.RightHand, rightRotationWeight);

        if (IsIKRotation == true)
        {
            // ȸ�� �� ����
            playerAnimator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandRotation);
            playerAnimator.SetIKRotation(AvatarIKGoal.RightHand, rightHandRotation);
        }

    }

    

    // ���� IK���� ������Ʈ
    private void ApplyBodyIK()
    {
        if (IsBodyIK == false)
        {
            return;
        }

        if(state == HandIkState.VAULT)
        {
            curGlobalWeight = Mathf.Lerp(curGlobalWeight, leftPositionWeight, Time.deltaTime * 10f);
            // ���� �� ���� ���� ��ġ�� ���
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

        // ���� ��ġ�� �ִ� ���� �������� ���� y��ǥ�� ����
        playerAnimator.bodyPosition = 
            Vector3.Lerp(playerAnimator.bodyPosition, 
                                  new Vector3(playerAnimator.bodyPosition.x,  playerAnimator.bodyPosition.y + differenceHeight, playerAnimator.bodyPosition.z),
                                  curGlobalWeight);


    }
}

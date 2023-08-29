using System;      
using UnityEngine; 

/// <summary>
/// ���� IK(�����) Ŭ����
/// </summary>
public class FootIK : MonoBehaviour
{
    // IK�� ���� �÷��̾�ִϸ����� ����
    Animator playerAnimator;

    // ���� Transform ����
    public Transform leftFootTransform;
    public Transform rightFootTransform;

    // �ʱⰪ ���庯��
    Transform leftFootDefaultTransform;
    Transform rightFootDefaultTransform;
    Vector3 initialForwardVector;

    /* Inspector â���� ������ ���� */
    // �� ����
    // �߹ٴ� ����
    [SerializeField]
    [Range(0, 0.25f)]
    float lengthFromHeelToToes = 0.1f;

    // �߸� ����
    [SerializeField]
    [Range(-0.05f, 0.125f)]
    float ankleHeightOffset = 0;

    // �ִ� ȸ�� ����
    [SerializeField]
    [Range(0, 60)]
    float maxRotationAngle = 45;

    // �ι��� �ִ� ���� ����
    [SerializeField]
    [Range(0, 1.0f)]
    float maxFootDistance = 0.35f;

    // IK ����
    // IK Ȱ��ȭ (�̵� ����)
    [SerializeField]
    bool IsIKPosition = true;
    [SerializeField]
    bool IsIKRotation = true;

    // ����ġ : 1�� �������� IK�� ���� ������
    [SerializeField]
    [Range(0, 1)]
    float globalWeight = 1;

    // IK SmoothDamp �Լ��� �ش� ���� �����ϴ� �ð�
    [SerializeField]
    [Range(0, 1.0f)]
    float smoothTime = 0.075f;

    // RayCast ����
    // SphereCast�� �� ������
    [SerializeField]
    [Range(0.001f, 0.1f)]
    float raySphereRadius = 0.05f;

    // ray�� �ִ� ����
    [SerializeField]
    [Range(0.1f, 2.0f)]
    float rayCastMaxDistance = 1;

    // �� ���̾� ���� (Default)
    public LayerMask groundLayers;

    // ray�� ���� ����
    [SerializeField]
    [Range(0.1f, 1.0f)]
    float leftFootRayStartHeight = 0.5f;
    [SerializeField]
    [Range(0.1f, 1.0f)]
    float rightFootRayStartHeight = 0.5f;

    // �ٵ� ������ Ȱ��ȭ
    [SerializeField]
    bool IsBodyIK = true;

    // ��ũ���� �ִ� ����
    [SerializeField]
    float crouchRange = 0.25f;

    // ���� ���� ������
    [SerializeField]
    float worldHeightOffset = 0;


    /* ������ ���̸� ����ϱ� ���� ����*/
    public RaycastHit leftFootRayHitInfo;
    public RaycastHit rightFootRayHitInfo;

    float leftFootRayHitHeight;
    float rightFootRayHitHeight;

    Vector3 leftFootRayStartPosition;
    Vector3 rightFootRayStartPosition;

    Vector3 leftFootDirectionVector;
    Vector3 rightFootDirectionVector;

    Vector3 leftFootProjectionVector;
    Vector3 rightFootProjectionVector;

    float leftFootProjectedAngle = 0;
    float rightFootProjectedAngle = 0;

    Vector3 leftFootRayHitProjectionVector;
    Vector3 rightFootRayHitProjectionVector;

    float leftFootRayHitProjectedAngle;
    float rightFootRayHitProjectedAngle;

    /* SmoothDamp�� �ڿ������� ��ȯ�ϱ� ���� ���ۿ� Ÿ�� ���� */
    Vector3 leftFootIKPositionBuffer;
    Vector3 rightFootIKPositionBuffer;

    Vector3 leftFootIKPositionTarget;
    Vector3 rightFootIKPositionTarget;

    Vector3 leftFootIKRotationBuffer;
    Vector3 rightFootIKRotationBuffer;

    Vector3 leftFootIKRotationTarget;
    Vector3 rightFootIKRotationTarget;


    // �ι� ������ ����
    float heightBetweenFeet = 0;

    /* Gizmos   */
    [SerializeField]
    bool IsGizmo;       // ����� Ȱ��ȭ ����
    bool isHitLeft;
    bool isHitRight;    

    /*  SmoothDamp ����ӵ� ����    */
    // ����
    float leftFootHeightLerpVelocity;
    float rightFootHeightLerpVelocity;
    // ȸ��
    Vector3 leftFootRotationLerpVelocity;
    Vector3 rightFootRotationLerpVelocity;
    // �ݶ��̴���ȯ
    Vector3 coliderCenterVelocity;
    float coliderHeightVelocity;

    CapsuleCollider capsuleCollider;

    /* Collider ���� ���� */
    Vector3 colliderCenterDefVal;
    Vector3 colliderCenterResultVal;
    float colliderHeightDefVal;

    public bool FootIKFlag = false;

    private void Start()
    {
        // ���� �ʱ�ȭ
        InitializeVariables();

        // �������� ����
        CreateOrientationReference();
    }



    private void Update()
    {
        if (!FootIKFlag) return;

        if (PlayerInfo.GetInstance.movementInfo.moveState.GetCurrentMoveState() == MoveState.DefaultMovement &&
            PlayerInfo.GetInstance.movementInfo.isGround &&
           playerAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Idle")
        {
            //�� ����
            UpdateFootProjection();

            // ray����
            UpdateRayHitInfo();

            // ��ǥ ȸ���� ������Ʈ
            UpdateIKPositionTarget();
            UpdateIKRotationTarget();

            // �� IK�� ��ǥ���� ������ �ڿ������� ��ȭ
            UpdateBuffer();
        }
    }


    // Ikó���� Coliderũ�� ����
    private void UpdateCollider()
    {
        // �� ���� ���� ����
        heightBetweenFeet = Mathf.Abs(leftFootIKPositionBuffer.y - rightFootIKPositionBuffer.y);

        if (heightBetweenFeet != 0 && heightBetweenFeet < maxFootDistance + raySphereRadius)
        {
            // �ݶ��̴� center�� ����
            colliderCenterResultVal.y = Mathf.Round(( colliderCenterDefVal.y+ heightBetweenFeet) * 1000f) / 1000f;  

            capsuleCollider.center = Vector3.SmoothDamp(capsuleCollider.center, colliderCenterResultVal, ref coliderCenterVelocity, smoothTime);

            // �ݶ��̴� ���� ����
            capsuleCollider.height = Mathf.SmoothDamp(capsuleCollider.height, Mathf.Round((colliderHeightDefVal - heightBetweenFeet) * 1000f) / 1000f , ref coliderHeightVelocity, smoothTime);
        }
        else
        {
            // �⺻������ �ǵ�����
            UnCollider();
        }
    }

    void UnCollider()
    {
        if (capsuleCollider.center != colliderCenterDefVal)
            capsuleCollider.center = Vector3.SmoothDamp(capsuleCollider.center, colliderCenterDefVal, ref coliderCenterVelocity, smoothTime);
        if (capsuleCollider.height != colliderHeightDefVal)
            capsuleCollider.height = Mathf.SmoothDamp(capsuleCollider.height, colliderHeightDefVal, ref coliderHeightVelocity, smoothTime);
    }

    // IK ������ ������Ʈ �Ǹ� ����Ǵ� �Լ�
    private void OnAnimatorIK()
    {
        if (!FootIKFlag) return;

        if (PlayerInfo.GetInstance.movementInfo.moveState.GetCurrentMoveState() == MoveState.DefaultMovement &&
             PlayerInfo.GetInstance.movementInfo.isGround &&
            playerAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Idle")
        {
            ApplyFootIK();
            ApplyBodyIK();
            UpdateCollider();
        }
        else
        {
            UnCollider();
        }
    }



    // ���� �ʱ�ȭ
    private void InitializeVariables()
    {
        playerAnimator = GetComponent<Animator>();

        leftFootTransform = playerAnimator.GetBoneTransform(HumanBodyBones.LeftFoot);
        rightFootTransform = playerAnimator.GetBoneTransform(HumanBodyBones.RightFoot);

        if (groundLayers.value == 0)
        {
            groundLayers = LayerMask.GetMask("Default");
        }

        // �� ���� ���� ���� ���͸� ��� ���� �ʿ� (��������)
        initialForwardVector = transform.forward;

        // ù ��° �������� �ڿ������� ���̵��� ����.
        leftFootIKPositionBuffer.y = transform.position.y + GetAnkleHeight();
        rightFootIKPositionBuffer.y = transform.position.y + GetAnkleHeight();

        // �ݶ��̴� ���� �ʱ�ȭ
        capsuleCollider = this.gameObject.GetComponent<CapsuleCollider>();
        colliderCenterResultVal = colliderCenterDefVal = capsuleCollider.center;
        colliderHeightDefVal = capsuleCollider.height;

    }

    

    // ���� ȸ���� ��ü������ ���� ���ؼ� �� ������ �����ϱ� ���� �Լ�
    private void CreateOrientationReference()
    {
        //��ȣ�� ���
        if (leftFootDefaultTransform != null)
        {
            Destroy(leftFootDefaultTransform);
        }
        if (rightFootDefaultTransform != null)
        {
            Destroy(rightFootDefaultTransform);
        }

        // �߿� ���ο� ������Ʈ�� ���� �⺻ ���Ⱚ�� ������Ų��.

        leftFootDefaultTransform = new GameObject("LeftFoot_OrientationReference").transform;
        rightFootDefaultTransform = new GameObject("RightFoot_OrientationReference").transform;

        leftFootDefaultTransform.position = leftFootTransform.position;
        rightFootDefaultTransform.position = rightFootTransform.position;

        leftFootDefaultTransform.SetParent(leftFootTransform);
        rightFootDefaultTransform.SetParent(rightFootTransform);
    }



    // ���� �������� ���ϴ��� Ȯ���ϴ� �Լ�
    private void UpdateFootProjection()
    {
        // �߹��� ����
        // ���� ���� ���� = �ʱ� ���� ���� * ���� �ִ� ������� ; ���͸� ���ʹϾ� ��ŭ ȸ��
        leftFootDirectionVector = leftFootDefaultTransform.rotation * initialForwardVector;
        rightFootDirectionVector = rightFootDefaultTransform.rotation * initialForwardVector;


        // �ٴڹ��� ���ϱ�
        //���� ���� ���� =   Vector3.ProjectOnPlane(���� ���⺤��, Vector3.up) ; ��鿡 ���� ���� ���͸� ����
        leftFootProjectionVector = Vector3.ProjectOnPlane(leftFootDirectionVector, Vector3.up);
        rightFootProjectionVector = Vector3.ProjectOnPlane(rightFootDirectionVector, Vector3.up);


        // ����� �������� ����
        // ���� ���� ���� = ���� ���� �� ������ ���� ���̸� ��������� ����
        leftFootProjectedAngle = Vector3.SignedAngle( leftFootProjectionVector, leftFootDirectionVector,
         /*������*/ Vector3.Cross(leftFootDirectionVector, leftFootProjectionVector));

        rightFootProjectedAngle = Vector3.SignedAngle(  rightFootProjectionVector,  rightFootDirectionVector,
            Vector3.Cross(rightFootDirectionVector, rightFootProjectionVector));

    }


    // ray���� ������Ʈ
    private void UpdateRayHitInfo()
    {
        // ray ���� ���� = ���� ��ġ + ���̽��� ����
        leftFootRayStartPosition = leftFootDefaultTransform.position;
        leftFootRayStartPosition.y += leftFootRayStartHeight;

        rightFootRayStartPosition = rightFootDefaultTransform.position;
        rightFootRayStartPosition.y += rightFootRayStartHeight;

        // SphereCast�� ���� ���� �������� ȸ����Ű�� ���� �ʿ��� ���� ���͸� ���ϱ� ���� ���ȴ�
        // ������ ���� ���͸� ������� vecter3.up ���
        isHitLeft =  Physics.SphereCast( leftFootRayStartPosition, raySphereRadius, Vector3.up * -1,  out leftFootRayHitInfo, rayCastMaxDistance , groundLayers);
        isHitRight = Physics.SphereCast( rightFootRayStartPosition, raySphereRadius,  Vector3.up * -1, out rightFootRayHitInfo, rayCastMaxDistance , groundLayers);

        // �޹� ������ ray ó��
        if (leftFootRayHitInfo.collider != null && rightFootRayHitInfo.point.y- leftFootRayHitInfo.point.y <= maxFootDistance )
        {
            leftFootRayHitHeight = leftFootRayHitInfo.point.y;
            
            
            // �ִϸ��̼����� �߻��ϴ� ȸ���� �����ϱ� ���� �ٴ����� ������ ������ ���
            // ray�� ���� ������Ʈ�� ���� ���Ϳ� ������ ������� �����Ѵ� (�߰� ������ ȸ���� ���)
            leftFootRayHitProjectionVector = Vector3.ProjectOnPlane(leftFootRayHitInfo.normal, Vector3.Cross(leftFootDirectionVector, leftFootProjectionVector));
            leftFootRayHitProjectedAngle = Vector3.Angle(leftFootRayHitProjectionVector, Vector3.up);

        }
        else
        {
            // �޹��� �� ������ ��� �������� ���̿� ����
            if (rightFootRayHitInfo.collider != null )
            {
                leftFootRayHitHeight = rightFootRayHitInfo.point.y;
            }
            else
            {
                leftFootRayHitHeight = transform.position.y;
            }
            
        }


        if (rightFootRayHitInfo.collider != null && leftFootRayHitInfo.point.y - rightFootRayHitInfo.point.y  <= maxFootDistance)
        {
            rightFootRayHitHeight = rightFootRayHitInfo.point.y;

            rightFootRayHitProjectionVector = Vector3.ProjectOnPlane(  rightFootRayHitInfo.normal, Vector3.Cross(rightFootDirectionVector, rightFootProjectionVector));
            rightFootRayHitProjectedAngle = Vector3.Angle( rightFootRayHitProjectionVector, Vector3.up);
        }
        else
        {
            if (leftFootRayHitInfo.collider != null )
            {
                rightFootRayHitHeight = leftFootRayHitInfo.point.y;
            }
            else
            {

                rightFootRayHitHeight = transform.position.y;
            }
        }
    }
  
    // Gizmo
    void OnDrawGizmos()
    {
        if (!IsGizmo) return;

        Gizmos.color = Color.red;
        if (isHitLeft)
        {
            Gizmos.DrawRay(leftFootRayStartPosition, Vector3.up * -1 * leftFootRayHitInfo.distance);
            Gizmos.DrawWireSphere(leftFootRayStartPosition + Vector3.up * -1 * leftFootRayHitInfo.distance, raySphereRadius);
        }
        else
        {
            Gizmos.DrawRay(leftFootRayStartPosition, Vector3.up * -1 * (rayCastMaxDistance));
        }
        if (isHitRight)
        {
            Gizmos.DrawRay(rightFootRayStartPosition, Vector3.up * -1 * rightFootRayHitInfo.distance);
            Gizmos.DrawWireSphere(rightFootRayStartPosition + Vector3.up * -1 * rightFootRayHitInfo.distance, raySphereRadius);
        }
        else
        {
            Gizmos.DrawRay(rightFootRayStartPosition, Vector3.up * -1 * (rayCastMaxDistance));
        }
    }

    // IK(��) ��ǥ�� update
    private void UpdateIKPositionTarget()
    {
        // ray�� ���� ����
        float leftFootHeightOffset = leftFootRayHitHeight;
        float rightFootHeightOffset = rightFootRayHitHeight;

        // ���� ������ ���� �� ���� ����

        // ������ ���������� ���� ���� �������� �޾ƿ´�
        leftFootHeightOffset += FootHeightOffset(leftFootRayHitProjectedAngle - leftFootProjectedAngle); 
        rightFootHeightOffset += FootHeightOffset(rightFootRayHitProjectedAngle - rightFootProjectedAngle);

        //�������
        leftFootHeightOffset += worldHeightOffset;
        rightFootHeightOffset += worldHeightOffset;

        //���� ��ġ ���� �κ�
        leftFootIKPositionTarget.y = leftFootHeightOffset;
        rightFootIKPositionTarget.y = rightFootHeightOffset;
    }


    private float FootHeightOffset(float projectedAngle)
    {
        float returnValue=0;
        // �����ؾߵǴ� ���� = sin�������� * �߹ٴ� ����
        returnValue += Mathf.Abs(Mathf.Sin(Mathf.Deg2Rad * projectedAngle) * lengthFromHeelToToes);
        // �߸���̿� ���� �߰����
        returnValue += Mathf.Cos(Mathf.Deg2Rad * projectedAngle) * GetAnkleHeight();
        
        return returnValue;
    }


    // IK(��) ȸ���� update
    private void UpdateIKRotationTarget()
    {
        // ��������
        float leftIntrpVar, rightIntrpVar;
        float temp;

        if (leftFootRayHitInfo.collider != null)
        {
            if ((temp = Vector3.Angle(transform.up, leftFootRayHitInfo.normal)) != 0)
            {
                // max���� �Ѵ� ȸ���� �߻��Ұ�� �׺��� ���� ȸ�����ϰ� ���� ���� �ڿ������� ����
                // 0�� 1���� ���� ���
                leftIntrpVar = Mathf.Clamp(temp, 0, maxRotationAngle) / temp;
            }
            else
            {
                leftIntrpVar = 0;
            }
            // ���� ���������� ���̰� ���� ������Ʈ ���� ������ �������͸� ������� �Ѵ�.
            leftFootIKRotationTarget = Vector3.Slerp(transform.up, leftFootRayHitInfo.normal, leftIntrpVar);
        }
        else
        {
            leftFootIKRotationTarget = transform.up;
        }

        if (rightFootRayHitInfo.collider != null)
        {
            if ((temp = Vector3.Angle(transform.up, rightFootRayHitInfo.normal)) != 0)
            {
                rightIntrpVar = Mathf.Clamp(temp, 0, maxRotationAngle) / temp;
            }
            else
            {
                rightIntrpVar = 0;
            }
            rightFootIKRotationTarget = Vector3.Slerp(transform.up, rightFootRayHitInfo.normal, rightIntrpVar);
        }
        else
        {
            rightFootIKRotationTarget = transform.up;
        }
    }


    // �� IK�� ��ǥ���� ������ �ڿ������� ��ȭ��Ű�� �Լ�
    private void UpdateBuffer()
    {
        leftFootIKPositionBuffer.y = 
            Mathf.SmoothDamp( leftFootIKPositionBuffer.y,   leftFootIKPositionTarget.y, ref leftFootHeightLerpVelocity, smoothTime);

        rightFootIKPositionBuffer.y = 
            Mathf.SmoothDamp( rightFootIKPositionBuffer.y,   rightFootIKPositionTarget.y, ref rightFootHeightLerpVelocity,  smoothTime);

        leftFootIKRotationBuffer = 
         Vector3.SmoothDamp( leftFootIKRotationBuffer,  leftFootIKRotationTarget, ref leftFootRotationLerpVelocity,  smoothTime);

        rightFootIKRotationBuffer =
            Vector3.SmoothDamp( rightFootIKRotationBuffer,  rightFootIKRotationTarget, ref rightFootRotationLerpVelocity,   smoothTime);

    }


    // ���� IK���� ������Ʈ
    private void ApplyFootIK()
    {
        // ��ġ �� ����
        // IK�� �ִ� ���� ��ġ ������ ���� ���ۿ� ����ִ� ��ġ ������ �����Ѵ�.
        LerpByAxis(playerAnimator.GetIKPosition(AvatarIKGoal.LeftFoot), ref leftFootIKPositionBuffer, false, true, false);
        LerpByAxis(playerAnimator.GetIKPosition(AvatarIKGoal.RightFoot), ref rightFootIKPositionBuffer, false, true, false);

        // globalWeight(����ġ)�� 1�� �������� IK�� ���� ���ϰ� ����ȴ�.
        playerAnimator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, globalWeight);
        playerAnimator.SetIKPositionWeight(AvatarIKGoal.RightFoot, globalWeight);

        if (IsIKPosition == true)
        {
            // ���ۿ� ����� ��ġ���� ����
            playerAnimator.SetIKPosition(AvatarIKGoal.LeftFoot, leftFootIKPositionBuffer);
            playerAnimator.SetIKPosition(AvatarIKGoal.RightFoot, rightFootIKPositionBuffer);
        }

        // ȸ�� �� ����
        // FromToRotation��  transform.up���� ���۷� ȸ���ϴ� ���ʹϾ� ���� ����
        Quaternion leftFootRotation =
            Quaternion.FromToRotation(transform.up, leftFootIKRotationBuffer) * playerAnimator.GetIKRotation(AvatarIKGoal.LeftFoot);

        Quaternion rightFootRotation =
            Quaternion.FromToRotation(transform.up, rightFootIKRotationBuffer) * playerAnimator.GetIKRotation(AvatarIKGoal.RightFoot);


        // ����ġ ����
        playerAnimator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, globalWeight);
        playerAnimator.SetIKRotationWeight(AvatarIKGoal.RightFoot, globalWeight);

        if (IsIKRotation == true)
        {
            // ȸ�� �� ����
            playerAnimator.SetIKRotation(AvatarIKGoal.LeftFoot, leftFootRotation);
            playerAnimator.SetIKRotation(AvatarIKGoal.RightFoot, rightFootRotation);
        }
    }

    private void ApplyFootIK(AvatarIKGoal foot)
    {
        Vector3 temp1;
        Vector3 temp2;
        if (foot == AvatarIKGoal.LeftFoot)
        {
            temp1 = leftFootIKPositionBuffer;
            temp2 = leftFootIKRotationTarget;
        }
        else
        {
            temp1 = rightFootIKPositionBuffer;
            temp2 = rightFootIKRotationBuffer;
        }

        // ��ġ �� ����
        // IK�� �ִ� ���� ��ġ ������ ���� ���ۿ� ����ִ� ��ġ ������ �����Ѵ�.
        LerpByAxis(playerAnimator.GetIKPosition(foot), ref temp1, false, true, false);

        // globalWeight(����ġ)�� 1�� �������� IK�� ���� ���ϰ� ����ȴ�.
        playerAnimator.SetIKPositionWeight(foot, globalWeight);

        if (IsIKPosition == true)
        {
            // ���ۿ� ����� ��ġ���� ����
            playerAnimator.SetIKPosition(foot, temp1);
        }

        // ȸ�� �� ����
        // FromToRotation��  transform.up���� ���۷� ȸ���ϴ� ���ʹϾ� ���� ����
        Quaternion leftFootRotation =
            Quaternion.FromToRotation(transform.up, temp2) * playerAnimator.GetIKRotation(foot);

        // ����ġ ����
        playerAnimator.SetIKRotationWeight(foot, globalWeight);

        if (IsIKRotation == true)
        {
            // ȸ�� �� ����
            playerAnimator.SetIKRotation(foot, leftFootRotation);
        }
    }


    // ���� IK���� ������Ʈ
    private void ApplyBodyIK()
    {
        if (IsBodyIK == false)
        {
            return;
        }
        // �� �Ʒ��� �ִ� ���� ��ǥ�� �ҷ�����
        float minFootHeight = Mathf.Min
        (
            playerAnimator.GetIKPosition(AvatarIKGoal.LeftFoot).y,
            playerAnimator.GetIKPosition(AvatarIKGoal.RightFoot).y
        );
        // ���� ��ġ�� �ִ� ���� �������� ���� y��ǥ�� ����
        playerAnimator.bodyPosition = new Vector3
        (
            playerAnimator.bodyPosition.x,
            playerAnimator.bodyPosition.y + ValueLimit(minFootHeight - transform.position.y, 0, crouchRange),
            playerAnimator.bodyPosition.z
        );
    }

    // �߸���� �ҷ�����
    private float GetAnkleHeight()
    {
        return raySphereRadius + ankleHeightOffset;
    }


    //�࿡ ���� �������� ����
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


    // ������ ����� ���� ����
    private float ValueLimit(float value, float mid, float range)
    {
        if (value < mid - range / 2f)
        {
            return value + range / 2f ;
        }
        else if (value > mid + range / 2f)
        {
            return value - range / 2f ;
        }
        else
        {
            return mid;
        }
    }

}


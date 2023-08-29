using System;      
using UnityEngine; 

/// <summary>
/// 발의 IK(역운동학) 클래스
/// </summary>
public class FootIK : MonoBehaviour
{
    // IK를 위한 플레이어애니메이터 정보
    Animator playerAnimator;

    // 발의 Transform 정보
    public Transform leftFootTransform;
    public Transform rightFootTransform;

    // 초기값 저장변수
    Transform leftFootDefaultTransform;
    Transform rightFootDefaultTransform;
    Vector3 initialForwardVector;

    /* Inspector 창에서 수정할 값들 */
    // 발 관련
    // 발바닥 길이
    [SerializeField]
    [Range(0, 0.25f)]
    float lengthFromHeelToToes = 0.1f;

    // 발목 높이
    [SerializeField]
    [Range(-0.05f, 0.125f)]
    float ankleHeightOffset = 0;

    // 최대 회전 각도
    [SerializeField]
    [Range(0, 60)]
    float maxRotationAngle = 45;

    // 두발의 최대 높이 차이
    [SerializeField]
    [Range(0, 1.0f)]
    float maxFootDistance = 0.35f;

    // IK 관련
    // IK 활성화 (이동 각도)
    [SerializeField]
    bool IsIKPosition = true;
    [SerializeField]
    bool IsIKRotation = true;

    // 가중치 : 1에 가까울수록 IK의 힘이 강해짐
    [SerializeField]
    [Range(0, 1)]
    float globalWeight = 1;

    // IK SmoothDamp 함수의 해당 값에 도달하는 시간
    [SerializeField]
    [Range(0, 1.0f)]
    float smoothTime = 0.075f;

    // RayCast 관련
    // SphereCast의 구 반지름
    [SerializeField]
    [Range(0.001f, 0.1f)]
    float raySphereRadius = 0.05f;

    // ray의 최대 길이
    [SerializeField]
    [Range(0.1f, 2.0f)]
    float rayCastMaxDistance = 1;

    // 땅 레이어 설정 (Default)
    public LayerMask groundLayers;

    // ray의 시작 높이
    [SerializeField]
    [Range(0.1f, 1.0f)]
    float leftFootRayStartHeight = 0.5f;
    [SerializeField]
    [Range(0.1f, 1.0f)]
    float rightFootRayStartHeight = 0.5f;

    // 바디 움직임 활성화
    [SerializeField]
    bool IsBodyIK = true;

    // 웅크리고 있는 범위
    [SerializeField]
    float crouchRange = 0.25f;

    // 세계 높이 오프셋
    [SerializeField]
    float worldHeightOffset = 0;


    /* 각도와 높이를 계산하기 위한 변수*/
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

    /* SmoothDamp로 자연스럽게 변환하기 위한 버퍼와 타겟 변수 */
    Vector3 leftFootIKPositionBuffer;
    Vector3 rightFootIKPositionBuffer;

    Vector3 leftFootIKPositionTarget;
    Vector3 rightFootIKPositionTarget;

    Vector3 leftFootIKRotationBuffer;
    Vector3 rightFootIKRotationBuffer;

    Vector3 leftFootIKRotationTarget;
    Vector3 rightFootIKRotationTarget;


    // 두발 사이의 높이
    float heightBetweenFeet = 0;

    /* Gizmos   */
    [SerializeField]
    bool IsGizmo;       // 기즈모 활성화 여부
    bool isHitLeft;
    bool isHitRight;    

    /*  SmoothDamp 현재속도 변수    */
    // 높이
    float leftFootHeightLerpVelocity;
    float rightFootHeightLerpVelocity;
    // 회전
    Vector3 leftFootRotationLerpVelocity;
    Vector3 rightFootRotationLerpVelocity;
    // 콜라이더변환
    Vector3 coliderCenterVelocity;
    float coliderHeightVelocity;

    CapsuleCollider capsuleCollider;

    /* Collider 조정 관련 */
    Vector3 colliderCenterDefVal;
    Vector3 colliderCenterResultVal;
    float colliderHeightDefVal;

    public bool FootIKFlag = false;

    private void Start()
    {
        // 변수 초기화
        InitializeVariables();

        // 방향참조 생성
        CreateOrientationReference();
    }



    private void Update()
    {
        if (!FootIKFlag) return;

        if (PlayerInfo.GetInstance.movementInfo.moveState.GetCurrentMoveState() == MoveState.DefaultMovement &&
            PlayerInfo.GetInstance.movementInfo.isGround &&
           playerAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Idle")
        {
            //발 투영
            UpdateFootProjection();

            // ray정보
            UpdateRayHitInfo();

            // 좌표 회전값 업데이트
            UpdateIKPositionTarget();
            UpdateIKRotationTarget();

            // 발 IK의 좌표값과 각도를 자연스럽게 변화
            UpdateBuffer();
        }
    }


    // Ik처리시 Colider크기 보정
    private void UpdateCollider()
    {
        // 두 발의 높이 차이
        heightBetweenFeet = Mathf.Abs(leftFootIKPositionBuffer.y - rightFootIKPositionBuffer.y);

        if (heightBetweenFeet != 0 && heightBetweenFeet < maxFootDistance + raySphereRadius)
        {
            // 콜라이더 center값 보정
            colliderCenterResultVal.y = Mathf.Round(( colliderCenterDefVal.y+ heightBetweenFeet) * 1000f) / 1000f;  

            capsuleCollider.center = Vector3.SmoothDamp(capsuleCollider.center, colliderCenterResultVal, ref coliderCenterVelocity, smoothTime);

            // 콜라이더 높이 보정
            capsuleCollider.height = Mathf.SmoothDamp(capsuleCollider.height, Mathf.Round((colliderHeightDefVal - heightBetweenFeet) * 1000f) / 1000f , ref coliderHeightVelocity, smoothTime);
        }
        else
        {
            // 기본값으로 되돌리기
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

    // IK 정보가 업데이트 되면 실행되는 함수
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



    // 변수 초기화
    private void InitializeVariables()
    {
        playerAnimator = GetComponent<Animator>();

        leftFootTransform = playerAnimator.GetBoneTransform(HumanBodyBones.LeftFoot);
        rightFootTransform = playerAnimator.GetBoneTransform(HumanBodyBones.RightFoot);

        if (groundLayers.value == 0)
        {
            groundLayers = LayerMask.GetMask("Default");
        }

        // 각 발의 최종 방향 벡터를 얻기 위해 필요 (선형보간)
        initialForwardVector = transform.forward;

        // 첫 번째 프레임이 자연스럽게 보이도록 설정.
        leftFootIKPositionBuffer.y = transform.position.y + GetAnkleHeight();
        rightFootIKPositionBuffer.y = transform.position.y + GetAnkleHeight();

        // 콜라이더 변수 초기화
        capsuleCollider = this.gameObject.GetComponent<CapsuleCollider>();
        colliderCenterResultVal = colliderCenterDefVal = capsuleCollider.center;
        colliderHeightDefVal = capsuleCollider.height;

    }

    

    // 발의 회전을 자체적으로 하지 못해서 뼈 방향을 추적하기 위한 함수
    private void CreateOrientationReference()
    {
        //재호출 대비
        if (leftFootDefaultTransform != null)
        {
            Destroy(leftFootDefaultTransform);
        }
        if (rightFootDefaultTransform != null)
        {
            Destroy(rightFootDefaultTransform);
        }

        // 발에 새로운 오브젝트를 만들어서 기본 방향값을 유지시킨다.

        leftFootDefaultTransform = new GameObject("LeftFoot_OrientationReference").transform;
        rightFootDefaultTransform = new GameObject("RightFoot_OrientationReference").transform;

        leftFootDefaultTransform.position = leftFootTransform.position;
        rightFootDefaultTransform.position = rightFootTransform.position;

        leftFootDefaultTransform.SetParent(leftFootTransform);
        rightFootDefaultTransform.SetParent(rightFootTransform);
    }



    // 발이 땅밑으로 향하는지 확인하는 함수
    private void UpdateFootProjection()
    {
        // 발방향 참조
        // 발의 방향 벡터 = 초기 발의 각도 * 보고 있는 방향백터 ; 벡터를 쿼터니언값 만큼 회전
        leftFootDirectionVector = leftFootDefaultTransform.rotation * initialForwardVector;
        rightFootDirectionVector = rightFootDefaultTransform.rotation * initialForwardVector;


        // 바닥방향 구하기
        //발의 투영 벡터 =   Vector3.ProjectOnPlane(발의 방향벡터, Vector3.up) ; 평면에 발의 방향 벡터를 투영
        leftFootProjectionVector = Vector3.ProjectOnPlane(leftFootDirectionVector, Vector3.up);
        rightFootProjectionVector = Vector3.ProjectOnPlane(rightFootDirectionVector, Vector3.up);


        // 양수가 나오도록 설정
        // 발의 투영 각도 = 위에 구한 두 벡터의 각도 차이를 축기준으로 측정
        leftFootProjectedAngle = Vector3.SignedAngle( leftFootProjectionVector, leftFootDirectionVector,
         /*기준축*/ Vector3.Cross(leftFootDirectionVector, leftFootProjectionVector));

        rightFootProjectedAngle = Vector3.SignedAngle(  rightFootProjectionVector,  rightFootDirectionVector,
            Vector3.Cross(rightFootDirectionVector, rightFootProjectionVector));

    }


    // ray정보 업데이트
    private void UpdateRayHitInfo()
    {
        // ray 시작 위지 = 각발 위치 + 레이시작 높이
        leftFootRayStartPosition = leftFootDefaultTransform.position;
        leftFootRayStartPosition.y += leftFootRayStartHeight;

        rightFootRayStartPosition = rightFootDefaultTransform.position;
        rightFootRayStartPosition.y += rightFootRayStartHeight;

        // SphereCast는 발을 다음 방향으로 회전시키기 위해 필요한 법선 벡터를 구하기 위해 사용된다
        // 월드의 법선 벡터를 얻기위해 vecter3.up 사용
        isHitLeft =  Physics.SphereCast( leftFootRayStartPosition, raySphereRadius, Vector3.up * -1,  out leftFootRayHitInfo, rayCastMaxDistance , groundLayers);
        isHitRight = Physics.SphereCast( rightFootRayStartPosition, raySphereRadius,  Vector3.up * -1, out rightFootRayHitInfo, rayCastMaxDistance , groundLayers);

        // 왼발 오른발 ray 처리
        if (leftFootRayHitInfo.collider != null && rightFootRayHitInfo.point.y- leftFootRayHitInfo.point.y <= maxFootDistance )
        {
            leftFootRayHitHeight = leftFootRayHitInfo.point.y;
            
            
            // 애니메이션으로 발생하는 회전을 제외하기 위해 바닥으로 부터의 각도로 계산
            // ray가 닿은 오브젝트의 법선 벡터에 외적한 결과값을 투영한다 (발과 평행한 회전만 계산)
            leftFootRayHitProjectionVector = Vector3.ProjectOnPlane(leftFootRayHitInfo.normal, Vector3.Cross(leftFootDirectionVector, leftFootProjectionVector));
            leftFootRayHitProjectedAngle = Vector3.Angle(leftFootRayHitProjectionVector, Vector3.up);

        }
        else
        {
            // 왼발이 붕 떠있을 경우 오른발의 높이와 맞춤
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

    // IK(발) 좌표값 update
    private void UpdateIKPositionTarget()
    {
        // ray가 닿은 지점
        float leftFootHeightOffset = leftFootRayHitHeight;
        float rightFootHeightOffset = rightFootRayHitHeight;

        // 투사 각도에 따른 발 높이 조정

        // 각발의 투영각도에 따른 높이 설정값을 받아온다
        leftFootHeightOffset += FootHeightOffset(leftFootRayHitProjectedAngle - leftFootProjectedAngle); 
        rightFootHeightOffset += FootHeightOffset(rightFootRayHitProjectedAngle - rightFootProjectedAngle);

        //월드높이
        leftFootHeightOffset += worldHeightOffset;
        rightFootHeightOffset += worldHeightOffset;

        //계산된 위치 적용 부분
        leftFootIKPositionTarget.y = leftFootHeightOffset;
        rightFootIKPositionTarget.y = rightFootHeightOffset;
    }


    private float FootHeightOffset(float projectedAngle)
    {
        float returnValue=0;
        // 조정해야되는 높이 = sin투영각도 * 발바닥 길이
        returnValue += Mathf.Abs(Mathf.Sin(Mathf.Deg2Rad * projectedAngle) * lengthFromHeelToToes);
        // 발목높이에 대한 추가계산
        returnValue += Mathf.Cos(Mathf.Deg2Rad * projectedAngle) * GetAnkleHeight();
        
        return returnValue;
    }


    // IK(발) 회전값 update
    private void UpdateIKRotationTarget()
    {
        // 보간변수
        float leftIntrpVar, rightIntrpVar;
        float temp;

        if (leftFootRayHitInfo.collider != null)
        {
            if ((temp = Vector3.Angle(transform.up, leftFootRayHitInfo.normal)) != 0)
            {
                // max값을 넘는 회전이 발생할경우 그보다 적은 회전을하게 만들어서 더욱 자연스럽게 연출
                // 0과 1사이 값을 출력
                leftIntrpVar = Mathf.Clamp(temp, 0, maxRotationAngle) / temp;
            }
            else
            {
                leftIntrpVar = 0;
            }
            // 구면 보간법으로 레이가 맞은 오브젝트 면의 수직인 법선벡터를 대상으로 한다.
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


    // 발 IK의 좌표값과 각도를 자연스럽게 변화시키는 함수
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


    // 발의 IK정보 업데이트
    private void ApplyFootIK()
    {
        // 위치 값 적용
        // IK에 있는 손의 위치 정보를 토대로 버퍼에 담겨있는 위치 정보를 수정한다.
        LerpByAxis(playerAnimator.GetIKPosition(AvatarIKGoal.LeftFoot), ref leftFootIKPositionBuffer, false, true, false);
        LerpByAxis(playerAnimator.GetIKPosition(AvatarIKGoal.RightFoot), ref rightFootIKPositionBuffer, false, true, false);

        // globalWeight(가중치)가 1에 가까울수록 IK가 더욱 강하게 적용된다.
        playerAnimator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, globalWeight);
        playerAnimator.SetIKPositionWeight(AvatarIKGoal.RightFoot, globalWeight);

        if (IsIKPosition == true)
        {
            // 버퍼에 저장된 위치값을 적용
            playerAnimator.SetIKPosition(AvatarIKGoal.LeftFoot, leftFootIKPositionBuffer);
            playerAnimator.SetIKPosition(AvatarIKGoal.RightFoot, rightFootIKPositionBuffer);
        }

        // 회전 값 적용
        // FromToRotation은  transform.up에서 버퍼로 회전하는 쿼터니언 값을 추출
        Quaternion leftFootRotation =
            Quaternion.FromToRotation(transform.up, leftFootIKRotationBuffer) * playerAnimator.GetIKRotation(AvatarIKGoal.LeftFoot);

        Quaternion rightFootRotation =
            Quaternion.FromToRotation(transform.up, rightFootIKRotationBuffer) * playerAnimator.GetIKRotation(AvatarIKGoal.RightFoot);


        // 가중치 적용
        playerAnimator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, globalWeight);
        playerAnimator.SetIKRotationWeight(AvatarIKGoal.RightFoot, globalWeight);

        if (IsIKRotation == true)
        {
            // 회전 값 적용
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

        // 위치 값 적용
        // IK에 있는 발의 위치 정보를 토대로 버퍼에 담겨있는 위치 정보를 수정한다.
        LerpByAxis(playerAnimator.GetIKPosition(foot), ref temp1, false, true, false);

        // globalWeight(가중치)가 1에 가까울수록 IK가 더욱 강하게 적용된다.
        playerAnimator.SetIKPositionWeight(foot, globalWeight);

        if (IsIKPosition == true)
        {
            // 버퍼에 저장된 위치값을 적용
            playerAnimator.SetIKPosition(foot, temp1);
        }

        // 회전 값 적용
        // FromToRotation은  transform.up에서 버퍼로 회전하는 쿼터니언 값을 추출
        Quaternion leftFootRotation =
            Quaternion.FromToRotation(transform.up, temp2) * playerAnimator.GetIKRotation(foot);

        // 가중치 적용
        playerAnimator.SetIKRotationWeight(foot, globalWeight);

        if (IsIKRotation == true)
        {
            // 회전 값 적용
            playerAnimator.SetIKRotation(foot, leftFootRotation);
        }
    }


    // 몸의 IK정보 업데이트
    private void ApplyBodyIK()
    {
        if (IsBodyIK == false)
        {
            return;
        }
        // 더 아래에 있는 발의 좌표값 불러오기
        float minFootHeight = Mathf.Min
        (
            playerAnimator.GetIKPosition(AvatarIKGoal.LeftFoot).y,
            playerAnimator.GetIKPosition(AvatarIKGoal.RightFoot).y
        );
        // 낮은 위치에 있는 발을 기준으로 몸의 y좌표값 수정
        playerAnimator.bodyPosition = new Vector3
        (
            playerAnimator.bodyPosition.x,
            playerAnimator.bodyPosition.y + ValueLimit(minFootHeight - transform.position.y, 0, crouchRange),
            playerAnimator.bodyPosition.z
        );
    }

    // 발목높이 불러오기
    private float GetAnkleHeight()
    {
        return raySphereRadius + ankleHeightOffset;
    }


    //축에 따른 선형보간 진행
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


    // 범위에 벗어나는 값을 보정
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


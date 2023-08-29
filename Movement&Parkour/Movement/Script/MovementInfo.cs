using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


/// <summary>
/// 움직임에 필요한 정보를 가지고 있는 클래스
/// </summary>
public class MovementInfo : MonoBehaviour
{
    /// <summary>
    /// 오브젝트를 감지하는 콜라이더 구조체
    /// </summary>
    [Serializable]
    public struct DetectObject
    {
        public DetectObj detectGround;
        public DetectObj detectVault;
        public DetectObj detectVaultLimit;
        public DetectObj detectClimb;
        public DetectObj detectClimbLimit;
        public DetectObj detectWallL;
        public DetectObj detectWallR;
    }
    public DetectObject detectObject; 

    // 움직임상태관리
    public MoveStateSystem moveState; 

    /*---- 변수 모음 ----*/


    #region Animator Variables 
    [NonSerialized] public Animator animator;
    [NonSerialized] public bool isAnimator;
    [NonSerialized] public int aniHashSpeed;
    [NonSerialized] public int aniHashGrounded;
    [NonSerialized] public int aniHashJump;
    [NonSerialized] public int aniHashFall;
    [NonSerialized] public int aniHashFallTime;
    [NonSerialized] public int aniHashHAxis;
    [NonSerialized] public int aniHashVAxis;
    [NonSerialized] public int aniHashClimbing;
    [NonSerialized] public int aniHashClimbingHook;
    [NonSerialized] public int aniHashClimbingEndUp;
    [NonSerialized] public int aniHashClimbingEndJump;
    [NonSerialized] public int aniHashClimbingSpeed;
    [NonSerialized] public int aniHashVault;
    [NonSerialized] public int aniHashClimbingEndDown;
    [NonSerialized] public int aniHashWallRunStartL;
    [NonSerialized] public int aniHashWallRunStartR;
    [NonSerialized] public int aniHashWallRunEnd;
    [NonSerialized] public int aniHashDropItemR;
    [NonSerialized] public int aniHashDropItemL;
    [NonSerialized] public int aniHashTakeItem;
    public string LandingTag = "Landing";
    #endregion;


    float hAxis; // Horizontal
    float vAxis; // Vertical
    [NonSerialized]
    public Vector2 moveVec; // 움직임
    public float currentSpeed; // 현재속도
    public Rigidbody player_rigidbody;
    public CapsuleCollider player_collider;

    public float gravity = -9.81f;                     // 중력값
    public float fallTimeout = 0.15f;                // 다른상태에서 떨어지는 상태로 변환될때 까지의 걸리는 시간
    public float fallTimeoutDelta;                  
    public float terminalVelocity = 53.0f;     // 종단속도
    public float fallTime;

    public bool isGround = true;
    public bool inputLock = false;

    // IK 정보
    [NonSerialized] public HandIK handIK;
    [NonSerialized] public FootIK footIK;

    // 카메라 정보
    public GameObject mainCamera;

    private void Awake()
    {
        if (mainCamera == null)
        {
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }

        // 애니메이션 헤시코드로 저장
        aniHashSpeed = Animator.StringToHash("Speed");
        aniHashGrounded = Animator.StringToHash("Grounded");
        aniHashJump = Animator.StringToHash("Jump");
        aniHashFall = Animator.StringToHash("FreeFall");
        aniHashFallTime = Animator.StringToHash("FallTime");
        aniHashHAxis = Animator.StringToHash("hAxis");
        aniHashVAxis = Animator.StringToHash("vAxis");
        aniHashClimbing = Animator.StringToHash("ClimbingStart");
        aniHashClimbingHook = Animator.StringToHash("ClimbingHook");
        aniHashClimbingEndUp = Animator.StringToHash("ClimbingEndUp");
        aniHashClimbingEndDown = Animator.StringToHash("ClimbingEndDown");
        aniHashClimbingEndJump = Animator.StringToHash("ClimbingEndJump");
        aniHashClimbingSpeed = Animator.StringToHash("ClimbingSpeed");
        aniHashWallRunStartL = Animator.StringToHash("WallRunStartL");
        aniHashWallRunStartR = Animator.StringToHash("WallRunStartR");
        aniHashWallRunEnd = Animator.StringToHash("WallRunEnd");
        aniHashVault = Animator.StringToHash("Vault");
        aniHashDropItemR = Animator.StringToHash("DropItemR");
        aniHashDropItemL = Animator.StringToHash("DropItemL");
        aniHashTakeItem = Animator.StringToHash("TakeItem");
    }
    private void Start()
    {
        player_rigidbody=GetComponent<Rigidbody>();
        player_collider = GetComponent<CapsuleCollider>();
        handIK = GetComponent<HandIK>();
        footIK = GetComponent<FootIK>();
        moveState = GetComponent<MoveStateSystem>();
        isAnimator = TryGetComponent(out animator);
    }

    private void Update()
    {
        if (inputLock)
        {
            moveVec = Vector2.zero;
            return;
        }

        // 입력받기
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");

        // 움직임 벡터 설정
        moveVec = new Vector2(hAxis, vAxis);

        animator.SetFloat(aniHashHAxis, moveVec.x);
        animator.SetFloat(aniHashVAxis, moveVec.y);

    }


    /// <summary>
    /// 플레이어가 땅에 닿아 있는지 확인하는 함수
    /// </summary>
    public void GroundedCheck()
    {
        // 방법1) 구에 닿는 오브젝트가 있는지 확인
        // Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - groundOffset, transform.position.z);
        // isGround = Physics.CheckSphere(spherePosition, groundCheckRadius, groundLayer, QueryTriggerInteraction.Ignore);

        // 방법2)
        if (detectObject.detectGround.IsDetect)
        {
            isGround = true;
        }
        else
        {
            isGround = false;
        }
        if (isAnimator)
        {
            animator.SetBool(aniHashGrounded, isGround);
        }
    }

    public void StopRigidbodyForTime(float time)
    {
        StartCoroutine(StopRigidbodyForTimeCoroutine(time));
    }

    private IEnumerator StopRigidbodyForTimeCoroutine(float time)
    {
        PlayerInfo.GetInstance.movementInfo.player_rigidbody.useGravity = false;
        PlayerInfo.GetInstance.movementInfo.player_rigidbody.isKinematic = true;

        yield return new WaitForSeconds(time);

        PlayerInfo.GetInstance.movementInfo.player_rigidbody.useGravity = true;
        PlayerInfo.GetInstance.movementInfo.player_rigidbody.isKinematic = false;
    }

}

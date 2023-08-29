using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


/// <summary>
/// �����ӿ� �ʿ��� ������ ������ �ִ� Ŭ����
/// </summary>
public class MovementInfo : MonoBehaviour
{
    /// <summary>
    /// ������Ʈ�� �����ϴ� �ݶ��̴� ����ü
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

    // �����ӻ��°���
    public MoveStateSystem moveState; 

    /*---- ���� ���� ----*/


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
    public Vector2 moveVec; // ������
    public float currentSpeed; // ����ӵ�
    public Rigidbody player_rigidbody;
    public CapsuleCollider player_collider;

    public float gravity = -9.81f;                     // �߷°�
    public float fallTimeout = 0.15f;                // �ٸ����¿��� �������� ���·� ��ȯ�ɶ� ������ �ɸ��� �ð�
    public float fallTimeoutDelta;                  
    public float terminalVelocity = 53.0f;     // ���ܼӵ�
    public float fallTime;

    public bool isGround = true;
    public bool inputLock = false;

    // IK ����
    [NonSerialized] public HandIK handIK;
    [NonSerialized] public FootIK footIK;

    // ī�޶� ����
    public GameObject mainCamera;

    private void Awake()
    {
        if (mainCamera == null)
        {
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }

        // �ִϸ��̼� ����ڵ�� ����
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

        // �Է¹ޱ�
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");

        // ������ ���� ����
        moveVec = new Vector2(hAxis, vAxis);

        animator.SetFloat(aniHashHAxis, moveVec.x);
        animator.SetFloat(aniHashVAxis, moveVec.y);

    }


    /// <summary>
    /// �÷��̾ ���� ��� �ִ��� Ȯ���ϴ� �Լ�
    /// </summary>
    public void GroundedCheck()
    {
        // ���1) ���� ��� ������Ʈ�� �ִ��� Ȯ��
        // Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - groundOffset, transform.position.z);
        // isGround = Physics.CheckSphere(spherePosition, groundCheckRadius, groundLayer, QueryTriggerInteraction.Ignore);

        // ���2)
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

using System;
using System.Collections;
using UnityEngine;

public class Player_Control : Singleton_Mono<Player_Control> //싱글톤 적용
{
    // 외부에서 생성되지 않도록 생성자 protected 선언
    protected Player_Control() { }

    #region 변수모음

    //에디터 상에서 수정하거나 테스트 용도로 확인용 public
    [Header("[현재 속도]")] public float m_MoveSpeed = 1f;
    [Header("[시작 속도]")] public float m_StartSpeed = 1f;
    [Header("[속도증가량]")] public float m_SpeedMultiply = 0.5f;
    [Header("[속도감소량]")] public float m_SpeedDivision = 1f;
    [Header("[설정된 한계 속도]")] public float m_MaxSpeed = 0f;
    [Header("[앉기 속도]")] public float m_CrouchSpeed = 2f;
    [Header("[걷기 속도]")] public float m_WalkSpeed = 5f;
    [Header("[뛰는 속도]")] public float m_RunnigSpeed = 10f;
    [Header("[회전 속도]")] public float m_RotateSpeed = 10f;
    [Header("[가해지는 점프력]")] public float m_JumpPower = 5f;
    [Header("[플레이어 상태]")] public E_PlayerState m_PlayerState;
    [Header("[임시 저장된 상태]")] public E_PlayerState m_PlayerStateSave;
    [Header("[키 입력방지]")] public bool KeyBlock;
    [Header("[점프중인지 확인용]")] public bool m_IsJump;

    //player 이동에 대한 지역변수 상하좌우 (-1,0,+1)
    [Header("[좌우 입력 확인]")] public float m_horizontalMove;
    [Header("[상하 입력 확인]")] public float m_verticalMove;

    //플레이어 리지드바디
    [Header("[플레이어 Rigidbody]")] public Rigidbody m_Player_rigidbody;
    //플레이어 애니메이션 스크립트
    [Header("[플레이어 Ani 스크립트]")] public Player_Animation m_Animation;

    [Range(0, 3)]
    [Header("[점프 레이케스트 거리]")] public float m_DownDistance;
    [Range(0, 3)]
    [Header("[점프 레이케스트간의 거리]")] public float m_BetweenDistance;
    [Range(0, 3)]
    [Header("[착지 동작 출력시작할 거리]")] public float m_LandingDistance;
    [Header("[낙하 출력 거리]")] public float m_FallingDistance;
    [Header("[플레이어 콜라이더]")] public CapsuleCollider m_playerCollider;

    //private
    //이동에 관한 처리용 변수
    private Vector3 m_Movement;
    //매쉬 랜더용
    private SkinnedMeshRenderer m_meshRenderer;
    //플레이어 애니메이터
    private Animator m_playerAni;
    //레이케스트용 변수
    private RaycastHit m_hit_down;
    //낙하상태 확인
    private bool m_IsFalling = false;

    #endregion;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    //기본 세팅
    void Start()
    {
        m_Animation = GetComponentInChildren<Player_Animation>();
        m_meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        KeyBlock = false;
        m_MoveSpeed = m_StartSpeed;
        m_MaxSpeed = m_WalkSpeed;
        m_Player_rigidbody = GetComponent<Rigidbody>();
        m_PlayerState = E_PlayerState.Idle;
        m_playerAni = GetComponent<Animator>();
        m_HoldingStateCollider.enabled = false;
    }


    void Update()
    {
        if (m_PlayerState == E_PlayerState.Die || m_isInteraction)
        {
            return;
        }
        LandingCheck();
        InputKey(); // 키입력 처리
    }

    void FixedUpdate()
    {
        // 플레이어 사망 or 상호작용 
        if (m_PlayerState == E_PlayerState.Die ||
            m_PlayerState == E_PlayerState.Interaction || m_isInteraction)
        {
            return;
        }
        //물리 처리 부분
        Moveing();
        Turn();
    }



    // 작성자: 서동주(이동관련), 조현진(상호작용관련)
    void InputKey()
    {
        //키막음
        if(KeyBlock)
        {
            return;
        }

        //상하좌우 키입력 받아옴
        m_horizontalMove = Input.GetAxisRaw("Horizontal");
        m_verticalMove = Input.GetAxisRaw("Vertical");

        // 상호 작용 상태 에서의 키입력 처리
        if (m_PlayerState == E_PlayerState.Interaction)
        {
            if (Input.GetKeyDown(KeyCode.E) && m_InteractionState != E_InteraciontState.RopeHanging) // 로프 점프 E키 제외
            {
                if (!m_isInteraction) // 상호작용이 진행중(애니메이션)이면 해제 못하도록 설정
                {
                    if (m_Object.GetType() == typeof(MoveBox))
                    {
                        MoveBox tempBox = m_Object.GetComponent<MoveBox>();
                        tempBox.MoveBoxRelease();
                    }
                    else if(m_Object.GetType() == typeof(LightObject))
                    {
                        LightObject tempObj = m_Object.GetComponent<LightObject>();
                        tempObj.LightObjectRelease();
                    }
                    InteractionRelease(); // 상호작용 해제
                }
            }

            if (m_InteractionObj != null && !m_isInteraction)
            {
                switch (m_InteractionObj.GetComponent<InteractableObject>().m_ObjectType) // 상호작용 중인 오브젝트의 타입으로 분류
                {
                    case E_ObjectType.Fixture_Box: // 고정된 상자
                        if (m_isArrivePoint)       // 상호작용 지점에 도착하면 상태 변경
                        {
                            ArriveInteractionPointProcess(E_InteraciontState.Riding);
                            m_Object.StartInteractionProcess();
                        }
                        break;

                    case E_ObjectType.Move_Box: // 이동 상자
                        if (m_isArrivePoint) 
                        {
                            Core.StartNonPublicMethod(m_Object, "SetOffset");
                            ArriveInteractionPointProcess(E_InteraciontState.Grabbing);

                            m_Animation.m_animator.SetTrigger("HoldTRG");

                            MoveBox tempObj = m_Object.GetComponent<MoveBox>();
                            tempObj.m_IKControl.m_IkActive = true;                                // IK ON
                            tempObj.m_IKControl.m_RightHandPoint = tempObj.m_HoldState_IK_RPos;
                            tempObj.m_IKControl.m_LeftHandPoint = tempObj.m_HoldState_IK_LPos;
                        }
                        break;

                    case E_ObjectType.Railing: // 난간
                        if (m_isArrivePoint) 
                        {
                            ArriveInteractionPointProcess(E_InteraciontState.Hanging);
                        }
                        break;

                    case E_ObjectType.Ladder: // 사다리
                        if (m_isArrivePoint) 
                        {
                            ArriveInteractionPointProcess(E_InteraciontState.Hanging);
                        }
                        
                        break;

                    case E_ObjectType.SwingingRope: // 밧줄
                        if (m_isArrivePoint) 
                        {
                            ArriveInteractionPointProcess(E_InteraciontState.RopeHanging);
                        }
                        break;

                    case E_ObjectType.Broken_Object: // 부숴지는 물체
                        if (m_isArrivePoint) 
                        {
                            ArriveInteractionPointProcess(E_InteraciontState.Breaking);
                            m_Object.StartInteractionProcess();
                        }

                        break;

                    case E_ObjectType.Locked_Door: // 잠긴문
                        if (m_isArrivePoint) 
                        {
                            ArriveInteractionPointProcess(E_InteraciontState.Open);
                            m_Object.StartInteractionProcess();
                        }
                        break;

                    case E_ObjectType.Umbrella: // 우산
                        if (m_isArrivePoint)
                        {
                            Core.StartNonPublicMethod(m_Object, "PickUpUmbrella");
                            ArriveInteractionPointProcess(E_InteraciontState.PickUp);
                        }
                        break;
                    case E_ObjectType.LightObject:
                        if(m_isArrivePoint)
                        {
                            ArriveInteractionPointProcess(E_InteraciontState.PickUp);
                            m_Animation.m_animator.SetTrigger("HoldNIdleTRG");
                        }
                        break;
                    case E_ObjectType.Lever:
                        if(m_isArrivePoint)
                        {
                            ArriveInteractionPointProcess(E_InteraciontState.Holding);
                        }
                        break;

                    //case "SwingDoor":
                    //    if (m_isArrivePoint)
                    //    {
                    //        ArriveInteractionPointProcess(E_InteraciontState.Open);
                    //    }

                    //    break;

                    //case "SlidingDoor":
                    //    if (m_isArrivePoint) 
                    //    {
                    //        ArriveInteractionPointProcess(E_InteraciontState.Open);
                    //    }

                    //    break;

                    //case "BigSlidingDoor":
                    //    if (m_isArrivePoint) 
                    //    {
                    //        ArriveInteractionPointProcess(E_InteraciontState.OpenNClose);
                    //    }
                    //    break;
                    case E_ObjectType.Big_Swing_Door:
                        if (m_isArrivePoint)
                        {
                            ArriveInteractionPointProcess(E_InteraciontState.OpenNClose);
                        }
                        break;
                        //case "Throwing":
                        //    if (m_isArrivePoint)
                        //    {
                        //        ArriveInteractionPointProcess(E_InteraciontState.Lifting);
                        //    }
                        //    break;

                }
            }

            return; // 상호작용 상태에서는 이동 처리 하지않도록 리턴
        }


        //처음 점프버튼이 눌렸을 경우
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (m_IsJump|| m_PlayerState==E_PlayerState.Interaction)
                return;


            //점프가 진행중이 아니면 실행
            if (m_PlayerState != E_PlayerState.JumpComplete && m_PlayerState != E_PlayerState.Jump)
            {
                //머리위에 물체 인식
                if (OverHeadCheck())
                    return;

                //스테이트저장
                m_PlayerStateSave = m_PlayerState;

                if(m_playerAni.GetBool("Jump") == false)
                {
                    m_playerAni.SetTrigger("JumpTRG");
                }
                //점프
                Jump();
            }
        }

        //점프 중에 스페이스바를 때거나 
        if (Input.GetKeyUp(KeyCode.Space) && m_Player_rigidbody.velocity.y>0)
        {
            m_PlayerState = E_PlayerState.JumpComplete;
            m_Player_rigidbody.velocity = m_Player_rigidbody.velocity * 0.5f;
            m_IsJump = true;
        }
        if (m_PlayerState == E_PlayerState.Jump && m_Player_rigidbody.velocity.y < 0)
        {
            m_PlayerState = E_PlayerState.JumpComplete;
            m_IsJump = true;
        }

        // LeftControl = 앉기키
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }

        //LeftShift = 달리기키
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Runnig();
        }

        // 상호작용
        if (Input.GetKeyDown(KeyCode.E))
        {
            InteractionFunc();
        }

    }

    #region 이동관련 작성자: 서동주

    // 달리기 부분
    void Runnig()
    {
        //기본 상태에서 달리기가 실행 되었을 경우
        if (m_PlayerState == E_PlayerState.Idle)
        {
            //플레이어 상태 변경
            m_PlayerState = E_PlayerState.Running;
            //플레이어 최고속도 변경
            m_MaxSpeed = m_RunnigSpeed;
        }
        //플레이어 상태가 앉아있는 상태일 경우
        else if (m_PlayerState == E_PlayerState.Crouch)
        {
            //머리위에 물체 인식
            if (OverHeadCheck())
                return;

            //플레이어의 상태를 달리기 상태로 변경 및 최고속도 변경
            m_PlayerState = E_PlayerState.Running;
            m_MaxSpeed = m_RunnigSpeed;
            m_Animation.SetAnimation(E_PlayerAnimation.Running);

        }
        //플레이어 상태가 달리기 상태일 경우
        else if (m_PlayerState == E_PlayerState.Running)
        {
            //기본 상태로 돌리고 걷기 속도로 변경
            m_PlayerState = E_PlayerState.Idle;
            m_MaxSpeed = m_WalkSpeed;
        }
        else
        {
            
        }
    }

    // 앉기 부분
    void Crouch()
    {
        //플레이어 상태가 기본이거나 달리기 상태이면 실행
        if (m_PlayerState == E_PlayerState.Idle || m_PlayerState == E_PlayerState.Running)
        {
            //앉기로바꿈 및 앉기 속도로 변경
            m_PlayerState = E_PlayerState.Crouch;
            m_MaxSpeed = m_CrouchSpeed;
            m_playerCollider.center = new Vector3(0, 0, 0);
            m_playerCollider.height = 1f;

            //임시처리 애니메이션 처리예정
            m_Animation.SetAnimation(E_PlayerAnimation.Crouch);
        }
        else if (m_PlayerState == E_PlayerState.Jump || m_PlayerState == E_PlayerState.JumpComplete)
        {
            //점프상태일경우 진행x
        }
        else
        {
            //일어나기
            //머리위에 물체 인식
            if (OverHeadCheck())
                return;

            //애니메이션 처리로 바꿀예정
            m_Animation.SetAnimation(E_PlayerAnimation.Idle);
            //일반 상태로 변경 및 걷기 속도로 변경
            m_playerCollider.center = new Vector3(0, 0.25f, 0);
            m_playerCollider.height = 1.5f;
            m_horizontalMove = 0;
            m_verticalMove = 0;
            KeyBlock = true;
            m_PlayerState = E_PlayerState.Idle;
            StartCoroutine("unCrouch");
        }
    }

    IEnumerator unCrouch()
    {
        yield return new WaitForSeconds(0.5f);
        m_MaxSpeed = m_WalkSpeed;
        KeyBlock = false;
    }

    // 움직임 처리부분
    void Moveing()
    {
        //움직이고 있으면 속도를 올린다
        if (m_horizontalMove != 0 || m_verticalMove != 0)
        {
            Invoke("SpeedUp", 0.1f);
        }
        //안움직이고 있으면 최소 속도로 변경
        else
        {
            m_MoveSpeed = m_StartSpeed;
        }
        //현재 속도가 최대속도를 넘으면 감속
        if (m_MoveSpeed > m_MaxSpeed)
        {
            Invoke("SpeedDown", 0.1f);
        }

        //이동처리부분
        //백터형식으로 변경
        m_Movement.Set(m_horizontalMove, 0, m_verticalMove);
        //백터 정규화 * 속도 * 시간 만큼 움직이도록 백터 설정
        m_Movement = m_Movement.normalized * m_MoveSpeed * Time.deltaTime;

        //실질적인 이동부분
        //이동하는곳 사이에 rigidbody를 가진 오브젝트가 있으면 물리처리
        m_Player_rigidbody.MovePosition(transform.position + m_Movement);
    }

    // 속도를 올리는 함수
    void SpeedUp()
    {
        //현재 속도가 최고속도보다 작으면 실행
        if (m_MoveSpeed < m_MaxSpeed)
        {
            //속도 증가량만큼 속도 증가
            m_MoveSpeed += m_SpeedMultiply;
            //넘어가면 맥스치로 유지
            if (m_MoveSpeed > m_MaxSpeed)
                m_MoveSpeed = m_MaxSpeed;
        }
    }

    // 속도 감소 함수
    void SpeedDown()
    {
        //현재속도가 최고속도보다 크면 실행
        if (m_MoveSpeed > m_MaxSpeed)
        {
            //감속량만큼 감소
            m_MoveSpeed -= m_SpeedDivision;
            //최고속도보다 작아질때까지 반복
            Invoke("SpeedDown", 0.1f);
        }
    }

    // 작성자: 서동주
    // 회전 함수
    void Turn()
    {
        //움직임이 없으면 실행x 
        if (m_horizontalMove == 0 && m_verticalMove == 0)
            return;
        //회전처리
        Quaternion newRotation = Quaternion.LookRotation(m_Movement);
        m_Player_rigidbody.rotation = Quaternion.Slerp(m_Player_rigidbody.rotation, newRotation, m_RotateSpeed * Time.deltaTime);
    }

    // 점프 함수
    void Jump() //점프할 힘을 받아옴
    {
        //점프가 완료되었으면 점프못함
        if (m_PlayerState == E_PlayerState.JumpComplete)
            return;
        
        //문제없이 여기까지 왔으면 점프처리
        m_Player_rigidbody.velocity = Vector3.zero;
        //입력받은 점프의 힘만큼 점프
        m_Player_rigidbody.AddForce(Vector3.up * m_JumpPower, ForceMode.VelocityChange);
        m_PlayerState = E_PlayerState.Jump;
    }

    // 착지함수
    void LandingCheck()
    {
        float distancey = 0.2f;
        RaycastHit hit;
        // 바닥거리 체크용
        if (Physics.Raycast(new Vector3(transform.position.x, m_meshRenderer.bounds.min.y + 0.2f, transform.position.z), -transform.up, out hit, 10f))
        {
            if(m_IsJump) { }
            else //점프중이 아니면
            {
                //맞은 레이가 낙하 거리보다 길경우
                if (hit.distance >= m_FallingDistance)
                {
                    // 4방향에서 아래 체크
                    if (Physics.Raycast(new Vector3(transform.position.x + m_BetweenDistance, m_meshRenderer.bounds.min.y + 0.2f, transform.position.z), -transform.up, out m_hit_down, m_DownDistance)
                        || Physics.Raycast(new Vector3(transform.position.x - m_BetweenDistance, m_meshRenderer.bounds.min.y + 0.2f, transform.position.z), -transform.up, out m_hit_down, m_DownDistance)
                        || Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z + m_BetweenDistance), -transform.up, out m_hit_down, m_DownDistance)
                        || Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z - m_BetweenDistance), -transform.up, out m_hit_down, m_DownDistance))
                    { }
                    else
                    {
                        // 상호작용중인지 확인
                        if (m_PlayerState == E_PlayerState.Interaction) 
                            return;

                        // 점프중으로 변경
                        m_IsJump = true;
                        // 낙하 상태로 변경
                        m_PlayerState = E_PlayerState.Falling;
                    }
                }
            }
        }

        if (!m_IsJump)
            return;


        if (Physics.Raycast(new Vector3(transform.position.x + m_BetweenDistance, m_meshRenderer.bounds.min.y + 0.2f, transform.position.z), -transform.up, out m_hit_down, m_DownDistance)
            || Physics.Raycast(new Vector3(transform.position.x - m_BetweenDistance, m_meshRenderer.bounds.min.y + 0.2f, transform.position.z), -transform.up, out m_hit_down, m_DownDistance)
            || Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z + m_BetweenDistance), -transform.up, out m_hit_down, m_DownDistance)
            || Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z - m_BetweenDistance), -transform.up, out m_hit_down, m_DownDistance))
        {
            if (m_hit_down.collider.isTrigger)
                return;

            // 점프중이거나 낙하상태이면
            if ( m_PlayerState == E_PlayerState.JumpComplete || m_PlayerState == E_PlayerState.Falling)
            {
                //착지 거리보다 작을경우
                if (m_hit_down.distance < m_LandingDistance)
                {
                    //랜딩애니메이션 출력하기위한 트리거 활성화
                    m_PlayerState = m_PlayerStateSave;
                    m_playerAni.SetTrigger("LandingTRG");
                    
                    m_horizontalMove = 0;
                    m_verticalMove = 0;
                    //KeyBlock = true;
                    m_IsFalling = false;
                    StartCoroutine("JumpProcess");
                }
            }
        }
        else //감지거리에 벗어났을경우
        {
            if (m_PlayerState == E_PlayerState.Jump || m_PlayerState == E_PlayerState.Interaction || m_PlayerState == E_PlayerState.JumpComplete)
            {
                return;
            }

            if (m_IsFalling)
            {
                return;
            }

            
            m_PlayerState = E_PlayerState.JumpComplete;
            m_PlayerStateSave = E_PlayerState.Idle;

            //낙하 애니메이션 실행
            m_playerAni.SetTrigger("FallingTRG");

            m_IsFalling = true;
            m_IsJump = false;

        }
    }

    IEnumerator JumpProcess()
    {
        //실행되고잇는게 점프인지
        while (!m_playerAni.GetCurrentAnimatorStateInfo(0).IsName("Jump End"))
        {
            yield return null;
        }
        //애니메이션 재생 중 실행되는 부분
        while (m_playerAni.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.8f)
        {
            yield return null;
        }
        //종료
        yield return new WaitForSeconds(0.05f);
        KeyBlock = false;
        yield return new WaitForSeconds(0.45f);
        m_IsJump = false;
    }

    // 머리위에 체크
    bool OverHeadCheck()
    {
        //레이케스트용 변수
        RaycastHit hit_up;
        float UpDistance = 1.0f;

        if  (Physics.Raycast(new Vector3(transform.position.x , transform.position.y, transform.position.z), transform.up, out hit_up, UpDistance)
            || Physics.Raycast(new Vector3(transform.position.x + 0.3f, transform.position.y, transform.position.z), transform.up, out hit_up, UpDistance)
            || Physics.Raycast(new Vector3(transform.position.x - 0.3f, transform.position.y, transform.position.z), transform.up, out hit_up, UpDistance)
            || Physics.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.3f), transform.up, out hit_up, UpDistance)
            || Physics.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.3f), transform.up, out hit_up, UpDistance))
        {
            if (!hit_up.collider.isTrigger)
            {
                return true;
            }
        }
        return false;
    }

    #endregion

    #region 상호작용관련 작성자: 조현진

    [SerializeField]
    [Header("상호작용 범위 콜라이더")] public BoxCollider m_InteractionRangeCollider = null;
    [Header("[상호작용 물체]")] public GameObject m_InteractionObj = null;
    [Header("[상호작용 상태]")] public E_InteraciontState m_InteractionState = E_InteraciontState.None;
    [Header("[상호작용 진행 상태]")] public bool m_isInteraction = false;
    [Header("[상호작용 이동 시간]")] public float m_DelaySec = 1.5f;
    [Header("[물건 들기 상태에서 사용할 충돌체]")] public CapsuleCollider m_HoldingStateCollider = null;

    public bool m_isArrivePoint = false;                     // 상호작용 지점 도착 확인용

    /* 상호작용 하면서 사용할 변수 */
    public Transform m_HitTransform = null;
    public RaycastHit m_HitInfo;

    [Header("[열쇠 개수]")] public int m_KeyCount = 0;
    
    [Header("[밧줄 점프할 때 힘 조절]")] public float m_RopeJumpingPower = 2.0f;

    public InteractableObject m_Object = null;   // 상호작용 오브젝트

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(this.transform.position,
                        m_InteractionRangeCollider.size);
    }

    // 상호 작용
    void InteractionFunc()
    {
        int layermask = Core.GetExcludeLayer("Except", "Player", "NoiseObj"); // 제외할 레이어
        Collider[] hitColliders = Physics.OverlapBox(this.transform.position,
                                                     m_InteractionRangeCollider.size * 0.5f,
                                                     Quaternion.identity,
                                                     layermask);

        int hitCount = hitColliders.Length;
        if (hitCount == 0) return;
        for (int i = 0; i < hitCount; i++)
        {
            InteractableObject tempObj = hitColliders[i].GetComponentInParent<InteractableObject>();
            if (tempObj != null)
            {
                if (tempObj.GetType() == typeof(Railing))
                {
                    float max_distance = 1f;
                    Physics.Raycast(transform.position, transform.forward, out m_HitInfo, max_distance);
                }

                m_HitTransform = hitColliders[i].GetComponent<Transform>();
                m_Object = tempObj;
                m_Object.InitInteraction();
                break;
            }
        }
    }

    // 상호작용 해제 처리
    public void InteractionRelease()
    {
        transform.SetParent(null);
        m_PlayerState = E_PlayerState.Idle;                     // 플레이어 상태
        m_InteractionState = E_InteraciontState.None;           // 플레이어 상호작용 상태
        m_InteractionObj = null;                                // 상호작용 오브젝트
        m_isInteraction = false;                                // 상호작용 진행 도중인지 체크하는 변수
        m_isArrivePoint = false;                                // 상호작용 지점 도착한지 체크하는 변수

        m_Object.m_isConnected = false;                         // 오브젝트 연결 해제

        m_Animation.m_animator.SetBool("IsInteraction", false);
        m_Animation.m_animator.SetTrigger("ExitTRG");

        m_MaxSpeed = m_WalkSpeed;                               // 플레이어 속도 변수
        m_Player_rigidbody.isKinematic = false;                 // 물리엔진 사용
       
        /* IK 설정 */
        if (m_Object.m_IKControl)
        {
            m_Object.m_IKControl.m_IkActive = false;            // IK Off
        }
   
    }

    // 상호작용 지점에 도착했을때 처리
    void ArriveInteractionPointProcess(E_InteraciontState p_state)
    {
        m_Animation.m_animator.SetBool("IsInteraction", false); // 애니메이션 off

        m_InteractionState = p_state;                           // 상호작용 상태 변경
        m_Object.m_IKControl = GetComponent<IKControl>();

        m_Object.m_isConnected = true;                          // 오브젝트 연결
        
        m_isArrivePoint = false;                                // 반복문에서 1번만 접근하도록 false
    }

    // 플레이어 상호작용 위치로 이동 및 회전
    public IEnumerator PlayerTranslate_InteractionPoint(Vector3 p_targetPos, Quaternion p_targetRot)
    {
        float timer = 0f;
        float distance = Vector3.Distance(transform.position, p_targetPos);
        float angle = Quaternion.Angle(transform.rotation, p_targetRot);

        // 제자리에서 다시 상호작용 한경우(임의로 조절)
        if (distance <= 0.1f && angle <= 10f)
        {
            m_isArrivePoint = true;
            StopCoroutine(m_Object.m_Coroutine);
            yield break;
        }

        m_Animation.m_animator.SetBool("IsInteraction", true); // 애니메이션 on

        while (timer <= m_DelaySec) // DelaySec 값(초)동안 반복
        {
            if (m_PlayerState != E_PlayerState.Interaction)
            {
                InteractionRelease(); // 상호작용 해제 처리
                break;
            }

            timer += Time.deltaTime;

            transform.position = Vector3.MoveTowards(transform.position, p_targetPos, Time.deltaTime * (distance / m_DelaySec));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, p_targetRot, Time.deltaTime * (angle / m_DelaySec));

            yield return null;
        }

        if (m_PlayerState == E_PlayerState.Interaction)
        {
            m_isArrivePoint = true;       
        }
    }

    // 상호작용 처리 :: 상호작용 오브젝트 에서 트리거 형식으로 애니메이션 실행 시킨후 이 함수 호출
    // p_callfn : 선택적 매개변수 값 안넣으면 null
    public IEnumerator InteractionProcess(string p_clipName, bool p_isRelease, Action<int> p_callfn = null, float p_aniPerVal = 0f)
    {
        m_Animation.m_animator.applyRootMotion = true;
        m_isInteraction = true;
        m_Player_rigidbody.isKinematic = true;       // 물리엔진 사용 x

        /* 원하는 클립 실행 될 때까지 반복 리턴 */
        while (!m_Animation.m_animator.GetCurrentAnimatorStateInfo(0).IsName(p_clipName))
        {
            yield return null;
        }

        bool isflag = true; // 콜백 함수 한번만 호출하는 용도

        /* 현재 실행중인 애니메이션(다른 스크립트에서 실행) 실행중이면 리턴 */
        while (m_Animation.m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1f)
        {
            // 애니메이션이 p_aniPerVal% 정도 진행 되면 콜백 함수 실행
            if (m_Animation.m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= p_aniPerVal && isflag)
            {
                isflag = false;
                if (p_callfn != null)
                {
                    p_callfn(0);
                }
            }

            yield return null;
        }

        /* 애니메이션이 종료후 */
        m_Animation.m_animator.applyRootMotion = false;
        if(p_isRelease)
        {
            InteractionRelease(); // 상호작용 해제 처리
        }
        else
        {
            m_isInteraction = false;
            m_Player_rigidbody.isKinematic = false;       // 물리엔진 사용
        }
    }

    // 밧줄 점프 처리
    public IEnumerator RopeJumpingProcess(Quaternion p_targetRot)
    {
        RaycastHit hitInfo;
        float distance = 0.5f; // 레이 길이
        
        float angle = Quaternion.Angle(transform.rotation, p_targetRot);  // 플레이어 회전값과 타겟 회전값 각도 계산
        bool endflag = false;

        m_isInteraction = true;

        // 플레이어 바닥면이 물체에 닿을 때 까지 반복
        while (!endflag)
        {
            yield return null;
            // Debug.DrawRay(transform.position, -transform.up * distance, Color.red, 1.0f);
            // 타겟 회전값으로 변경(프레임 마다)
            transform.rotation = Quaternion.RotateTowards(transform.rotation, p_targetRot, angle * Time.deltaTime);
            
            if (Physics.Raycast(transform.position, -transform.up, out hitInfo, distance))
            {
                endflag = true;
            }
        }

        InteractionRelease(); // 상호작용 해제 처리
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (m_PlayerState != E_PlayerState.Interaction) return;
        if (m_Object.GetType() != typeof(MoveBox)) return;
        if (collision.gameObject.layer == 10 || collision.transform == this) return;

        MoveBox tempBox = m_Object.GetComponent<MoveBox>();
        tempBox.m_IsPlayerCollision = true;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (m_PlayerState != E_PlayerState.Interaction) return;
        if (m_Object.GetType() != typeof(MoveBox)) return;
        if (collision.gameObject.layer == 10 || collision.transform == this) return;

        // 충돌 상태에서 이동 하려고 할때 처리
        MoveBox tempBox = m_Object.GetComponent<MoveBox>();
        if (tempBox.m_MoveVector == Vector3.zero) return;

        RaycastHit hit_info;
        float distance = 0.5f;
        Vector3 tempVector = new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z);
        int layermask = Core.GetExcludeLayer("MoveObject"); // Except 제외한 레이어 마스크

        Debug.DrawRay(tempVector, tempBox.m_MoveVector.normalized * distance, Color.blue, 0.2f);
        if (Physics.Raycast(tempVector, tempBox.m_MoveVector.normalized, out hit_info, distance, layermask))
        {
            tempBox.m_IsPlayerCollision = true;
            //Debug.Log("충돌");
        }
        else
        {
            tempBox.m_IsPlayerCollision = false;
            //Debug.Log("충돌 해제");
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (m_PlayerState != E_PlayerState.Interaction) return;
        if (m_Object.GetType() != typeof(MoveBox)) return;
        if (collision.gameObject.layer == 10 || collision.transform == this) return;

        MoveBox tempBox = m_Object.GetComponent<MoveBox>();
        tempBox.m_IsPlayerCollision = false;
    }

    #endregion
}

using System;
using System.Collections;
using UnityEngine;

public class Player_Control : Singleton_Mono<Player_Control> //�̱��� ����
{
    // �ܺο��� �������� �ʵ��� ������ protected ����
    protected Player_Control() { }

    #region ��������

    //������ �󿡼� �����ϰų� �׽�Ʈ �뵵�� Ȯ�ο� public
    [Header("[���� �ӵ�]")] public float m_MoveSpeed = 1f;
    [Header("[���� �ӵ�]")] public float m_StartSpeed = 1f;
    [Header("[�ӵ�������]")] public float m_SpeedMultiply = 0.5f;
    [Header("[�ӵ����ҷ�]")] public float m_SpeedDivision = 1f;
    [Header("[������ �Ѱ� �ӵ�]")] public float m_MaxSpeed = 0f;
    [Header("[�ɱ� �ӵ�]")] public float m_CrouchSpeed = 2f;
    [Header("[�ȱ� �ӵ�]")] public float m_WalkSpeed = 5f;
    [Header("[�ٴ� �ӵ�]")] public float m_RunnigSpeed = 10f;
    [Header("[ȸ�� �ӵ�]")] public float m_RotateSpeed = 10f;
    [Header("[�������� ������]")] public float m_JumpPower = 5f;
    [Header("[�÷��̾� ����]")] public E_PlayerState m_PlayerState;
    [Header("[�ӽ� ����� ����]")] public E_PlayerState m_PlayerStateSave;
    [Header("[Ű �Է¹���]")] public bool KeyBlock;
    [Header("[���������� Ȯ�ο�]")] public bool m_IsJump;

    //player �̵��� ���� �������� �����¿� (-1,0,+1)
    [Header("[�¿� �Է� Ȯ��]")] public float m_horizontalMove;
    [Header("[���� �Է� Ȯ��]")] public float m_verticalMove;

    //�÷��̾� ������ٵ�
    [Header("[�÷��̾� Rigidbody]")] public Rigidbody m_Player_rigidbody;
    //�÷��̾� �ִϸ��̼� ��ũ��Ʈ
    [Header("[�÷��̾� Ani ��ũ��Ʈ]")] public Player_Animation m_Animation;

    [Range(0, 3)]
    [Header("[���� �����ɽ�Ʈ �Ÿ�]")] public float m_DownDistance;
    [Range(0, 3)]
    [Header("[���� �����ɽ�Ʈ���� �Ÿ�]")] public float m_BetweenDistance;
    [Range(0, 3)]
    [Header("[���� ���� ��½����� �Ÿ�]")] public float m_LandingDistance;
    [Header("[���� ��� �Ÿ�]")] public float m_FallingDistance;
    [Header("[�÷��̾� �ݶ��̴�]")] public CapsuleCollider m_playerCollider;

    //private
    //�̵��� ���� ó���� ����
    private Vector3 m_Movement;
    //�Ž� ������
    private SkinnedMeshRenderer m_meshRenderer;
    //�÷��̾� �ִϸ�����
    private Animator m_playerAni;
    //�����ɽ�Ʈ�� ����
    private RaycastHit m_hit_down;
    //���ϻ��� Ȯ��
    private bool m_IsFalling = false;

    #endregion;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    //�⺻ ����
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
        InputKey(); // Ű�Է� ó��
    }

    void FixedUpdate()
    {
        // �÷��̾� ��� or ��ȣ�ۿ� 
        if (m_PlayerState == E_PlayerState.Die ||
            m_PlayerState == E_PlayerState.Interaction || m_isInteraction)
        {
            return;
        }
        //���� ó�� �κ�
        Moveing();
        Turn();
    }



    // �ۼ���: ������(�̵�����), ������(��ȣ�ۿ����)
    void InputKey()
    {
        //Ű����
        if(KeyBlock)
        {
            return;
        }

        //�����¿� Ű�Է� �޾ƿ�
        m_horizontalMove = Input.GetAxisRaw("Horizontal");
        m_verticalMove = Input.GetAxisRaw("Vertical");

        // ��ȣ �ۿ� ���� ������ Ű�Է� ó��
        if (m_PlayerState == E_PlayerState.Interaction)
        {
            if (Input.GetKeyDown(KeyCode.E) && m_InteractionState != E_InteraciontState.RopeHanging) // ���� ���� EŰ ����
            {
                if (!m_isInteraction) // ��ȣ�ۿ��� ������(�ִϸ��̼�)�̸� ���� ���ϵ��� ����
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
                    InteractionRelease(); // ��ȣ�ۿ� ����
                }
            }

            if (m_InteractionObj != null && !m_isInteraction)
            {
                switch (m_InteractionObj.GetComponent<InteractableObject>().m_ObjectType) // ��ȣ�ۿ� ���� ������Ʈ�� Ÿ������ �з�
                {
                    case E_ObjectType.Fixture_Box: // ������ ����
                        if (m_isArrivePoint)       // ��ȣ�ۿ� ������ �����ϸ� ���� ����
                        {
                            ArriveInteractionPointProcess(E_InteraciontState.Riding);
                            m_Object.StartInteractionProcess();
                        }
                        break;

                    case E_ObjectType.Move_Box: // �̵� ����
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

                    case E_ObjectType.Railing: // ����
                        if (m_isArrivePoint) 
                        {
                            ArriveInteractionPointProcess(E_InteraciontState.Hanging);
                        }
                        break;

                    case E_ObjectType.Ladder: // ��ٸ�
                        if (m_isArrivePoint) 
                        {
                            ArriveInteractionPointProcess(E_InteraciontState.Hanging);
                        }
                        
                        break;

                    case E_ObjectType.SwingingRope: // ����
                        if (m_isArrivePoint) 
                        {
                            ArriveInteractionPointProcess(E_InteraciontState.RopeHanging);
                        }
                        break;

                    case E_ObjectType.Broken_Object: // �ν����� ��ü
                        if (m_isArrivePoint) 
                        {
                            ArriveInteractionPointProcess(E_InteraciontState.Breaking);
                            m_Object.StartInteractionProcess();
                        }

                        break;

                    case E_ObjectType.Locked_Door: // ��乮
                        if (m_isArrivePoint) 
                        {
                            ArriveInteractionPointProcess(E_InteraciontState.Open);
                            m_Object.StartInteractionProcess();
                        }
                        break;

                    case E_ObjectType.Umbrella: // ���
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

            return; // ��ȣ�ۿ� ���¿����� �̵� ó�� �����ʵ��� ����
        }


        //ó�� ������ư�� ������ ���
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (m_IsJump|| m_PlayerState==E_PlayerState.Interaction)
                return;


            //������ �������� �ƴϸ� ����
            if (m_PlayerState != E_PlayerState.JumpComplete && m_PlayerState != E_PlayerState.Jump)
            {
                //�Ӹ����� ��ü �ν�
                if (OverHeadCheck())
                    return;

                //������Ʈ����
                m_PlayerStateSave = m_PlayerState;

                if(m_playerAni.GetBool("Jump") == false)
                {
                    m_playerAni.SetTrigger("JumpTRG");
                }
                //����
                Jump();
            }
        }

        //���� �߿� �����̽��ٸ� ���ų� 
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

        // LeftControl = �ɱ�Ű
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }

        //LeftShift = �޸���Ű
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Runnig();
        }

        // ��ȣ�ۿ�
        if (Input.GetKeyDown(KeyCode.E))
        {
            InteractionFunc();
        }

    }

    #region �̵����� �ۼ���: ������

    // �޸��� �κ�
    void Runnig()
    {
        //�⺻ ���¿��� �޸��Ⱑ ���� �Ǿ��� ���
        if (m_PlayerState == E_PlayerState.Idle)
        {
            //�÷��̾� ���� ����
            m_PlayerState = E_PlayerState.Running;
            //�÷��̾� �ְ�ӵ� ����
            m_MaxSpeed = m_RunnigSpeed;
        }
        //�÷��̾� ���°� �ɾ��ִ� ������ ���
        else if (m_PlayerState == E_PlayerState.Crouch)
        {
            //�Ӹ����� ��ü �ν�
            if (OverHeadCheck())
                return;

            //�÷��̾��� ���¸� �޸��� ���·� ���� �� �ְ�ӵ� ����
            m_PlayerState = E_PlayerState.Running;
            m_MaxSpeed = m_RunnigSpeed;
            m_Animation.SetAnimation(E_PlayerAnimation.Running);

        }
        //�÷��̾� ���°� �޸��� ������ ���
        else if (m_PlayerState == E_PlayerState.Running)
        {
            //�⺻ ���·� ������ �ȱ� �ӵ��� ����
            m_PlayerState = E_PlayerState.Idle;
            m_MaxSpeed = m_WalkSpeed;
        }
        else
        {
            
        }
    }

    // �ɱ� �κ�
    void Crouch()
    {
        //�÷��̾� ���°� �⺻�̰ų� �޸��� �����̸� ����
        if (m_PlayerState == E_PlayerState.Idle || m_PlayerState == E_PlayerState.Running)
        {
            //�ɱ�ιٲ� �� �ɱ� �ӵ��� ����
            m_PlayerState = E_PlayerState.Crouch;
            m_MaxSpeed = m_CrouchSpeed;
            m_playerCollider.center = new Vector3(0, 0, 0);
            m_playerCollider.height = 1f;

            //�ӽ�ó�� �ִϸ��̼� ó������
            m_Animation.SetAnimation(E_PlayerAnimation.Crouch);
        }
        else if (m_PlayerState == E_PlayerState.Jump || m_PlayerState == E_PlayerState.JumpComplete)
        {
            //���������ϰ�� ����x
        }
        else
        {
            //�Ͼ��
            //�Ӹ����� ��ü �ν�
            if (OverHeadCheck())
                return;

            //�ִϸ��̼� ó���� �ٲܿ���
            m_Animation.SetAnimation(E_PlayerAnimation.Idle);
            //�Ϲ� ���·� ���� �� �ȱ� �ӵ��� ����
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

    // ������ ó���κ�
    void Moveing()
    {
        //�����̰� ������ �ӵ��� �ø���
        if (m_horizontalMove != 0 || m_verticalMove != 0)
        {
            Invoke("SpeedUp", 0.1f);
        }
        //�ȿ����̰� ������ �ּ� �ӵ��� ����
        else
        {
            m_MoveSpeed = m_StartSpeed;
        }
        //���� �ӵ��� �ִ�ӵ��� ������ ����
        if (m_MoveSpeed > m_MaxSpeed)
        {
            Invoke("SpeedDown", 0.1f);
        }

        //�̵�ó���κ�
        //������������ ����
        m_Movement.Set(m_horizontalMove, 0, m_verticalMove);
        //���� ����ȭ * �ӵ� * �ð� ��ŭ �����̵��� ���� ����
        m_Movement = m_Movement.normalized * m_MoveSpeed * Time.deltaTime;

        //�������� �̵��κ�
        //�̵��ϴ°� ���̿� rigidbody�� ���� ������Ʈ�� ������ ����ó��
        m_Player_rigidbody.MovePosition(transform.position + m_Movement);
    }

    // �ӵ��� �ø��� �Լ�
    void SpeedUp()
    {
        //���� �ӵ��� �ְ�ӵ����� ������ ����
        if (m_MoveSpeed < m_MaxSpeed)
        {
            //�ӵ� ��������ŭ �ӵ� ����
            m_MoveSpeed += m_SpeedMultiply;
            //�Ѿ�� �ƽ�ġ�� ����
            if (m_MoveSpeed > m_MaxSpeed)
                m_MoveSpeed = m_MaxSpeed;
        }
    }

    // �ӵ� ���� �Լ�
    void SpeedDown()
    {
        //����ӵ��� �ְ�ӵ����� ũ�� ����
        if (m_MoveSpeed > m_MaxSpeed)
        {
            //���ӷ���ŭ ����
            m_MoveSpeed -= m_SpeedDivision;
            //�ְ�ӵ����� �۾��������� �ݺ�
            Invoke("SpeedDown", 0.1f);
        }
    }

    // �ۼ���: ������
    // ȸ�� �Լ�
    void Turn()
    {
        //�������� ������ ����x 
        if (m_horizontalMove == 0 && m_verticalMove == 0)
            return;
        //ȸ��ó��
        Quaternion newRotation = Quaternion.LookRotation(m_Movement);
        m_Player_rigidbody.rotation = Quaternion.Slerp(m_Player_rigidbody.rotation, newRotation, m_RotateSpeed * Time.deltaTime);
    }

    // ���� �Լ�
    void Jump() //������ ���� �޾ƿ�
    {
        //������ �Ϸ�Ǿ����� ��������
        if (m_PlayerState == E_PlayerState.JumpComplete)
            return;
        
        //�������� ������� ������ ����ó��
        m_Player_rigidbody.velocity = Vector3.zero;
        //�Է¹��� ������ ����ŭ ����
        m_Player_rigidbody.AddForce(Vector3.up * m_JumpPower, ForceMode.VelocityChange);
        m_PlayerState = E_PlayerState.Jump;
    }

    // �����Լ�
    void LandingCheck()
    {
        float distancey = 0.2f;
        RaycastHit hit;
        // �ٴڰŸ� üũ��
        if (Physics.Raycast(new Vector3(transform.position.x, m_meshRenderer.bounds.min.y + 0.2f, transform.position.z), -transform.up, out hit, 10f))
        {
            if(m_IsJump) { }
            else //�������� �ƴϸ�
            {
                //���� ���̰� ���� �Ÿ����� ����
                if (hit.distance >= m_FallingDistance)
                {
                    // 4���⿡�� �Ʒ� üũ
                    if (Physics.Raycast(new Vector3(transform.position.x + m_BetweenDistance, m_meshRenderer.bounds.min.y + 0.2f, transform.position.z), -transform.up, out m_hit_down, m_DownDistance)
                        || Physics.Raycast(new Vector3(transform.position.x - m_BetweenDistance, m_meshRenderer.bounds.min.y + 0.2f, transform.position.z), -transform.up, out m_hit_down, m_DownDistance)
                        || Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z + m_BetweenDistance), -transform.up, out m_hit_down, m_DownDistance)
                        || Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z - m_BetweenDistance), -transform.up, out m_hit_down, m_DownDistance))
                    { }
                    else
                    {
                        // ��ȣ�ۿ������� Ȯ��
                        if (m_PlayerState == E_PlayerState.Interaction) 
                            return;

                        // ���������� ����
                        m_IsJump = true;
                        // ���� ���·� ����
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

            // �������̰ų� ���ϻ����̸�
            if ( m_PlayerState == E_PlayerState.JumpComplete || m_PlayerState == E_PlayerState.Falling)
            {
                //���� �Ÿ����� �������
                if (m_hit_down.distance < m_LandingDistance)
                {
                    //�����ִϸ��̼� ����ϱ����� Ʈ���� Ȱ��ȭ
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
        else //�����Ÿ��� ��������
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

            //���� �ִϸ��̼� ����
            m_playerAni.SetTrigger("FallingTRG");

            m_IsFalling = true;
            m_IsJump = false;

        }
    }

    IEnumerator JumpProcess()
    {
        //����ǰ��մ°� ��������
        while (!m_playerAni.GetCurrentAnimatorStateInfo(0).IsName("Jump End"))
        {
            yield return null;
        }
        //�ִϸ��̼� ��� �� ����Ǵ� �κ�
        while (m_playerAni.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.8f)
        {
            yield return null;
        }
        //����
        yield return new WaitForSeconds(0.05f);
        KeyBlock = false;
        yield return new WaitForSeconds(0.45f);
        m_IsJump = false;
    }

    // �Ӹ����� üũ
    bool OverHeadCheck()
    {
        //�����ɽ�Ʈ�� ����
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

    #region ��ȣ�ۿ���� �ۼ���: ������

    [SerializeField]
    [Header("��ȣ�ۿ� ���� �ݶ��̴�")] public BoxCollider m_InteractionRangeCollider = null;
    [Header("[��ȣ�ۿ� ��ü]")] public GameObject m_InteractionObj = null;
    [Header("[��ȣ�ۿ� ����]")] public E_InteraciontState m_InteractionState = E_InteraciontState.None;
    [Header("[��ȣ�ۿ� ���� ����]")] public bool m_isInteraction = false;
    [Header("[��ȣ�ۿ� �̵� �ð�]")] public float m_DelaySec = 1.5f;
    [Header("[���� ��� ���¿��� ����� �浹ü]")] public CapsuleCollider m_HoldingStateCollider = null;

    public bool m_isArrivePoint = false;                     // ��ȣ�ۿ� ���� ���� Ȯ�ο�

    /* ��ȣ�ۿ� �ϸ鼭 ����� ���� */
    public Transform m_HitTransform = null;
    public RaycastHit m_HitInfo;

    [Header("[���� ����]")] public int m_KeyCount = 0;
    
    [Header("[���� ������ �� �� ����]")] public float m_RopeJumpingPower = 2.0f;

    public InteractableObject m_Object = null;   // ��ȣ�ۿ� ������Ʈ

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(this.transform.position,
                        m_InteractionRangeCollider.size);
    }

    // ��ȣ �ۿ�
    void InteractionFunc()
    {
        int layermask = Core.GetExcludeLayer("Except", "Player", "NoiseObj"); // ������ ���̾�
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

    // ��ȣ�ۿ� ���� ó��
    public void InteractionRelease()
    {
        transform.SetParent(null);
        m_PlayerState = E_PlayerState.Idle;                     // �÷��̾� ����
        m_InteractionState = E_InteraciontState.None;           // �÷��̾� ��ȣ�ۿ� ����
        m_InteractionObj = null;                                // ��ȣ�ۿ� ������Ʈ
        m_isInteraction = false;                                // ��ȣ�ۿ� ���� �������� üũ�ϴ� ����
        m_isArrivePoint = false;                                // ��ȣ�ۿ� ���� �������� üũ�ϴ� ����

        m_Object.m_isConnected = false;                         // ������Ʈ ���� ����

        m_Animation.m_animator.SetBool("IsInteraction", false);
        m_Animation.m_animator.SetTrigger("ExitTRG");

        m_MaxSpeed = m_WalkSpeed;                               // �÷��̾� �ӵ� ����
        m_Player_rigidbody.isKinematic = false;                 // �������� ���
       
        /* IK ���� */
        if (m_Object.m_IKControl)
        {
            m_Object.m_IKControl.m_IkActive = false;            // IK Off
        }
   
    }

    // ��ȣ�ۿ� ������ ���������� ó��
    void ArriveInteractionPointProcess(E_InteraciontState p_state)
    {
        m_Animation.m_animator.SetBool("IsInteraction", false); // �ִϸ��̼� off

        m_InteractionState = p_state;                           // ��ȣ�ۿ� ���� ����
        m_Object.m_IKControl = GetComponent<IKControl>();

        m_Object.m_isConnected = true;                          // ������Ʈ ����
        
        m_isArrivePoint = false;                                // �ݺ������� 1���� �����ϵ��� false
    }

    // �÷��̾� ��ȣ�ۿ� ��ġ�� �̵� �� ȸ��
    public IEnumerator PlayerTranslate_InteractionPoint(Vector3 p_targetPos, Quaternion p_targetRot)
    {
        float timer = 0f;
        float distance = Vector3.Distance(transform.position, p_targetPos);
        float angle = Quaternion.Angle(transform.rotation, p_targetRot);

        // ���ڸ����� �ٽ� ��ȣ�ۿ� �Ѱ��(���Ƿ� ����)
        if (distance <= 0.1f && angle <= 10f)
        {
            m_isArrivePoint = true;
            StopCoroutine(m_Object.m_Coroutine);
            yield break;
        }

        m_Animation.m_animator.SetBool("IsInteraction", true); // �ִϸ��̼� on

        while (timer <= m_DelaySec) // DelaySec ��(��)���� �ݺ�
        {
            if (m_PlayerState != E_PlayerState.Interaction)
            {
                InteractionRelease(); // ��ȣ�ۿ� ���� ó��
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

    // ��ȣ�ۿ� ó�� :: ��ȣ�ۿ� ������Ʈ ���� Ʈ���� �������� �ִϸ��̼� ���� ��Ų�� �� �Լ� ȣ��
    // p_callfn : ������ �Ű����� �� �ȳ����� null
    public IEnumerator InteractionProcess(string p_clipName, bool p_isRelease, Action<int> p_callfn = null, float p_aniPerVal = 0f)
    {
        m_Animation.m_animator.applyRootMotion = true;
        m_isInteraction = true;
        m_Player_rigidbody.isKinematic = true;       // �������� ��� x

        /* ���ϴ� Ŭ�� ���� �� ������ �ݺ� ���� */
        while (!m_Animation.m_animator.GetCurrentAnimatorStateInfo(0).IsName(p_clipName))
        {
            yield return null;
        }

        bool isflag = true; // �ݹ� �Լ� �ѹ��� ȣ���ϴ� �뵵

        /* ���� �������� �ִϸ��̼�(�ٸ� ��ũ��Ʈ���� ����) �������̸� ���� */
        while (m_Animation.m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1f)
        {
            // �ִϸ��̼��� p_aniPerVal% ���� ���� �Ǹ� �ݹ� �Լ� ����
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

        /* �ִϸ��̼��� ������ */
        m_Animation.m_animator.applyRootMotion = false;
        if(p_isRelease)
        {
            InteractionRelease(); // ��ȣ�ۿ� ���� ó��
        }
        else
        {
            m_isInteraction = false;
            m_Player_rigidbody.isKinematic = false;       // �������� ���
        }
    }

    // ���� ���� ó��
    public IEnumerator RopeJumpingProcess(Quaternion p_targetRot)
    {
        RaycastHit hitInfo;
        float distance = 0.5f; // ���� ����
        
        float angle = Quaternion.Angle(transform.rotation, p_targetRot);  // �÷��̾� ȸ������ Ÿ�� ȸ���� ���� ���
        bool endflag = false;

        m_isInteraction = true;

        // �÷��̾� �ٴڸ��� ��ü�� ���� �� ���� �ݺ�
        while (!endflag)
        {
            yield return null;
            // Debug.DrawRay(transform.position, -transform.up * distance, Color.red, 1.0f);
            // Ÿ�� ȸ�������� ����(������ ����)
            transform.rotation = Quaternion.RotateTowards(transform.rotation, p_targetRot, angle * Time.deltaTime);
            
            if (Physics.Raycast(transform.position, -transform.up, out hitInfo, distance))
            {
                endflag = true;
            }
        }

        InteractionRelease(); // ��ȣ�ۿ� ���� ó��
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

        // �浹 ���¿��� �̵� �Ϸ��� �Ҷ� ó��
        MoveBox tempBox = m_Object.GetComponent<MoveBox>();
        if (tempBox.m_MoveVector == Vector3.zero) return;

        RaycastHit hit_info;
        float distance = 0.5f;
        Vector3 tempVector = new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z);
        int layermask = Core.GetExcludeLayer("MoveObject"); // Except ������ ���̾� ����ũ

        Debug.DrawRay(tempVector, tempBox.m_MoveVector.normalized * distance, Color.blue, 0.2f);
        if (Physics.Raycast(tempVector, tempBox.m_MoveVector.normalized, out hit_info, distance, layermask))
        {
            tempBox.m_IsPlayerCollision = true;
            //Debug.Log("�浹");
        }
        else
        {
            tempBox.m_IsPlayerCollision = false;
            //Debug.Log("�浹 ����");
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

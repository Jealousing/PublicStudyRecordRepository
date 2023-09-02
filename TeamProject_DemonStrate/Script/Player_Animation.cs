using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 작성자: 서동주
public class Player_Animation : Singleton_Mono<Player_Animation>
{
    public Animator m_animator;
    public E_PlayerAnimation m_PlayerAnimation;

    public float m_KeyBlockTime = 0.005f;

    public int num;
    public int repaetnum;

    bool IsLeftArrow;
    bool IsRightArrow;
    bool IsUpArrow;
    bool IsDownArrow;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
    }


    // Start is called before the first frame update
    void Start()
    {
        repaetnum = num;
        m_PlayerAnimation = E_PlayerAnimation.Idle;
    }
    
    void UpdateAnimator()
    {
        m_animator.SetInteger("PlayerState", (int)m_PlayerAnimation);
    }

    public void SetAnimation(E_PlayerAnimation p_playerAnimation)
    {
        E_PlayerAnimation temp;
        temp = m_PlayerAnimation;
        if (temp != p_playerAnimation)
        {
            m_PlayerAnimation = p_playerAnimation;
        }
        UpdateAnimator();
    }
    void EndKeyBlock()
    {
        Player_Control.GetInstance.KeyBlock = false;
        if (Player_Control.GetInstance.m_IsJump)
            Player_Control.GetInstance.m_IsJump = false;
    }

    void Moveing()
    {
        if (Player_Control.GetInstance.m_horizontalMove != 0 || Player_Control.GetInstance.m_verticalMove != 0)
        {
            if (Player_Control.GetInstance.m_PlayerState == E_PlayerState.JumpComplete || Player_Control.GetInstance.m_PlayerState == E_PlayerState.Jump)
            {
                return;
            }

            if (Player_Control.GetInstance.m_PlayerState == E_PlayerState.Running)
            {
                SetAnimation(E_PlayerAnimation.Running);
            }
            else if (Player_Control.GetInstance.m_PlayerState == E_PlayerState.Crouch)
            {
                SetAnimation(E_PlayerAnimation.CrouchMove);
            }
            else
            {
                SetAnimation(E_PlayerAnimation.Walking);
            }


        }
        else
        {
            if (m_PlayerAnimation == E_PlayerAnimation.CrouchMove)
            {
                SetAnimation(E_PlayerAnimation.Crouch);
            }

            if (Player_Control.GetInstance.m_PlayerState == E_PlayerState.JumpComplete || Player_Control.GetInstance.m_PlayerState == E_PlayerState.Jump
                || Player_Control.GetInstance.m_PlayerState == E_PlayerState.Crouch)
            {
                return;
            }

            if (m_PlayerAnimation == E_PlayerAnimation.Running && Player_Control.GetInstance.m_MoveSpeed > Player_Control.GetInstance.m_WalkSpeed
              &&
              (Player_Control.GetInstance.m_PlayerState != E_PlayerState.Jump || Player_Control.GetInstance.m_PlayerState != E_PlayerState.JumpComplete))
            {
                //Player_Control.GetInstance.KeyBlock = true;
                //SetAnimation(E_PlayerAnimation.ChangeDirection);
                //Player_Control.GetInstance.m_MoveSpeed = Player_Control.GetInstance.m_StartSpeed;
                //Player_Control.GetInstance.m_Player_rigidbody.AddRelativeForce(Vector3.forward *Player_Control.GetInstance.ForwardDistanceCheck());
                
            }
            else
            {
                SetAnimation(E_PlayerAnimation.Idle);
            }
        }
    }


    void Update()
    {
        if (Player_Control.GetInstance.m_PlayerState == E_PlayerState.Interaction ||
            Player_Control.GetInstance.m_isInteraction)
        {
            return;
        }
        //Inputkey();
        Moveing();
    }
    IEnumerator Brakes()
    {
        if (!m_animator.GetCurrentAnimatorStateInfo(0).IsName("Runnig Turn"))
        {
            if (triger)
            {
                yield break;
            }
            Debug.Log("급정거실행");
            Player_Control.GetInstance.KeyBlock = true;
            Player_Control.GetInstance.m_horizontalMove = 0;
            Player_Control.GetInstance.m_verticalMove = 0;

            m_animator.SetTrigger("BrakeTRG");
            StartCoroutine(InteractionProcess("Runnig Turn"));
            
            Player_Control.GetInstance.m_MoveSpeed = Player_Control.GetInstance.m_StartSpeed;
            yield return new WaitForSecondsRealtime(0.8f);
            Player_Control.GetInstance.KeyBlock = false;
            

        }
        else
        {
            yield return new WaitForSecondsRealtime(0f);
        }

    }

    public IEnumerator InteractionProcess(string p_clipName)
    {
        
        m_animator.applyRootMotion = true;

        /* 원하는 클립 실행 될 때까지 반복 리턴 */
        while (!m_animator.GetCurrentAnimatorStateInfo(0).IsName(p_clipName))
        {
            yield return null;
        }
       // Vector3 test=-this.transform.forward ;
        //transform.Rotate(new Vector3(0, 180f, 0));

        /* 현재 실행중인 애니메이션(다른 스크립트에서 실행) 실행중이면 리턴 */
        while (m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1f)
        {
            //진행중
            //this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(test), 25 * Time.deltaTime);
            yield return null;
        }

        /* 애니메이션이 종료후 */
        triger = false;
        m_animator.applyRootMotion = false;
    }

    bool triger;
    void Inputkey()
    {
        if (triger)
        {
            return;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            IsLeftArrow = true;
            if (IsRightArrow)
            {
                IsRightArrow = false;
                StartCoroutine(Brakes());
            }
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            IsRightArrow = true;
            if (IsLeftArrow)
            {
                IsLeftArrow = false;
                StartCoroutine(Brakes());
            }
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            IsUpArrow = true;
            if (IsDownArrow)
            {
                IsDownArrow = false;
                StartCoroutine(Brakes());
            }
                
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            IsDownArrow = true;
            if (IsUpArrow)
            {
                IsUpArrow = false;
                StartCoroutine(Brakes());
            }
        }

        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            IsLeftArrow = false;
        }

        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            IsRightArrow = false;
        }

        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            IsUpArrow = false;
        }

        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            IsDownArrow = false;
        }
    }

}


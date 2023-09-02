using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// �ۼ���: ������
public class SlaveAI : MonoBehaviour
{
    #region ���� 

    //����� NavAgent ����
    NavMeshAgent m_SlaveAgent = null;
    // npc �̵��ӵ�
    [Header("[�̵��ӵ�]")] public float m_MoveSpeed = 5.0f;

    //��������Ʈ ������
    [SerializeField]
    [Header("[��������Ʈ����]")] public WayPointData[] m_WayPointData = new WayPointData[1];
    //�� ���� ī��Ʈ
    int m_MapCount = 0;
    //��������Ʈ ���� ī��Ʈ
    int m_WayCount = 0;

    //Noise�ý��� ����
    [SerializeField]
    [Header("[�����ý���Gizmos OnOff]")] bool IsNoiseGizmos;
    [SerializeField]
    [Header("[�����ý��ۼ���]")] NoiseDetectionSetting[] m_NoiseSet;
    //�������� �ν��� ���̾�
    [SerializeField]
    [Header("[�Ҹ��� �ν��� ���̾� ����ũ]")] LayerMask m_NoiseLayerMask = 0;
    //��������� ���������� �뿹�� ���������
    [SerializeField]
    [Header("[�Ҹ��� �ν��� ���̾� ����ũ]")]  int m_SurpriseLv;
    //Ÿ�ٵ� �������� ����
    public int m_TargetNoiseLv;
    //������ �ִ� ����
    int m_MaxNoiseLv;
    //�ִ� Ž���Ÿ�
    float m_MaxDistance = 0f;


    //Ÿ��
    public Transform m_FollowingTarget;
    Transform m_Target;
    Transform m_NoiseTarget;
    Transform m_TempNoiseTarget;
    bool IsAni;
    bool IsNotArrival;
    bool IsWarning = false;
    bool IsSurprised = false;

    //�ִϸ��̼� ��Ʈ��
    Animator m_SlaveAni;

    public bool m_StopSlave = true;

    //������ ������Ʈ
    public GameObject m_WorkBenchObj; //�۾���
    public Transform m_WorkBenchTr;
    public GameObject m_Hammer;

    Coroutine RunnigCoroutine;
    E_SlaveAction m_SlaveAction;
    Transform m_SlaveLookTarget;

    // ���� ���� 
    public enum Slave_State
    {
        STATE_IDLE,          //�⺻ ���x
        STATE_WORK,         //�۾� ����
        STATE_TARGET,       //Ÿ�� ���� ����
        STATE_ROAM,         //���� ����
        STATE_TEMP          //������ ���� ���x
    }

    //���� ���� ����
    public Slave_State startState = Slave_State.STATE_WORK;
    //���� ����
    private Slave_State currentState;

    //���� ���� ȣ���
    Action<StateFlow> m_PreviousCallFN = null;
    //���� ���� ȣ���
    Action<StateFlow> m_CurrentCallFN = null;
    //���� ����Ʈ
    List<Action<StateFlow>> m_AllCallFNList = new List<Action<StateFlow>>();

    //���� ������Ʈ ������ ��
    Slave_State m_CurrentStateType = Slave_State.STATE_TEMP;
    Slave_State m_PreviousStateType = Slave_State.STATE_TEMP;

    bool startFlag = false;

    #endregion;

    #region State(Start,Update,Change)

    // ���� , ���� , ������
    private enum StateFlow
    {
        ENTER,
        UPDATE,
        EXIT
    }

    // ���� ���� ����
    private void SetStartState(Slave_State startState)
    {
        m_CurrentCallFN = m_AllCallFNList[(int)startState];
        m_CurrentCallFN(StateFlow.ENTER);
    }

    // ������Ʈ
    void Update()
    {
        if (m_StopSlave)
            return;

        CommonUpdate();

        //���� �����ϴ� ����
        if(m_CurrentCallFN!=null)
        {
            m_CurrentCallFN(StateFlow.UPDATE);
        }
    }

    
    // ���� ����
    private void ChangeState(Slave_State nextState)
    {
        //�������� ���¿� ������ ���°� �ٸ���츸 ����
        if (m_CurrentStateType == nextState)
            return;
        
        //���� ���� ����
        m_PreviousStateType = m_CurrentStateType;
        m_PreviousCallFN = m_CurrentCallFN;
        //������� ����
        m_CurrentStateType = nextState;
        m_CurrentCallFN = m_AllCallFNList[(int)nextState];

        //�������̴� ���� exit ����
        m_PreviousCallFN(StateFlow.EXIT);

        //���� ���� �˸�
        m_CurrentCallFN(StateFlow.ENTER);

    }

    #endregion;

    #region Awake, Start, Update State

    //�⺻����
    private void Awake()
    {
        m_SlaveAni = GetComponent<Animator>();
        m_SlaveAgent = GetComponent<NavMeshAgent>();
       
    }
    void Start()
    {
        m_Hammer.SetActive(false);
        m_Target = null;
        m_StopSlave = true;
        //�ʿ��� �����ý��� ���� ���� ����
        for (int i = 0; i < m_NoiseSet.Length; i++)
        {
            if (m_MaxNoiseLv < m_NoiseSet[i].DetectionLv)
                m_MaxNoiseLv = m_NoiseSet[i].DetectionLv;
            if (m_MaxDistance < m_NoiseSet[i].Distance)
                m_MaxDistance = m_NoiseSet[i].Distance;
        }

        IsAni = true;

        //list ����� �߰�
        for (int i = 0; i < ((int)(Slave_State.STATE_TEMP))+1; i++)
        {
            m_AllCallFNList.Add(null);
        }

        //������� ����
        m_AllCallFNList[(int)Slave_State.STATE_IDLE] = STATE_IDLE;
        m_AllCallFNList[(int)Slave_State.STATE_WORK] = STATE_WORK;
        m_AllCallFNList[(int)Slave_State.STATE_TARGET] = STATE_TARGET;
        m_AllCallFNList[(int)Slave_State.STATE_ROAM] = STATE_ROAM;
        m_AllCallFNList[(int)Slave_State.STATE_TEMP] = STATE_TEMP;

        //���� ���� ����
        SetStartState(startState);

        //���� ���� ����
        m_PreviousStateType = Slave_State.STATE_WORK;

        //������ �ý��� �ݺ�
        InvokeRepeating("NoiseSystem", 1.0f, 1.0f);
        //InvokeRepeating("TargetCheck", 1.0f, 1.0f);

        StartCoroutine(FollowTarget(m_WorkBenchObj.transform, "WorkTRG"));
    }


    private void CommonUpdate()
    {

    }

    private void STATE_IDLE(StateFlow stateFlow)
    {
        switch (stateFlow)
        {
            case StateFlow.ENTER:
                
                break;
            case StateFlow.UPDATE:

                break;
            case StateFlow.EXIT:

                break;
        }
    }

    private void STATE_WORK(StateFlow stateFlow)
    {
        
        switch (stateFlow)
        {
            case StateFlow.ENTER:
                //��ũ��ġ�� �̵�(�Ÿ���)
                if(Vector3.Distance(m_WorkBenchObj.transform.position,this.transform.position)>=0.3f)
                {
                    //��ũ��ġ(�۾��ϴ°�)�� �̵�
                    m_SlaveAni.ResetTrigger("WorkEndTRG");
                    m_SlaveAgent.SetDestination(m_WorkBenchTr.position);
                    m_SlaveAni.SetTrigger("WalkTRG");
                }
                break;
            case StateFlow.UPDATE:
                //Ÿ�� üũ 
                TargetCheck();
                //�̵��Ѱ� �������Ȯ�� �������� 
                if (m_SlaveAgent.velocity.sqrMagnitude >= 0.25f * 0.25f && m_SlaveAgent.remainingDistance <= 0.25f )
                {
                    m_SlaveAni.ResetTrigger("WalkTRG");
                    StartCoroutine(FollowTarget(m_WorkBenchObj.transform,"WorkTRG"));
                }
                break;
            case StateFlow.EXIT:
                break;
        }
    }

    private void STATE_TARGET(StateFlow stateFlow)
    {
        //Ÿ�ټ���
        switch (stateFlow)
        {
            case StateFlow.ENTER:
                //�ȱ� �ִϸ��̼� ����
                m_SlaveAni.SetTrigger("WalkTRG");
                break;
            case StateFlow.UPDATE:
                //����Ȯ��
                if (m_SlaveAgent.velocity.sqrMagnitude >= 0.25f * 0.25f && m_SlaveAgent.remainingDistance <= 0.25f)
                {
                    if(m_TargetNoiseLv!=0)
                    {
                        m_SlaveAni.ResetTrigger("WalkTRG");
                        m_TargetNoiseLv = 0;
                        StartCoroutine(ActionProcess("LowGropingTRG", "LowGroping"));
                    }
                    else
                    {
                        ResetTarget();
                        m_SlaveAni.ResetTrigger("WalkTRG");
                        m_SlaveAni.SetTrigger("IdleTRG");
                    }
                    
                    if(!IsWarning)
                    {
                        IsWarning = true;
                    }

                    // ���º���
                    ChangeState(Slave_State.STATE_ROAM);
                }
                break;
            case StateFlow.EXIT:
                break;
        }
    }

    private void STATE_ROAM(StateFlow stateFlow)
    {
        //����
        switch (stateFlow)
        {
            case StateFlow.ENTER:
                //��������Ʈ �̵� ����
                NextWayPoint();
                break;
            case StateFlow.UPDATE:
                //Ÿ���� �ִ��� Ȯ��
                TargetCheck();

                //Ÿ���� ������ ��������x
                if (m_Target != null || m_TempNoiseTarget != null || m_NoiseTarget != null)
                    return;

                //����
                if (m_SlaveAgent.velocity.sqrMagnitude >= 0.25f * 0.25f && m_SlaveAgent.remainingDistance <= 0.25f)
                {
                    //�����ߴµ� ������ �ִϸ��̼��� ���� ���
                    if(m_SlaveAction == E_SlaveAction.Null)
                    {
                        //���� ��������Ʈ�� �̵�
                        NextWayPoint();
                    }
                    else
                    {
                        //������ �ִϸ��̼� ���
                        switch (m_SlaveAction)
                        {
                            case E_SlaveAction.Groping:
                                StartCoroutine(ActionProcess("LowGropingTRG", "LowGroping"));
                                break;
                            case E_SlaveAction.HighGroping:
                                StartCoroutine(ActionProcess("HighGropingTRG", "HighGroping"));
                                break;

                        }
                    }
                }
                break;
            case StateFlow.EXIT:
                break;
        }
    }

    

    private void STATE_TEMP(StateFlow stateFlow)
    {
        switch (stateFlow)
        {
            case StateFlow.ENTER:
                break;
            case StateFlow.UPDATE:
                break;
            case StateFlow.EXIT:
                break;
        }
    }

    #endregion;

    #region ���� ���� �Լ�

    //���� �ý���
    void NoiseSystem()
    {
        bool IsNoise;
        Transform Targert;
        int tempNoiseLv;

        if (m_StopSlave)
            return;

        //�����ý��ۺ��� ������ �־�� �Ǵ� �Ϲ� ���� (���� ���氡��)
        if (m_Target != null)
            return;

        //��������
        (IsNoise, Targert, tempNoiseLv) = NoiseDetection();

        if (IsNoise)
        {
            //Ÿ���� ��������
            m_TargetNoiseLv = tempNoiseLv;
            //Ÿ�ټ���
            SetNoiseTarget(Targert);
        }

    }

    //���� ����
    (bool, Transform, int) NoiseDetection()
    {//�������, Ÿ����ġ����, ��������
        //�ݶ��̴� ����(Ư�� ���̾)
        Collider[] colliders = Physics.OverlapSphere(transform.position, m_MaxDistance, m_NoiseLayerMask);

        //������ �� �ݶ��̴��� �������
        if (colliders.Length > 0)
        {
            //���� ���������� ������������ Ž��
            for (int i = m_MaxNoiseLv; 0 < i; i--)
            {
                //ã�� ���������� Ž���Ÿ��� ã������ ����
                float FindDistance = 0;
                //�ش� ������ �´� �Ÿ� ã��(���ÿ���)
                for (int j = 0; j < m_NoiseSet.Length; j++)
                {
                    if (m_NoiseSet[j].DetectionLv == i)
                    {
                        FindDistance = m_NoiseSet[j].Distance;
                    }
                }
                //������ �ݶ��̴���ŭ �ݺ�
                for (int j = 0; j < colliders.Length; j++)
                {
                    //������ �´� �Ÿ��� ���� �ִ� �������� Ȯ��
                    if (colliders[j].GetComponent<NoiseData>().m_NoiseLv == i &&
                        Vector3.Distance(colliders[j].gameObject.transform.position, this.transform.position) <= FindDistance)
                    {
                        //Ÿ������ ������ ������ �������
                        if (m_TargetNoiseLv == 0)
                        {
                            return (true, colliders[j].transform, i);
                        }
                        else //Ÿ������ ������ ������ ������
                        {
                            //Ÿ������ ������ �������� ���������� Ŭ ��� Ÿ�ٺ���
                            if (m_TargetNoiseLv < i)
                            {
                                return (true, colliders[j].transform, i);
                            }
                            else
                            {
                            }
                        }
                    }
                }
            }
        }
        //������ Ÿ�پ���
        return (false, transform, 0);
    }

    //����Ÿ�ټ���
    public void SetNoiseTarget(Transform p_target)
    {
        if (!p_target)
        {
            return;
        }
        // 1������ ��û ������ ���������� �ٷ� �÷��̾ �߰��Ѵ�.
        if(m_TargetNoiseLv==1)
        {
            m_Target = Player_Control.GetInstance.transform;
        }

        //Ÿ�� ���������� ���ߵǴ� �������� ���ٸ�
        else if (m_TargetNoiseLv >= m_SurpriseLv)
        {
            //�ӽ�Ÿ������ ����
            if (m_TempNoiseTarget != null)
                return;

            m_TempNoiseTarget = p_target;
            
            if (RunnigCoroutine==null)
            {
                //�뿹npc�� ���� ����
                RunnigCoroutine=StartCoroutine(SurprisedProcess());
            }

        }
        else
        {
            m_NoiseTarget = p_target;
        }
    }

   
    IEnumerator SurprisedProcess()
    {
        if (IsSurprised)
            yield break;
        else
        {
            IsSurprised = true;
        }

        //���������� ���� �� ������ ����
        m_SlaveAni.SetTrigger("SurprisedTRG");
        m_SlaveAgent.isStopped = true;
        m_SlaveAgent.updatePosition = false;
        m_SlaveAgent.updateRotation = false;
        m_SlaveAgent.velocity = Vector3.zero;

        //����ǰ��մ°� �������������� Ȯ��
        while (!m_SlaveAni.GetCurrentAnimatorStateInfo(0).IsName("Surprised"))
        {
            yield return null;
        }
        //�ִϸ��̼� ��� �� ����Ǵ� �κ�
        while (m_SlaveAni.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.80f)
        {
            yield return null;
        }

        //���������� ���� 
        if(m_PreviousStateType == Slave_State.STATE_WORK)
        {
            m_SlaveAni.SetTrigger("WorkEndTRG");
            RunnigCoroutine = StartCoroutine(WorkEndProcess());

        }
        else
        {
            
            IsSurprised = false;
            m_SlaveAgent.isStopped = false;
            m_SlaveAgent.updatePosition = true;
            m_SlaveAgent.updateRotation = true;
            RunnigCoroutine = null;
            IsAni = false;
            TargetCheck();
        }
    }

    IEnumerator WorkEndProcess()
    {
        //����ǰ��մ°� ���������� Ȯ��
        while (!m_SlaveAni.GetCurrentAnimatorStateInfo(0).IsName("WorkEnd"))
        {
            yield return null;
        }

        m_Hammer.SetActive(false);

        //�ִϸ��̼� ��� �� ����Ǵ� �κ�
        while (m_SlaveAni.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }

        //������ �ִϸ��̼� ���� 
        m_SlaveAgent.isStopped = false;
        m_SlaveAgent.updatePosition = true;
        m_SlaveAgent.updateRotation = true;
        RunnigCoroutine = null;
        IsAni = false;
        IsSurprised = false;
        TargetCheck();
    }


    #endregion;

    #region Ÿ�� ���� �Լ�
    //Ÿ�� Ȯ��
    void TargetCheck()
    {
        // ���� ���� Ȯ��
        if (m_StopSlave)
            return;

        if (m_Target != null) //Ÿ���� ������
        {
            if(m_FollowingTarget!=m_Target)
            {
                //Ÿ���� ����
                StartCoroutine(SetTarget(m_Target));
            }
            //Ÿ������ ����
            if(Vector3.Distance(m_Target.position, this.transform.position)>10.0f)
            {
                m_Target = null;
                ChangeState(Slave_State.STATE_ROAM);
            }
        }
        else if (m_NoiseTarget != null)
        {
            //���󰡰��ִ� Ÿ���̶� ��
            if(m_FollowingTarget!=m_NoiseTarget)
            {
                //Ÿ�ټ���
                StartCoroutine(SetTarget(m_NoiseTarget));
                IsNotArrival = true;
            }
            
        }
        else
        {
            if (m_TempNoiseTarget != null && !IsAni)
            {
                //�ӽ� ���� Ÿ���� ����Ÿ������ ����
                IsAni = true;
                m_NoiseTarget = m_TempNoiseTarget;
                m_TempNoiseTarget = null;
            }
        }

        //Ÿ���� ������ ������ ���������
        if (IsNotArrival)
        {
            //���� ������
            if (m_SlaveAgent.velocity.sqrMagnitude >= 0.2f * 0.2f && m_SlaveAgent.remainingDistance <= 0.5f)
            {
                //Ÿ�� ���¼���
                IsNotArrival = false;
                ResetTarget();
            }

        }
    }

    //Ÿ�ټ���
    IEnumerator SetTarget(Transform p_TargetTr)
    {
        if (!p_TargetTr)
            yield break;


        while(RunnigCoroutine!=null)
        {
            yield return null;
        }


        if (!p_TargetTr)
            yield break;

        if (m_SlaveAgent.SetDestination(p_TargetTr.position))
        {
        }
        else
        {
        }
        m_FollowingTarget = p_TargetTr;
        ChangeState(Slave_State.STATE_TARGET);
    }
    Quaternion test1;
    Quaternion test2;
    //Ÿ�ٹٶ󺸱� (Ÿ�� Transform, �ൿ�� �ִϸ��̼� Ʈ���� ��)
    IEnumerator FollowTarget(Transform p_Target, String p_Trigger)
    {
        int testnum = 0;

        if (p_Target == null)
        {
            m_SlaveAni.SetTrigger(p_Trigger);
            yield break;
        }
            

        while (true)
        {

            Vector3 TargetDir = p_Target.position - this.transform.position;
            
            this.transform.rotation = test2 =Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(TargetDir), Time.deltaTime * 0.11f);
           
            if (test1== test2)
            {
                yield return null;

                testnum++;
                m_SlaveAni.ResetTrigger("WalkTRG");
                if(testnum>=15)
                {
                    m_SlaveAni.ResetTrigger("RightTurnTRG");
                    m_SlaveAni.ResetTrigger("LeftTurnTRG");
                    m_SlaveAni.SetTrigger(p_Trigger);
                    this.transform.LookAt(p_Target);

                    if (p_Trigger == "WorkTRG")
                    {
                        //����ǰ��մ�
                        while (!m_SlaveAni.GetCurrentAnimatorStateInfo(0).IsName("Work Start"))
                        {
                            yield return null;
                        }
                        //�ִϸ��̼� ��� �� ����Ǵ� �κ�
                        while (m_SlaveAni.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.3f)
                        {
                            yield return null;
                        }
                        m_Hammer.SetActive(true);
                    }

                    testnum = 0;
                    StopCoroutine("FollowTarget");
                    yield break;
                }

            }
            else
            {
                yield return null;
                float Limit = Vector3.SignedAngle(transform.up, TargetDir, -this.transform.forward);
                //Debug.Log("Limit �� : " + Limit);

                if (Limit > 90.0f)//����ȸ��
                {
                    if (!m_SlaveAni.GetCurrentAnimatorStateInfo(0).IsName("RightTurn"))
                    {
                       // m_SlaveAni.SetTrigger("RightTurnTRG");
                    }
                }
                else if (Limit < -90.0f)//����ȸ��
                {
                    if (!m_SlaveAni.GetCurrentAnimatorStateInfo(0).IsName("LeftTurn"))
                    {
                        //m_SlaveAni.SetTrigger("LeftTurnTRG");
                    }
                }

                test1 = test2;
            }
           

        }
    }

    public void ResetTarget()
    {
        m_SlaveAgent.ResetPath();
        m_Target = null;
        m_NoiseTarget = null;
        m_TempNoiseTarget = null;
    }
    #endregion;

    #region ���� ���� �Լ�
    void NextWayPoint()
    {
        //���� ��������Ʈ�� �̵�
        m_SlaveAni.SetTrigger("WalkTRG");
        m_SlaveAction = m_WayPointData[m_MapCount].WayPoints[m_WayCount].Action;
        m_SlaveLookTarget = m_WayPointData[m_MapCount].WayPoints[m_WayCount].LookObj;
        m_SlaveAgent.SetDestination(m_WayPointData[m_MapCount].WayPoints[m_WayCount++].WayPoint.position);
        //�� ���������� �ʱ�ȭ
        if (m_WayCount >= m_WayPointData[m_MapCount].WayPoints.Length)
        {
            m_WayCount = 0;
        }
    }

    //���� �ٲ�� ���(�뿹�� ������ ��)
    public void ChangeOfPatrolRoom(GameObject other)
    {
        for (int i = 0; i < m_WayPointData.Length; i++)
        {
            if (ReferenceEquals(other, m_WayPointData[i].Collider))
            {
                m_MapCount = i;
                float min = 10000;
                int waypoint = 0;
                for (int j = 0; j < m_WayPointData[i].WayPoints.Length; j++)
                {
                    if (j == 0)
                        min = Vector3.Distance(m_WayPointData[i].WayPoints[waypoint].WayPoint.position, this.transform.position);

                    //���� ����� ��������Ʈ ã��
                    if (min > Vector3.Distance(m_WayPointData[i].WayPoints[waypoint].WayPoint.position, this.transform.position))
                    {
                        min = Vector3.Distance(m_WayPointData[i].WayPoints[waypoint].WayPoint.position, this.transform.position);
                        waypoint = j;
                    }
                }
                if (Vector3.Distance(this.transform.position, m_WayPointData[i].WayPoints[waypoint].WayPoint.position) <= 0.5f)
                    return;
                m_WayCount = waypoint;
                if(m_Hammer.activeSelf)
                {
                    m_Hammer.SetActive(false);
                }
                ChangeState(Slave_State.STATE_ROAM);
            }
        }
    }
    

    IEnumerator ActionProcess(String p_Trigger, String p_AniName)
    {
        m_SlaveAni.SetTrigger(p_Trigger);
        //StartCoroutine(FollowTarget(m_SlaveLookTarget, p_Trigger));

        m_SlaveAgent.isStopped = true;
        m_SlaveAgent.updatePosition = false;
        m_SlaveAgent.updateRotation = false;
        m_SlaveAgent.velocity = Vector3.zero;

        //����ǰ��մ�
        while (!m_SlaveAni.GetCurrentAnimatorStateInfo(0).IsName(p_AniName))
        {
            yield return null;
        }
        //�ִϸ��̼� ��� �� ����Ǵ� �κ�
        while (m_SlaveAni.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }


        //���� 
        m_SlaveAgent.isStopped = false;
        m_SlaveAgent.updatePosition = true;
        m_SlaveAgent.updateRotation = true;
        IsAni = false;
        m_TargetNoiseLv = 0;
        IsSurprised = false;

        //���� ��������Ʈ�� �̵�
        NextWayPoint();
    }


    #endregion;

    #region �� �� �Լ���

    private void OnDrawGizmosSelected()
    {
        if(IsNoiseGizmos)
        {
            for (int i = 0; i < m_NoiseSet.Length; i++)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(this.transform.position, m_NoiseSet[i].Distance);
            }
        }
    }
    void StopMoveing()
    {
        m_SlaveAgent.speed = 0.1f;
    }

    void UndoMoveSpeed()
    {
        m_SlaveAgent.speed = m_MoveSpeed;
    }
    public void SurprisedEvent()
    {
        IsAni = false;
    }

    public void StartFlag()
    {
        m_StopSlave = false;
    }

    public void EndFlag()
    {
        CancelInvoke("NextWayPoint");
        CancelInvoke("NoiseSystem");
        m_SlaveAgent.ResetPath();
    }

    #endregion;
}


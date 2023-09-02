using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// 작성자: 서동주
public class SlaveAI : MonoBehaviour
{
    #region 변수 

    //사용할 NavAgent 설정
    NavMeshAgent m_SlaveAgent = null;
    // npc 이동속도
    [Header("[이동속도]")] public float m_MoveSpeed = 5.0f;

    //웨이포인트 데이터
    [SerializeField]
    [Header("[웨이포인트설정]")] public WayPointData[] m_WayPointData = new WayPointData[1];
    //맵 진행 카운트
    int m_MapCount = 0;
    //웨이포인트 진행 카운트
    int m_WayCount = 0;

    //Noise시스템 세팅
    [SerializeField]
    [Header("[소음시스템Gizmos OnOff]")] bool IsNoiseGizmos;
    [SerializeField]
    [Header("[소음시스템설정]")] NoiseDetectionSetting[] m_NoiseSet;
    //소음으로 인식할 레이어
    [SerializeField]
    [Header("[소리를 인식할 레이어 마스크]")] LayerMask m_NoiseLayerMask = 0;
    //어느정도의 소음레벨에 노예가 놀랄지설정
    [SerializeField]
    [Header("[소리를 인식할 레이어 마스크]")]  int m_SurpriseLv;
    //타겟된 노이즈의 레벨
    public int m_TargetNoiseLv;
    //설정된 최대 레벨
    int m_MaxNoiseLv;
    //최대 탐지거리
    float m_MaxDistance = 0f;


    //타겟
    public Transform m_FollowingTarget;
    Transform m_Target;
    Transform m_NoiseTarget;
    Transform m_TempNoiseTarget;
    bool IsAni;
    bool IsNotArrival;
    bool IsWarning = false;
    bool IsSurprised = false;

    //애니메이션 컨트롤
    Animator m_SlaveAni;

    public bool m_StopSlave = true;

    //참조할 오브젝트
    public GameObject m_WorkBenchObj; //작업대
    public Transform m_WorkBenchTr;
    public GameObject m_Hammer;

    Coroutine RunnigCoroutine;
    E_SlaveAction m_SlaveAction;
    Transform m_SlaveLookTarget;

    // 상태 설정 
    public enum Slave_State
    {
        STATE_IDLE,          //기본 사용x
        STATE_WORK,         //작업 상태
        STATE_TARGET,       //타겟 추적 상태
        STATE_ROAM,         //순찰 상태
        STATE_TEMP          //쓰래기 상태 사용x
    }

    //시작 상태 설정
    public Slave_State startState = Slave_State.STATE_WORK;
    //현재 상태
    private Slave_State currentState;

    //이전 상태 호출용
    Action<StateFlow> m_PreviousCallFN = null;
    //현재 상태 호출용
    Action<StateFlow> m_CurrentCallFN = null;
    //상태 리스트
    List<Action<StateFlow>> m_AllCallFNList = new List<Action<StateFlow>>();

    //이전 스테이트 저장후 비교
    Slave_State m_CurrentStateType = Slave_State.STATE_TEMP;
    Slave_State m_PreviousStateType = Slave_State.STATE_TEMP;

    bool startFlag = false;

    #endregion;

    #region State(Start,Update,Change)

    // 접속 , 진행 , 나가기
    private enum StateFlow
    {
        ENTER,
        UPDATE,
        EXIT
    }

    // 시작 상태 설정
    private void SetStartState(Slave_State startState)
    {
        m_CurrentCallFN = m_AllCallFNList[(int)startState];
        m_CurrentCallFN(StateFlow.ENTER);
    }

    // 업데이트
    void Update()
    {
        if (m_StopSlave)
            return;

        CommonUpdate();

        //현재 진행하는 상태
        if(m_CurrentCallFN!=null)
        {
            m_CurrentCallFN(StateFlow.UPDATE);
        }
    }

    
    // 상태 변경
    private void ChangeState(Slave_State nextState)
    {
        //진행중인 상태와 변경할 상태가 다를경우만 진행
        if (m_CurrentStateType == nextState)
            return;
        
        //이전 상태 저장
        m_PreviousStateType = m_CurrentStateType;
        m_PreviousCallFN = m_CurrentCallFN;
        //현재상태 변경
        m_CurrentStateType = nextState;
        m_CurrentCallFN = m_AllCallFNList[(int)nextState];

        //진행중이던 상태 exit 실행
        m_PreviousCallFN(StateFlow.EXIT);

        //상태 진입 알림
        m_CurrentCallFN(StateFlow.ENTER);

    }

    #endregion;

    #region Awake, Start, Update State

    //기본설정
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
        //필요한 소음시스템 관련 변수 설정
        for (int i = 0; i < m_NoiseSet.Length; i++)
        {
            if (m_MaxNoiseLv < m_NoiseSet[i].DetectionLv)
                m_MaxNoiseLv = m_NoiseSet[i].DetectionLv;
            if (m_MaxDistance < m_NoiseSet[i].Distance)
                m_MaxDistance = m_NoiseSet[i].Distance;
        }

        IsAni = true;

        //list 빈공간 추가
        for (int i = 0; i < ((int)(Slave_State.STATE_TEMP))+1; i++)
        {
            m_AllCallFNList.Add(null);
        }

        //순서대로 대입
        m_AllCallFNList[(int)Slave_State.STATE_IDLE] = STATE_IDLE;
        m_AllCallFNList[(int)Slave_State.STATE_WORK] = STATE_WORK;
        m_AllCallFNList[(int)Slave_State.STATE_TARGET] = STATE_TARGET;
        m_AllCallFNList[(int)Slave_State.STATE_ROAM] = STATE_ROAM;
        m_AllCallFNList[(int)Slave_State.STATE_TEMP] = STATE_TEMP;

        //시작 상태 설정
        SetStartState(startState);

        //이전 상태 설정
        m_PreviousStateType = Slave_State.STATE_WORK;

        //노이즈 시스템 반복
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
                //워크밴치로 이동(거리비교)
                if(Vector3.Distance(m_WorkBenchObj.transform.position,this.transform.position)>=0.3f)
                {
                    //워크밴치(작업하는곳)로 이동
                    m_SlaveAni.ResetTrigger("WorkEndTRG");
                    m_SlaveAgent.SetDestination(m_WorkBenchTr.position);
                    m_SlaveAni.SetTrigger("WalkTRG");
                }
                break;
            case StateFlow.UPDATE:
                //타겟 체크 
                TargetCheck();
                //이동한게 멈췄는지확인 멈췄으면 
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
        //타겟설정
        switch (stateFlow)
        {
            case StateFlow.ENTER:
                //걷기 애니메이션 설정
                m_SlaveAni.SetTrigger("WalkTRG");
                break;
            case StateFlow.UPDATE:
                //도착확인
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

                    // 상태변경
                    ChangeState(Slave_State.STATE_ROAM);
                }
                break;
            case StateFlow.EXIT:
                break;
        }
    }

    private void STATE_ROAM(StateFlow stateFlow)
    {
        //순찰
        switch (stateFlow)
        {
            case StateFlow.ENTER:
                //웨이포인트 이동 설정
                NextWayPoint();
                break;
            case StateFlow.UPDATE:
                //타겟이 있는지 확인
                TargetCheck();

                //타겟이 있으면 다음진행x
                if (m_Target != null || m_TempNoiseTarget != null || m_NoiseTarget != null)
                    return;

                //도착
                if (m_SlaveAgent.velocity.sqrMagnitude >= 0.25f * 0.25f && m_SlaveAgent.remainingDistance <= 0.25f)
                {
                    //도착했는데 설정된 애니메이션이 없을 경우
                    if(m_SlaveAction == E_SlaveAction.Null)
                    {
                        //다음 웨이포인트로 이동
                        NextWayPoint();
                    }
                    else
                    {
                        //설정된 애니메이션 출력
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

    #region 소음 관련 함수

    //소음 시스템
    void NoiseSystem()
    {
        bool IsNoise;
        Transform Targert;
        int tempNoiseLv;

        if (m_StopSlave)
            return;

        //소음시스템보다 우위에 있어야 되는 일반 추적 (추후 변경가능)
        if (m_Target != null)
            return;

        //소음감지
        (IsNoise, Targert, tempNoiseLv) = NoiseDetection();

        if (IsNoise)
        {
            //타겟의 소음레벨
            m_TargetNoiseLv = tempNoiseLv;
            //타겟설정
            SetNoiseTarget(Targert);
        }

    }

    //소음 감지
    (bool, Transform, int) NoiseDetection()
    {//감지결과, 타겟위치정보, 소음레벨
        //콜라이더 감지(특정 레이어만)
        Collider[] colliders = Physics.OverlapSphere(transform.position, m_MaxDistance, m_NoiseLayerMask);

        //감지가 된 콜라이더가 있을경우
        if (colliders.Length > 0)
        {
            //제일 높은레벨의 감지구역부터 탐색
            for (int i = m_MaxNoiseLv; 0 < i; i--)
            {
                //찾는 소음레벨의 탐지거리를 찾기위한 변수
                float FindDistance = 0;
                //해당 레벨에 맞는 거리 찾기(세팅에서)
                for (int j = 0; j < m_NoiseSet.Length; j++)
                {
                    if (m_NoiseSet[j].DetectionLv == i)
                    {
                        FindDistance = m_NoiseSet[j].Distance;
                    }
                }
                //감지된 콜라이더만큼 반복
                for (int j = 0; j < colliders.Length; j++)
                {
                    //레벨에 맞는 거리에 들어와 있는 소음인지 확인
                    if (colliders[j].GetComponent<NoiseData>().m_NoiseLv == i &&
                        Vector3.Distance(colliders[j].gameObject.transform.position, this.transform.position) <= FindDistance)
                    {
                        //타겟으로 설정된 소음이 없을경우
                        if (m_TargetNoiseLv == 0)
                        {
                            return (true, colliders[j].transform, i);
                        }
                        else //타겟으로 설정된 소음이 있지만
                        {
                            //타겟으로 설정된 소음보다 소음레벨이 클 경우 타겟변경
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
        //감지된 타겟없음
        return (false, transform, 0);
    }

    //소음타겟설정
    public void SetNoiseTarget(Transform p_target)
    {
        if (!p_target)
        {
            return;
        }
        // 1레벨은 엄청 근접한 소음임으로 바로 플레이어를 추격한다.
        if(m_TargetNoiseLv==1)
        {
            m_Target = Player_Control.GetInstance.transform;
        }

        //타겟 소음레벨이 놀라야되는 레벨보다 높다면
        else if (m_TargetNoiseLv >= m_SurpriseLv)
        {
            //임시타겟으로 설정
            if (m_TempNoiseTarget != null)
                return;

            m_TempNoiseTarget = p_target;
            
            if (RunnigCoroutine==null)
            {
                //노예npc가 놀라게 설정
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

        //서프라이즈 실행 및 움직임 멈춤
        m_SlaveAni.SetTrigger("SurprisedTRG");
        m_SlaveAgent.isStopped = true;
        m_SlaveAgent.updatePosition = false;
        m_SlaveAgent.updateRotation = false;
        m_SlaveAgent.velocity = Vector3.zero;

        //실행되고잇는게 서프라이즈인지 확인
        while (!m_SlaveAni.GetCurrentAnimatorStateInfo(0).IsName("Surprised"))
        {
            yield return null;
        }
        //애니메이션 재생 중 실행되는 부분
        while (m_SlaveAni.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.80f)
        {
            yield return null;
        }

        //서프라이즈 종료 
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
        //실행되고잇는게 일종료인지 확인
        while (!m_SlaveAni.GetCurrentAnimatorStateInfo(0).IsName("WorkEnd"))
        {
            yield return null;
        }

        m_Hammer.SetActive(false);

        //애니메이션 재생 중 실행되는 부분
        while (m_SlaveAni.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }

        //일종료 애니메이션 종료 
        m_SlaveAgent.isStopped = false;
        m_SlaveAgent.updatePosition = true;
        m_SlaveAgent.updateRotation = true;
        RunnigCoroutine = null;
        IsAni = false;
        IsSurprised = false;
        TargetCheck();
    }


    #endregion;

    #region 타겟 관련 함수
    //타겟 확인
    void TargetCheck()
    {
        // 정지 설정 확인
        if (m_StopSlave)
            return;

        if (m_Target != null) //타겟이 있으면
        {
            if(m_FollowingTarget!=m_Target)
            {
                //타겟을 따라감
                StartCoroutine(SetTarget(m_Target));
            }
            //타겟해제 조건
            if(Vector3.Distance(m_Target.position, this.transform.position)>10.0f)
            {
                m_Target = null;
                ChangeState(Slave_State.STATE_ROAM);
            }
        }
        else if (m_NoiseTarget != null)
        {
            //따라가고있는 타겟이랑 비교
            if(m_FollowingTarget!=m_NoiseTarget)
            {
                //타겟설정
                StartCoroutine(SetTarget(m_NoiseTarget));
                IsNotArrival = true;
            }
            
        }
        else
        {
            if (m_TempNoiseTarget != null && !IsAni)
            {
                //임시 소음 타겟을 소음타겟으로 변경
                IsAni = true;
                m_NoiseTarget = m_TempNoiseTarget;
                m_TempNoiseTarget = null;
            }
        }

        //타겟은 없지만 도착을 안했을경우
        if (IsNotArrival)
        {
            //도착 했으면
            if (m_SlaveAgent.velocity.sqrMagnitude >= 0.2f * 0.2f && m_SlaveAgent.remainingDistance <= 0.5f)
            {
                //타겟 리셋설정
                IsNotArrival = false;
                ResetTarget();
            }

        }
    }

    //타겟설정
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
    //타겟바라보기 (타겟 Transform, 행동할 애니메이션 트리거 명)
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
                        //실행되고잇는
                        while (!m_SlaveAni.GetCurrentAnimatorStateInfo(0).IsName("Work Start"))
                        {
                            yield return null;
                        }
                        //애니메이션 재생 중 실행되는 부분
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
                //Debug.Log("Limit 값 : " + Limit);

                if (Limit > 90.0f)//우측회전
                {
                    if (!m_SlaveAni.GetCurrentAnimatorStateInfo(0).IsName("RightTurn"))
                    {
                       // m_SlaveAni.SetTrigger("RightTurnTRG");
                    }
                }
                else if (Limit < -90.0f)//좌측회전
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

    #region 순찰 관련 함수
    void NextWayPoint()
    {
        //다음 웨이포인트로 이동
        m_SlaveAni.SetTrigger("WalkTRG");
        m_SlaveAction = m_WayPointData[m_MapCount].WayPoints[m_WayCount].Action;
        m_SlaveLookTarget = m_WayPointData[m_MapCount].WayPoints[m_WayCount].LookObj;
        m_SlaveAgent.SetDestination(m_WayPointData[m_MapCount].WayPoints[m_WayCount++].WayPoint.position);
        //다 실행했으면 초기화
        if (m_WayCount >= m_WayPointData[m_MapCount].WayPoints.Length)
        {
            m_WayCount = 0;
        }
    }

    //방이 바뀔대 사용(노예가 움직일 방)
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

                    //제일 가까운 웨이포인트 찾기
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

        //실행되고잇는
        while (!m_SlaveAni.GetCurrentAnimatorStateInfo(0).IsName(p_AniName))
        {
            yield return null;
        }
        //애니메이션 재생 중 실행되는 부분
        while (m_SlaveAni.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }


        //종료 
        m_SlaveAgent.isStopped = false;
        m_SlaveAgent.updatePosition = true;
        m_SlaveAgent.updateRotation = true;
        IsAni = false;
        m_TargetNoiseLv = 0;
        IsSurprised = false;

        //다음 웨이포인트로 이동
        NextWayPoint();
    }


    #endregion;

    #region 그 외 함수들

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


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SearchType
{
    Sentry, //보초
    Patrol, //순찰
    All
}

/// <summary>
/// 탐색을 지속적으로 하고있는 상태스크립트
/// </summary>
public class EnemySearch : StateAbstractClass
{
    /*
     
    순찰 시스템은 몬스터의 순찰 타입에 따라 나눠서 구현을 목표로 한다.

    1) 경계 구역(자기자리)을 지키면서 경계하는 타입 
    2) 순찰 구역을 돌아다니면서 경계하는 타입

     */

   public EnemyInfo enemyInfo;

    // 저장된 웨이포인트 Patrol 전용
    // 둘다 인스펙터창에서 숨겨주지만 기능은 다르다
    // HideInInspector -> 인스팩터에 적용된 에디터 내용 적용 o
    // NonSerialized    -> 인스펙터에 적용된 에디터 내용 적용 x
    public List<Vector3> wayPoint;
    int wayCount = 0;

    [NonSerialized] public Vector3 returnPos;

    // Sentry 전용 변수
    Vector3 sentryDefaultPos;
    Vector3 sentryDefaultRot;
    // Head 
    public Transform Head = null;
    Transform DefaultTrHead = null;

    // 타겟이 확인되면 추격
    public Transform target = null;

    // 선택된 순찰타입
    public SearchType selectSearchType = SearchType.All;

    // 코루틴 저장 변수
    private IEnumerator patrolCoroutine = null;

    // 중복실행 방지
    public bool isFeelThePresence = false;
    bool isPatrolSystem = false;


    // 경계회전관련 변수
    [SerializeField] bool isBoundary = false;
    [SerializeField] private float boundaryAngle = 30.0f;
    [SerializeField][Range(0.1f, 5.0f)] private float boundarySpeed = 2.5f;
    Vector3[] sentryBoundaryList;
    int sentryBoundaryCount = 0;

    bool isDefaultPos = true; // 필요없는 반복 실행 방지
    bool isDefaultRot = true;

    bool isSentryRot = false;
    bool isBodyRotation = false;

    // 이동속도 관련 순찰, 추적
    float patrolSpeed = 1.5f;
    float trackingSpeed = 3.0f;


    // 시작시 순찰시스템 
    void Awake()
    {
        Debug.Log("Awake - wayPoint Count: " + wayPoint.Count);
        enemyInfo = GetComponent<EnemyInfo>();
        // 타입에 따른 변수 초기화
        if (selectSearchType == SearchType.Sentry)
        {
            sentryDefaultPos = transform.position;
            sentryDefaultRot = transform.forward;
            returnPos = this.transform.position;
            SetSentryBoundary();
        }

        if(Head!=null)
        {
            DefaultTrHead = new GameObject("Head_OrientationReference").transform;
            DefaultTrHead.position = Head.position;
            DefaultTrHead.rotation = Head.rotation;
            DefaultTrHead.SetParent(Head);
        }
    }
    private void Start()
    {
        Debug.Log("Start - wayPoint Count: " + wayPoint.Count);
    }

    public override void StateEnter()
    {
        StartPatrol();
    }

    public override void StateExit()
    {
        StopPatrol();
    }

    Vector3 AngleToDir(float angle)
    {
        float radian = (angle + transform.eulerAngles.y) * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(radian), 0f, Mathf.Cos(radian));
    }

    void SetSentryBoundary()
    {
        sentryBoundaryList = new Vector3[2];
        sentryBoundaryList[0] = AngleToDir(-boundaryAngle * 0.5f);
        sentryBoundaryList[1] = AngleToDir(boundaryAngle * 0.5f);
    }

    private void StartPatrol()
    {
        if (patrolCoroutine == null)
        {
            StartCoroutine(patrolCoroutine = PatrolSystem());
            return;
        }
        StartCoroutine(PatrolSystem());
    }

    private void StopPatrol()
    {
        StopCoroutine(patrolCoroutine);
        isPatrolSystem = false;
        if (wayCount == 0)
        {
            wayCount = wayPoint.Count - 1;
        }
        else if (wayCount >= 1)
        {
            wayCount--;
        }
        enemyInfo.navMeshAgent.ResetPath();
    }


    public override void StateUpdate()
    {
        if(enemyInfo.isAnimator)
        {
            enemyInfo.animator.SetFloat(enemyInfo.aniHashSpeed, enemyInfo.navMeshAgent.velocity.magnitude);
        }
    }

    public override void StateLateUpdate()
    {
        // 애니메이션 호환성을 위해 애니메이션 처리 후 머리 방향 조절
        if(isSentryRot && !enemyInfo.target)
        {
            SentryRotation();
        }
    }

    public override void StateFixedUpdate()
    {
    }

    // 의심해서 돌아보는 코루틴
    public IEnumerator FeelThePresence(Vector3 target)
    {
        if (!isFeelThePresence)
            isFeelThePresence = true;
        else
            yield break;

        StopPatrol();
        yield return null;

        enemyInfo.navMeshAgent.ResetPath();

        Vector3 lookDirection = new Vector3(target.x, this.transform.position.y, target.z) - this.transform.position;
        Quaternion defaultBodyRot = transform.rotation;
        Quaternion defaultHeadRot = Head.rotation;
        isBodyRotation = true;
        isSentryRot = false;

        for (int i = 0; i< 100; i++)
        {
            // 머리와 바디 매칭
            transform.rotation = Quaternion.Slerp(defaultBodyRot, Quaternion.LookRotation(lookDirection), 0.01f*i);
            Head.rotation = Quaternion.Slerp(defaultHeadRot, Quaternion.LookRotation(lookDirection), 0.01f * i);

            if (enemyInfo.target !=null)
            {
                isFeelThePresence = false;
                StartPatrol();
                yield break;
            }

            yield return new WaitForSeconds(0.01f);
        }

       
        yield return new WaitForSeconds(1.5f);
        isFeelThePresence = false;
        StartPatrol();
    }

    
    // 순찰 시스템
    private IEnumerator PatrolSystem()
    {
        // 혹시 모를 중복 실행 방지
        if (!isPatrolSystem)
            isPatrolSystem = true;
        else
            yield break;

        if (isSentryRot)
        {
            isBodyRotation = true;
            isSentryRot = false;
        }

        while (true)
        {
            if(isFeelThePresence)
            {
                break;
            }
            // 타겟이 있으면 추적
            if (enemyInfo.target != null )
            {
                enemyInfo.navMeshAgent.speed = trackingSpeed;
                if(Head!=null)
                {
                    Head.rotation = Quaternion.LookRotation(this.transform.forward);
                }
                if(selectSearchType==SearchType.Patrol)
                {
                    returnPos = this.transform.position;
                }
                enemyInfo.stateSystem.ChangeState(EnemyState.Tracking);
                enemyInfo.navMeshAgent.SetDestination(enemyInfo.target.transform.position);
                break;
            }
            // 타겟이 없고 속도가 0일 경우 ( 추적 종료 )
            else if (enemyInfo.navMeshAgent.velocity == Vector3.zero)
            {
                enemyInfo.navMeshAgent.speed = patrolSpeed;
                switch (selectSearchType)
                {
                    case SearchType.Sentry:
                        if(SentryMovement())
                        {
                            yield return null;
                        }
                        else
                        {
                            isSentryRot = true;
                            yield return null;
                        }
                        break;

                    case SearchType.Patrol:
                        enemyInfo.navMeshAgent.SetDestination(wayPoint[wayCount]);
                        returnPos = wayPoint[wayCount];
                        wayCount++;
                        if (wayCount >= wayPoint.Count)
                        {
                            wayCount = 0;
                        }
                        yield return new WaitForSeconds(0.5f);
                        break;

                    default:
                        break;
                }
            }
            yield return null;
        }

    }

    // 센트리 타입 움직임 함수
    bool SentryMovement()
    {
        if (Vector3.Distance(this.transform.position, sentryDefaultPos) > 0.15f)
        {
            enemyInfo.navMeshAgent.SetDestination(sentryDefaultPos);
            isDefaultPos = false;
            return true;
        }
        else if ( !isDefaultPos && Vector3.Distance(this.transform.position, sentryDefaultPos) < 0.15f)
        {
            this.transform.position = sentryDefaultPos;
            isDefaultPos = true;
            return false;
        }

        return false;
    }

    // 센트리 타입 회전 함수
    void SentryRotation()
    {
        Transform watcher;
        Vector3 target;
        if (isBoundary)
        {
            target = sentryBoundaryList[sentryBoundaryCount];
        }
        else
        {
            target = sentryDefaultRot;
        }
        
        if(Head)
        {
            watcher = Head;
        }
        else
        {
            watcher = this.transform;
        }

        if(isBodyRotation)
        {
            if (Vector3.Angle(this.transform.forward, sentryDefaultRot) > 1.5f)
            {
                transform.rotation = Quaternion.Lerp(Quaternion.LookRotation(transform.forward),
                  Quaternion.LookRotation(sentryDefaultRot), boundarySpeed * Time.deltaTime);
            }
            else
            {
                transform.rotation = Quaternion.LookRotation(sentryDefaultRot);
                isBodyRotation = false;
            }
        }
        else
        {
            if (Vector3.Angle(watcher.forward, target) > 1.5f)
            {
                watcher.rotation = Quaternion.Lerp(Quaternion.LookRotation(watcher.forward),
                  Quaternion.LookRotation(target), boundarySpeed * Time.deltaTime);

            }
            else
            {
                watcher.rotation = Quaternion.LookRotation(target);

                if (isBoundary)
                {
                    sentryBoundaryCount++;

                    if (sentryBoundaryCount == sentryBoundaryList.Length)
                        sentryBoundaryCount = 0;
                }
            }
        }

        

    }

}

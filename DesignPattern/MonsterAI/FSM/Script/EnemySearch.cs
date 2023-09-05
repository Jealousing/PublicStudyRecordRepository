using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SearchType
{
    Sentry, //����
    Patrol, //����
    All
}

/// <summary>
/// Ž���� ���������� �ϰ��ִ� ���½�ũ��Ʈ
/// </summary>
public class EnemySearch : StateAbstractClass
{
    /*
     
    ���� �ý����� ������ ���� Ÿ�Կ� ���� ������ ������ ��ǥ�� �Ѵ�.

    1) ��� ����(�ڱ��ڸ�)�� ��Ű�鼭 ����ϴ� Ÿ�� 
    2) ���� ������ ���ƴٴϸ鼭 ����ϴ� Ÿ��

     */

   public EnemyInfo enemyInfo;

    // ����� ��������Ʈ Patrol ����
    // �Ѵ� �ν�����â���� ���������� ����� �ٸ���
    // HideInInspector -> �ν����Ϳ� ����� ������ ���� ���� o
    // NonSerialized    -> �ν����Ϳ� ����� ������ ���� ���� x
    public List<Vector3> wayPoint;
    int wayCount = 0;

    [NonSerialized] public Vector3 returnPos;

    // Sentry ���� ����
    Vector3 sentryDefaultPos;
    Vector3 sentryDefaultRot;
    // Head 
    public Transform Head = null;
    Transform DefaultTrHead = null;

    // Ÿ���� Ȯ�εǸ� �߰�
    public Transform target = null;

    // ���õ� ����Ÿ��
    public SearchType selectSearchType = SearchType.All;

    // �ڷ�ƾ ���� ����
    private IEnumerator patrolCoroutine = null;

    // �ߺ����� ����
    public bool isFeelThePresence = false;
    bool isPatrolSystem = false;


    // ���ȸ������ ����
    [SerializeField] bool isBoundary = false;
    [SerializeField] private float boundaryAngle = 30.0f;
    [SerializeField][Range(0.1f, 5.0f)] private float boundarySpeed = 2.5f;
    Vector3[] sentryBoundaryList;
    int sentryBoundaryCount = 0;

    bool isDefaultPos = true; // �ʿ���� �ݺ� ���� ����
    bool isDefaultRot = true;

    bool isSentryRot = false;
    bool isBodyRotation = false;

    // �̵��ӵ� ���� ����, ����
    float patrolSpeed = 1.5f;
    float trackingSpeed = 3.0f;


    // ���۽� �����ý��� 
    void Awake()
    {
        Debug.Log("Awake - wayPoint Count: " + wayPoint.Count);
        enemyInfo = GetComponent<EnemyInfo>();
        // Ÿ�Կ� ���� ���� �ʱ�ȭ
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
        // �ִϸ��̼� ȣȯ���� ���� �ִϸ��̼� ó�� �� �Ӹ� ���� ����
        if(isSentryRot && !enemyInfo.target)
        {
            SentryRotation();
        }
    }

    public override void StateFixedUpdate()
    {
    }

    // �ǽ��ؼ� ���ƺ��� �ڷ�ƾ
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
            // �Ӹ��� �ٵ� ��Ī
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

    
    // ���� �ý���
    private IEnumerator PatrolSystem()
    {
        // Ȥ�� �� �ߺ� ���� ����
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
            // Ÿ���� ������ ����
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
            // Ÿ���� ���� �ӵ��� 0�� ��� ( ���� ���� )
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

    // ��Ʈ�� Ÿ�� ������ �Լ�
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

    // ��Ʈ�� Ÿ�� ȸ�� �Լ�
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;

public class SearchBTExample : BehaviorTree
{
    /* BlackBoard에서 받아올 데이터 */
    BasicInfo target;

    /* 인스펙터에서 설정할 옵션 */ 
    public float viewDistance = 10.0f;
    [SerializeField] SphereCollider searchCollider;
    [SerializeField] WayPoint wayPointData;

    /* 나머지 */
    List<BasicInfo> targetList = new List<BasicInfo>();
    NavMeshAgent navMeshAgent;
    FSMBase info;
    LayerMask exceptionLayer;
    string playerTag = "Player"; 

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        info = GetComponent<FSMBase>();

        exceptionLayer = ~(1 << LayerMask.NameToLayer("Ignore Raycast"));
    }
    public override void Evaluate()
    {
        base.Evaluate();

        info.animator.SetFloat("Speed", navMeshAgent.velocity.magnitude);
    } 

    public override void InitializeTree(Blackboard blackboard)
    { 
        // 한번만 갱신하면 되는 부분
        if (!isInit)
        {
            isInit = true;
            this.blackboard = blackboard;
            searchCollider.radius = viewDistance;

            rootNode = new SequenceNode
            (
                new ConditionNode(() => CheckTarget()),
                new ActionNode(() => Patrol()) 
            );
        }
         
        // 진입마다 체크해야 되는 부분
        target = blackboard.Get<BasicInfo>("target"); 
        navMeshAgent.speed = 3.5f;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            BasicInfo temp;
            if (other.TryGetComponent<BasicInfo>(out temp) &&
                !targetList.Contains(temp))
            {
                targetList.Add(temp);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            BasicInfo temp;
            if (other.TryGetComponent<BasicInfo>(out temp) &&
                targetList.Contains(temp))
            {
                targetList.Remove(temp);
                if (target == temp)
                {
                    target = null;
                    blackboard.Set("target", target);
                }
            }
        }
    }

    protected bool CheckTarget()
    {
        // 타겟이 있으면 추격으로 상태전환
        if (target != null)
        {
            if (this.TryGetComponent(out ChaseState state))
            {
                blackboard.Set("target", target);
                info.ChangeState(state);
                return false;
            } 
        }

        // 타겟리스트를 확인해서 타겟리스트에 하나라도 들어있을경우 제일 가까운 오브젝트를 타겟으로 설정
        if (targetList.Count > 0)
        {
            List<BasicInfo> sortedTargets = targetList.OrderBy(temp => Vector3.Distance(temp.transform.position, transform.position)).ToList();
            foreach (BasicInfo temp in sortedTargets)
            {
                RaycastHit hit;

                if (Physics.SphereCast(transform.position + transform.up, 2f, (temp.transform.position - transform.position).normalized,
                    out hit, viewDistance, exceptionLayer, QueryTriggerInteraction.Ignore) && hit.transform.Equals(temp.transform))
                {
                    target = temp;
                    blackboard.Set("target", target);
                    NotifyNearbyMonsters();
                    return false;
                }
            }
        }

        // 추가로 웨이포인트(순찰루트)가 있는지 확인
        if (wayPointData == null) return false;
        if (wayPointData.wayPoint.Count > 0) return true;

        return false;
    }

    protected void NotifyNearbyMonsters()
    {
        LayerMask enemyLayer = LayerMask.GetMask("Enemy");
        Collider[] colliders = Physics.OverlapSphere(transform.position, 3f, enemyLayer, QueryTriggerInteraction.Ignore);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].TryGetComponent(out BTEnemyInfo temp) && temp.target == null)
            {
                temp.target = target;
            }
        }
    }

    protected void Patrol()
    {
        // 순찰 동작을 여기에 구현
        // 예시: NavMeshAgent를 이용하여 웨이포인트 간 이동
        navMeshAgent.isStopped = false;
        // 웨이포인트로 이동하는 코드 추가
        if (navMeshAgent.velocity == Vector3.zero)
        {
            navMeshAgent.SetDestination(wayPointData.wayPoint[wayPointData.wayCount++]);
            if (wayPointData.wayCount >= wayPointData.wayPoint.Count)
            {
                wayPointData.wayCount = 0;
            }
        }
    } 
}

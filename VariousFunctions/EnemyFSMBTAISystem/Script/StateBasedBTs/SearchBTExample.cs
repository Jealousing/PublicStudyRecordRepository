using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;

public class SearchBTExample : BehaviorTree
{
    /* BlackBoard���� �޾ƿ� ������ */
    BasicInfo target;

    /* �ν����Ϳ��� ������ �ɼ� */ 
    public float viewDistance = 10.0f;
    [SerializeField] SphereCollider searchCollider;
    [SerializeField] WayPoint wayPointData;

    /* ������ */
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
        // �ѹ��� �����ϸ� �Ǵ� �κ�
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
         
        // ���Ը��� üũ�ؾ� �Ǵ� �κ�
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
        // Ÿ���� ������ �߰����� ������ȯ
        if (target != null)
        {
            if (this.TryGetComponent(out ChaseState state))
            {
                blackboard.Set("target", target);
                info.ChangeState(state);
                return false;
            } 
        }

        // Ÿ�ٸ���Ʈ�� Ȯ���ؼ� Ÿ�ٸ���Ʈ�� �ϳ��� ���������� ���� ����� ������Ʈ�� Ÿ������ ����
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

        // �߰��� ��������Ʈ(������Ʈ)�� �ִ��� Ȯ��
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
        // ���� ������ ���⿡ ����
        // ����: NavMeshAgent�� �̿��Ͽ� ��������Ʈ �� �̵�
        navMeshAgent.isStopped = false;
        // ��������Ʈ�� �̵��ϴ� �ڵ� �߰�
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

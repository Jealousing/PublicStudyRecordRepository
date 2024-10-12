using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using UnityEngine.AI;

public class ChaseBTExample : BehaviorTree
{
    /* BlackBoard에서 받아올 데이터 */
    Blackboard blackboard;
    BasicInfo target;

    /* 인스펙터에서 설정할 옵션 */
    public float minChaseDistace;
    public float maxChaseDistace;

    /* 나머지 */
    NavMeshAgent navMeshAgent;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    public override void InitializeTree(Blackboard blackboard)
    {
        if(blackboard==null) this.blackboard = blackboard;
        target = blackboard.Get<BasicInfo>("target");

        // 행동 트리 구성
        rootNode = new SequenceNode(
            new ConditionNode(() => TargetValidation()),
            new ActionNode(() => RotateTowardsPlayer()),
            new ActionNode(() => MoveTowardsPlayer())
        );
    }


    bool TargetValidation()
    {
        if(target !=null)
        {
            float distance = Vector3.Distance(this.transform.position, target.transform.position);
            if (distance <minChaseDistace)
            { 
                if(this.TryGetComponent(out CombatState state))
                { 
                    blackboard.Get<FSMBase>("State").ChangeState(state);
                    return false;
                }  
            }
            else if(distance >maxChaseDistace)
            { 
                if (this.TryGetComponent(out SearchState state))
                {
                    blackboard.Get<FSMBase>("State").ChangeState(state);
                    return false;
                }  
            }
            return true;
        }
        else
        { 
            if (this.TryGetComponent(out SearchState state))
            {
                blackboard.Get<FSMBase>("State").ChangeState(state);
                return false;
            }
            else
            {
                Debug.LogWarning("SearchState is not set.");
                return false;
            }
        }
    }
    void RotateTowardsPlayer() 
    {
        Vector3 lookPos = target.transform.position - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * navMeshAgent.angularSpeed);
    }
    void MoveTowardsPlayer() 
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.SetDestination(target.transform.position); 
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using UnityEngine.AI;
 
public class ChaseBTExample : BehaviorTree
{
    /* BlackBoard���� �޾ƿ� ������ */ 
    BasicInfo target;

    /* �ν����Ϳ��� ������ �ɼ� */
    public float minChaseDistace;
    public float maxChaseDistace;

    /* ������ */
    NavMeshAgent navMeshAgent; 
    FSMBase info;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();  
        info = GetComponent<FSMBase>();
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

            // �ൿ Ʈ�� ����
            rootNode = new SequenceNode(
                new ConditionNode(() => TargetValidation()),
                new ActionNode(() => RotateTowardsPlayer()),
                new ActionNode(() => MoveTowardsPlayer())
            );

            blackboard.Set("maxChaseDistace", maxChaseDistace);
        }

        // ���Ը��� üũ�ؾ� �Ǵ� �κ�
        target = blackboard.Get<BasicInfo>("target");
        navMeshAgent.speed = 5;
    }

    public override void Exit()
    {
       target = null;
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
                    info.ChangeState(state);
                    return false;
                }  
            }
            else if(distance >maxChaseDistace)
            { 
                if (this.TryGetComponent(out SearchState state))
                {
                    target = null;
                    blackboard.Set("target", target);
                    navMeshAgent.ResetPath();
                    info.ChangeState(state);
                    return false;
                }  
            }
            return true;
        }
        else
        { 
            if (this.TryGetComponent(out SearchState state))
            {
                info.ChangeState(state);
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
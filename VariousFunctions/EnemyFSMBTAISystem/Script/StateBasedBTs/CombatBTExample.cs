using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CombatBTExample : BehaviorTree 
{
    /* BlackBoard���� �޾ƿ� ������ */
    BasicInfo target;

    /* �ν����Ϳ��� ������ �ɼ� */
    [SerializeField] private float basicAttackDistance;


    /* ������ */ 
    NavMeshAgent navMeshAgent;
    FSMBase info;

    bool isPattern = false;
    float maxChaseDistace;
    HealthPointPattern healthPointPattern;
    ConditionPattern conditionPattern;
    CooldownPattern cooldownPattern; 
    
    // �Ϲݰ���
    BasicAttackPattern basicAttackPattern;


    #region Event Functions
    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>(); 
        info = GetComponent<FSMBase>();
    }

    // Update()
    public override void Evaluate()
    { 
        base.Evaluate();
        info.animator.SetFloat("Speed", navMeshAgent.velocity.magnitude);
    }
    #endregion
     
    public override void InitializeTree(Blackboard blackboard)
    {
        // �ѹ��� �����ϸ� �Ǵ� �κ�
        if (!isInit)
        {
            isInit = true;
            this.blackboard = blackboard;

            maxChaseDistace = blackboard.Get<float>("maxChaseDistace");

            healthPointPattern = new HealthPointPattern(blackboard);
            conditionPattern = new ConditionPattern(blackboard);
            cooldownPattern = new CooldownPattern(blackboard);
            basicAttackPattern = new BasicAttackPattern(blackboard);
              
            rootNode = new SelectorNode
                ( 
                     // �� ����� �켱������ ���� ���ϵ��� Ȯ���� ���

                     // ü�� ���� ( ��ȸ��)
                     healthPointPattern.SequencePattern(),

                     // ���� ���� ( ���Ǹ� �ȴٸ� ��� ��� )
                     conditionPattern.SequencePattern(),

                     // ��Ÿ�� ���� ( �����ʿ���� �ð����� ����ϴ� ���ϵ� )
                     cooldownPattern.SequencePattern(),

                     // �Ϲݰ��� ����
                     basicAttackPattern.SequencePattern()
                );

            AddPattern();
        }

        // ���Ը��� üũ�ؾ� �Ǵ� �κ�
        navMeshAgent.speed = 5;
    }

    private void AddPattern()
    {
        healthPointPattern.ClearPattern();
        healthPointPattern.AddPattern(new Tuple<float, Action>(0.4f, () => StartCoroutine(HPRecovery())));

        conditionPattern.ClearPattern();

        cooldownPattern.ClearPattern();

        basicAttackPattern.ClearPattern();
        basicAttackPattern.AddPattern(() => StartCoroutine(BiteAttack()));
        basicAttackPattern.AddPattern(() => StartCoroutine(BiteAttack()));
        basicAttackPattern.AddPattern(() => StartCoroutine(TailAttack()));
    }

    private bool CheckTarget()
    {
        // ������������ Ÿ���� ������ �ٽ� SearchState�� ���ư���
        if ((target = blackboard.Get<BasicInfo>("target")) == null
            || Vector3.Distance(transform.position, target.transform.position) > maxChaseDistace)
        {
            if (this.TryGetComponent(out SearchState state)) blackboard.Get<FSMBase>("State").ChangeState(state);
            return true;
        }
        return false;
    }

    private void StartPattern()
    {
        isPattern = true;
        blackboard.Set("isPattern", isPattern);
    }

    private void EndPattern()
    {
        isPattern = false;
        navMeshAgent.isStopped = true;
        blackboard.Set("isPattern", isPattern);
    }

    #region HealthPoint Pattern
    IEnumerator HPRecovery()
    { 
        StartPattern();
        navMeshAgent.isStopped = true;
        yield return null; 

        string aniName = "Scream";
        info.animator.SetTrigger(aniName);

        while (!info.animator.GetCurrentAnimatorStateInfo(0).IsTag(aniName))
        {
            yield return null;
        }

        while (info.animator.GetCurrentAnimatorStateInfo(0).IsTag(aniName) &&
            info.animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.5f)
        { 
            yield return null;
        }

        info.HP += info.maxHP / 2f;

        yield return new WaitForSeconds(1.0f);
        EndPattern();

        // hp ȸ�� ����Ʈ �߰� �ʿ�
        for (int i = 0; i < 20; i++)
        {
            info.HP += info.maxHP / 20.0f;
            yield return new WaitForSeconds(3.0f);
        }
         
    }
    #endregion

    #region Condition Pattern

    #endregion

    #region Cooldown Pattern

    #endregion

    #region BasicAttack Pattern
    IEnumerator BiteAttack()
    {
        if (CheckTarget()) yield break;
        StartPattern();
        float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
        Vector3 lastTargetPosition = target.transform.position;  

        // target���� �Ÿ��� basicAttackDistance���� �ִٸ� �̵�
        if (distanceToTarget > basicAttackDistance)
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(lastTargetPosition);  

            while (distanceToTarget > basicAttackDistance)
            {
                if (CheckTarget()) EndPattern(); 
                if (Vector3.Distance(lastTargetPosition, target.transform.position) > 0.25f)
                {
                    lastTargetPosition = target.transform.position;  
                    navMeshAgent.SetDestination(lastTargetPosition);  
                }

                distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
                yield return null;
            }
        }

        navMeshAgent.isStopped = true;
         
        Vector3 lookPos = target.transform.position - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);

        string aniName = "BiteAttack";
        info.animator.SetTrigger(aniName); 


        while (!info.animator.GetCurrentAnimatorStateInfo(0).IsTag(aniName))
        {
            yield return null;
        }

        while (info.animator.GetCurrentAnimatorStateInfo(0).IsTag(aniName) &&
            info.animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.3f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 15f);
            yield return null;
        }

        DamageManager.GetInstance.DamageBuilder(DamageRangeType.CUBE)
            .SetTargetLayer(LayerMask.GetMask("Player"))
            .SetIsDraw(true)
            .SetFillTime(0.25f)
            .SetDamage(30)
            .SetPos(this.transform.position + this.transform.forward * 1.5f)
            .SetRot(this.transform.rotation)
            .SetMinDistance( 4f )
            .SetMaxDistance( 10f )
            .SetWidth( 4f )
            .SetHeight( 15f )
            .Build();

        while (info.animator.GetCurrentAnimatorStateInfo(0).IsTag(aniName) &&
            info.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.8f)
        {
            yield return null;
        }
          
        // ������ ������ n�ʵڿ� �ٽ� ������ �����ϵ��� ����
        yield return new WaitForSeconds(1.0f);
        EndPattern(); 
    }  

    IEnumerator TailAttack()
    {
        StartPattern();
        float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
        Vector3 lastTargetPosition = target.transform.position;

        // target���� �Ÿ��� basicAttackDistance���� �ִٸ� �̵�
        if (distanceToTarget > basicAttackDistance)
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(lastTargetPosition);  

            while (distanceToTarget > basicAttackDistance)
            {
                if (CheckTarget()) EndPattern(); 
                if (Vector3.Distance(lastTargetPosition, target.transform.position) > 0.25f)
                {
                    lastTargetPosition = target.transform.position;  
                    navMeshAgent.SetDestination(lastTargetPosition);  
                }

                distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
                yield return null;
            }
        }
        navMeshAgent.isStopped = true;

        Vector3 lookPos = target.transform.position - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);

        info.animator.SetTrigger("TailAttack"); 
        string aniName = "TailAttack";

        while (!info.animator.GetCurrentAnimatorStateInfo(0).IsTag(aniName))
        {
            yield return null;
        }

        while (info.animator.GetCurrentAnimatorStateInfo(0).IsTag(aniName) &&
            info.animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.3f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 15f);
            yield return null;
        }

        DamageManager.GetInstance.DamageBuilder(DamageRangeType.CUBE)
            .SetTargetLayer(LayerMask.GetMask("Player"))
            .SetIsDraw(true)
            .SetFillTime(0.25f)
            .SetDamage(30)
            .SetPos(this.transform.position + this.transform.forward * 1.5f)
            .SetRot(this.transform.rotation)
            .SetMinDistance(4f)
            .SetMaxDistance(10f)
            .SetWidth(4f)
            .SetHeight(15f)
            .Build();

        while (info.animator.GetCurrentAnimatorStateInfo(0).IsTag(aniName) &&
            info.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.8f)
        {
            yield return null;
        } 

        // ������ ������ n�ʵڿ� �ٽ� ������ �����ϵ��� ����
        yield return new WaitForSeconds(1.0f);
        EndPattern();
    }
    #endregion
}

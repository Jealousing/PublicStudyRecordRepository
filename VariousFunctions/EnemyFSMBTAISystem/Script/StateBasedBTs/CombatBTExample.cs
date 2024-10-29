using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CombatBTExample : BehaviorTree 
{
    /* BlackBoard에서 받아올 데이터 */
    BasicInfo target;

    /* 인스펙터에서 설정할 옵션 */
    [SerializeField] private float basicAttackDistance;


    /* 나머지 */ 
    NavMeshAgent navMeshAgent;
    FSMBase info;

    bool isPattern = false;
    float maxChaseDistace;
    HealthPointPattern healthPointPattern;
    ConditionPattern conditionPattern;
    CooldownPattern cooldownPattern; 
    
    // 일반공격
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
        // 한번만 갱신하면 되는 부분
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
                     // 각 사용할 우선순위에 따른 패턴들을 확인후 사용

                     // 체력 패턴 ( 일회성)
                     healthPointPattern.SequencePattern(),

                     // 조건 패턴 ( 조건만 된다면 계속 사용 )
                     conditionPattern.SequencePattern(),

                     // 쿨타임 패턴 ( 조건필요없이 시간마다 사용하는 패턴들 )
                     cooldownPattern.SequencePattern(),

                     // 일반공격 패턴
                     basicAttackPattern.SequencePattern()
                );

            AddPattern();
        }

        // 진입마다 체크해야 되는 부분
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
        // 패턴진행전에 타겟이 없으면 다시 SearchState로 돌아가기
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

        // hp 회복 이팩트 추가 필요
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

        // target과의 거리가 basicAttackDistance보다 멀다면 이동
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
          
        // 공격이 끝나도 n초뒤에 다시 공격이 가능하도록 설정
        yield return new WaitForSeconds(1.0f);
        EndPattern(); 
    }  

    IEnumerator TailAttack()
    {
        StartPattern();
        float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
        Vector3 lastTargetPosition = target.transform.position;

        // target과의 거리가 basicAttackDistance보다 멀다면 이동
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

        // 공격이 끝나도 n초뒤에 다시 공격이 가능하도록 설정
        yield return new WaitForSeconds(1.0f);
        EndPattern();
    }
    #endregion
}

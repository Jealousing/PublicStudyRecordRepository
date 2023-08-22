using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : StateAbstractClass
{
    [SerializeField] PatternInfo patternInfo;
    EnemyInfo enemyInfo;
    private void Awake()
    {
        enemyInfo = GetComponent<EnemyInfo>();
    }
    public override void StateEnter()
    {
        enemyInfo.animator.SetFloat(enemyInfo.aniHashSpeed,0);
        enemyInfo.navMeshAgent.ResetPath();
    }

    public override void StateExit()
    {
    }

    public override void StateFixedUpdate()
    {
    }
    public override void StateLateUpdate()
    {
        if (patternInfo.IsPattern)
        {
            Vector3 lookDirection = enemyInfo.target.transform.position - this.transform.position;
            lookDirection.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection.normalized, Vector3.up);
            transform.rotation = targetRotation;
        }
    }
    public override void StateUpdate()
    {
        // 공격하는 부분
        /*
        우선순위) 
        1) 패턴이 진행중인지 확인
        2) 타겟확인 및 상태변경
        3) 체력에 따른 패턴
            체력에 따른 패턴은 보통 일정 체력이 되었을 때 발동하는 패턴.
        4) 스킬처럼 가지고있는 몬스터의 고유 패턴
            사용 횟수에 제한은 없지만 스킬쿨이 따로있고 쿨에 따른 패턴을 진행하는부분
        5) 일반공격
            위의 패턴들이 진행중이지 않을때 발동하는 기본적인 공격
            일반 공격중에는 따로 확인해서 중간에 끊도록 설정
         */

        if (patternInfo.IsPattern) return;

        // 다음 프레임을 진행해도 되는지 여부
        if (!checkTarget())
        {
            enemyInfo.stateSystem.ChangeState(EnemyState.Search);
            return;
        }

        if (Vector3.Distance(this.transform.position, enemyInfo.target.transform.position) > enemyInfo.attackDistance)
        {
            enemyInfo.stateSystem.ChangeState(EnemyState.Tracking);
            return;
        }

        if(patternInfo.HpPattern())
        {
            return;
        }

        if (patternInfo.CooldownPattern())
        {
            return;
        }

        if (patternInfo.BasicAttack())
        {
            return;
        }


    }

    bool checkTarget()
    {
        return enemyInfo.target != null;
    }
}

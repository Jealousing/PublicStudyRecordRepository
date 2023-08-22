using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 추격상태 스크립트
/// </summary>
public class EnemyTracking : StateAbstractClass
{
    /*
    추격에서 해야될 내용
     
    Enter:

    Exit:

    Update:
    타겟이 존재하는지 확인
    1) 존재한다면 타겟과의 거리 확인->
        1-1) 거리가 일정이하 ( 공격 패턴을 실행할 수 있는 거리 )면 ChangeState을 이용해 공격패턴으로 변경
        1-2) 거리가 일정이상 ( 공격 패턴을 실행할 수 없는 거리 ) 면 해당 타겟을 추격
    2) 존재하지 않는다면 ->
        1-1) 탐색상태로 변경

     */

    EnemyInfo enemyInfo;


    private void Awake()
    {
        enemyInfo = GetComponent<EnemyInfo>();
    }
    public override void StateEnter()
    {
    }

    public override void StateExit()
    {
    }

    public override void StateFixedUpdate()
    {
    }

    public override void StateLateUpdate()
    {
        if (enemyInfo.navMeshAgent.velocity == Vector3.zero) return;
        Vector3 lookDirection = enemyInfo.navMeshAgent.velocity.normalized;

        Quaternion targetRotation = Quaternion.LookRotation(lookDirection, Vector3.up);
        if (Quaternion.Angle(transform.rotation, targetRotation) > 60f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
        else
        {
            transform.rotation = targetRotation;
        }


    }

    public override void StateUpdate()
    {
        if (enemyInfo.isAnimator)
        {
            enemyInfo.animator.SetFloat(enemyInfo.aniHashSpeed, enemyInfo.navMeshAgent.velocity.magnitude);
        }
        // 타겟이 존재하는지 확인
        if (checkTarget())
        {
            // 거리확인
            float distance = Vector3.Distance(this.transform.position, enemyInfo.target.transform.position);
            
            // 공격가능거리
            if (distance< enemyInfo.attackDistance)
            {
                enemyInfo.stateSystem.ChangeState(EnemyState.AttackPattern);
            }
            else
            {
                if(Vector3.Distance(enemyInfo.navMeshAgent.destination , enemyInfo.target.transform.position) > 0.3f)
                {
                    enemyInfo.navMeshAgent.SetDestination(enemyInfo.target.transform.position);
                }
            }
        }
        // 타겟이 없으면
        else
        {
            enemyInfo.stateSystem.ChangeState(EnemyState.Search);
        }
    }

    bool checkTarget()
    {
        return enemyInfo.target != null;
    }
}

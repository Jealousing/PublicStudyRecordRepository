using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �߰ݻ��� ��ũ��Ʈ
/// </summary>
public class EnemyTracking : StateAbstractClass
{
    /*
    �߰ݿ��� �ؾߵ� ����
     
    Enter:

    Exit:

    Update:
    Ÿ���� �����ϴ��� Ȯ��
    1) �����Ѵٸ� Ÿ�ٰ��� �Ÿ� Ȯ��->
        1-1) �Ÿ��� �������� ( ���� ������ ������ �� �ִ� �Ÿ� )�� ChangeState�� �̿��� ������������ ����
        1-2) �Ÿ��� �����̻� ( ���� ������ ������ �� ���� �Ÿ� ) �� �ش� Ÿ���� �߰�
    2) �������� �ʴ´ٸ� ->
        1-1) Ž�����·� ����

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
        // Ÿ���� �����ϴ��� Ȯ��
        if (checkTarget())
        {
            // �Ÿ�Ȯ��
            float distance = Vector3.Distance(this.transform.position, enemyInfo.target.transform.position);
            
            // ���ݰ��ɰŸ�
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
        // Ÿ���� ������
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

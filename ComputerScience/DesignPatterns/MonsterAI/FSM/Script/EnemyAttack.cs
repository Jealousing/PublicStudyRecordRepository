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
        // �����ϴ� �κ�
        /*
        �켱����) 
        1) ������ ���������� Ȯ��
        2) Ÿ��Ȯ�� �� ���º���
        3) ü�¿� ���� ����
            ü�¿� ���� ������ ���� ���� ü���� �Ǿ��� �� �ߵ��ϴ� ����.
        4) ��ųó�� �������ִ� ������ ���� ����
            ��� Ƚ���� ������ ������ ��ų���� �����ְ� �� ���� ������ �����ϴºκ�
        5) �Ϲݰ���
            ���� ���ϵ��� ���������� ������ �ߵ��ϴ� �⺻���� ����
            �Ϲ� �����߿��� ���� Ȯ���ؼ� �߰��� ������ ����
         */

        if (patternInfo.IsPattern) return;

        // ���� �������� �����ص� �Ǵ��� ����
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

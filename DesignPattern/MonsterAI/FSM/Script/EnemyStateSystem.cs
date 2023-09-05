using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    ReSpawn,
    Search,
    Tracking,
    AttackPattern,

    TempState
}

/// <summary>
/// �߻�Ŭ������ �̿��� ���¸ӽ� ����
/// ���� ���¸ӽ����� �����ϴ� Ŭ����
/// </summary>
public class EnemyStateSystem : MonoBehaviour
{
    /*
     ������ �����ߴ� �׼Ǹ���Ʈ�� Ȱ���� ������ �ٸ����� 
    ������ ���°��� �ý��ۿ��� �ҷ����°� ���� �Լ� ( enter exit update ��)�� �ҷ����� ������
    �߻�Ŭ������ �̿��ؼ� �ٿ�ĳ������ ���� �ҷ������ ����

    �⺻������ ���� Ž���ϴ� ��ũ��Ʈ�� �߰� ��ũ��Ʈ�� �� �ȿ��� ������ �޶����� ������
    ū Ʋ�� �ȹٲ�� ������ ���������� ��밡���ϰ�

    ����(����) ���´� ������ ��ä�� ���� �޶��� �� �ֱ� ������ �ν����� â���� 
    �ش� ��ü�� �˸´� ���α����� ���ݻ��½�ũ��Ʈ�� �������� �� �ְ� �ȴ�.
     
     */
    private EnemyState startEnemyStateType = EnemyState.Search;
    public EnemyState enemyCurrentStateEnum;
    public EnemyState enemyPreviousStateEnum;

    // �θ� �߻�Ŭ���� �ڽ�Ŭ�������� 
    public StateAbstractClass EnemyCurrentState;
    private List<StateAbstractClass> EnemyStateList = new();

    public StateAbstractClass enemySearchState;
    private EnemyTracking enemyTracking;
    private EnemyAttack enemyAttack;

    private void Awake()
    {
        enemyTracking = GetComponent<EnemyTracking>();
        enemyAttack = GetComponent<EnemyAttack>();
    }

    void Start()
    {
       
        for (int i = 0; i < ((int)(EnemyState.TempState)) + 1; i++)
        {
            EnemyStateList.Add(null);
        }

        EnemyStateList[(int)EnemyState.Search] = enemySearchState;
        EnemyStateList[(int)EnemyState.Tracking] = enemyTracking;
        EnemyStateList[(int)EnemyState.AttackPattern] = enemyAttack;

        EnemyCurrentState = EnemyStateList[(int)startEnemyStateType];
        EnemyCurrentState?.StateEnter();
        enemyCurrentStateEnum = startEnemyStateType;
    }

    private void FixedUpdate()
    {
        EnemyCurrentState?.StateFixedUpdate();
    }

    private void Update()
    {
        EnemyCurrentState?.StateUpdate();
    }

    private void LateUpdate()
    {
        EnemyCurrentState?.StateLateUpdate();
    }

    /// <summary>
    /// ���� ���� ������ ���¸� �����´�.
    /// </summary>
    public EnemyState GetCurrentEnemyState()
    {
        return enemyCurrentStateEnum;
    }

    /// <summary>
    /// ���� ���� ������ ���¸� �����Ѵ�.
    /// </summary>
    public void ChangeState(EnemyState nextState)
    {
        //�������� ���¿� ������ ���°� �ٸ���츸 ����
        if (enemyCurrentStateEnum == nextState)
            return;

        //���� ���� ����
        enemyPreviousStateEnum = enemyCurrentStateEnum;

        //������� ����
        enemyCurrentStateEnum = nextState;
        EnemyCurrentState = EnemyStateList[(int)nextState];

        //�������̴� ���� exit ����
        EnemyCurrentState?.StateExit();


        //���� ���� �˸�
        EnemyCurrentState?.StateEnter();
    }

}

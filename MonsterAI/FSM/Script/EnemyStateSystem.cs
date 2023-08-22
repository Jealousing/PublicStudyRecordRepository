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
/// 추상클래스를 이용한 상태머신 구현
/// 적을 상태머신으로 관리하는 클래스
/// </summary>
public class EnemyStateSystem : MonoBehaviour
{
    /*
     이전에 구현했던 액션리스트를 활용한 구현과 다른점은 
    어차피 상태관리 시스템에서 불러오는건 같은 함수 ( enter exit update 등)를 불러오기 때문에
    추상클래스를 이용해서 다운캐스팅을 통한 불러오기로 구현

    기본적으로 적을 탐색하는 스크립트와 추격 스크립트는 그 안에서 설정만 달라지는 것이지
    큰 틀은 안바뀌기 때문에 공통적으로 사용가능하고

    공격(패턴) 상태는 몬스터의 개채에 따라 달라질 수 있기 때문에 인스펙터 창에서 
    해당 개체에 알맞는 따로구현된 공격상태스크립트를 지정해줄 수 있게 된다.
     
     */
    private EnemyState startEnemyStateType = EnemyState.Search;
    public EnemyState enemyCurrentStateEnum;
    public EnemyState enemyPreviousStateEnum;

    // 부모 추상클래스 자식클래스에서 
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
    /// 현재 적의 움직임 상태를 가져온다.
    /// </summary>
    public EnemyState GetCurrentEnemyState()
    {
        return enemyCurrentStateEnum;
    }

    /// <summary>
    /// 현재 적의 움직임 상태를 변경한다.
    /// </summary>
    public void ChangeState(EnemyState nextState)
    {
        //진행중인 상태와 변경할 상태가 다를경우만 진행
        if (enemyCurrentStateEnum == nextState)
            return;

        //이전 상태 저장
        enemyPreviousStateEnum = enemyCurrentStateEnum;

        //현재상태 변경
        enemyCurrentStateEnum = nextState;
        EnemyCurrentState = EnemyStateList[(int)nextState];

        //진행중이던 상태 exit 실행
        EnemyCurrentState?.StateExit();


        //상태 진입 알림
        EnemyCurrentState?.StateEnter();
    }

}

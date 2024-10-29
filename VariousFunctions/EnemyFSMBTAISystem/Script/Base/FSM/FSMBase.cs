using System;
using System.Collections;
using System.Collections.Generic; 
using UnityEngine;
  

public class FSMBase: BasicInfo
{
    public FSMState currentState; 
    private Blackboard blackboard;

    [SerializeField] float setMaxHP; 

    public override float maxHP
    {
        get => setMaxHP;
        set => maxHP = value;
    }

    public override float recoveryHP => 10;
    public override float maxMP => 0;
    public override float recoveryMP => 0;
    private void Awake()
    {
        base.Setting();
    }
    void Start()
    {
        blackboard = new Blackboard();
        animator = GetComponent<Animator>();

        blackboard.Set("info",this);
        if (TryGetComponent(out IdleState state)) ChangeState(state);
    }
    
    void Update()
    {
        currentState.BTUpdate();
    }

    public void ChangeState(FSMState newState)
    {
        if (newState == null) return;

        currentState?.Exit();
        currentState = newState;
        currentState?.Enter(blackboard);
    }

    public override void TakeDamage(float damage)
    {
        // 설정된 타겟이 없을때 공격받으면 타겟설정하도록

        if (!gameObject.activeSelf) return;
        HP -= damage; 
        UIDamageTextManager.GetInstance.GetDamageText((int)damage, this);
    }
}
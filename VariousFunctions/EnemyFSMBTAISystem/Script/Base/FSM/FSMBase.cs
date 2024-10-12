using System;
using System.Collections;
using System.Collections.Generic; 
using UnityEngine;
  

public class FSMBase: MonoBehaviour
{
    public FSMState currentState; 
    private Blackboard blackboard;
    void Start()
    {
        blackboard = new Blackboard();
        blackboard.Set("State", this);
    }
    
    void Update()
    {
        currentState.Update();
    }

    public void ChangeState(FSMState newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter(blackboard);
    }
}
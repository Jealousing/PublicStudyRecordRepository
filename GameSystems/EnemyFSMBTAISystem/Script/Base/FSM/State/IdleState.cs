using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : FSMState
{ 
    public override void Enter(Blackboard blackboard)
    {
        base.blackboard = blackboard;
        behaviorTree.InitializeTree(blackboard);
    }
    public override void Exit()
    {
        behaviorTree.Exit();
    }
    public override void FixedUpdate() { }
    public override void LateUpdate() { }
}

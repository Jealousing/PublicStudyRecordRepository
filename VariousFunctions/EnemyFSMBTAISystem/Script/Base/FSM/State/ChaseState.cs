using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChaseState : FSMState
{ 
    public override void Enter(Blackboard blackboard) 
    {
        base.blackboard = blackboard;
        behaviorTree.InitializeTree(blackboard);
    }
    public override void Exit() { }
    public override void FixedUpdate() { }
    public override void LateUpdate() { }
}
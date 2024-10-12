using System.Collections;
using System.Collections.Generic;
using UnityEngine;
  
public abstract class FSMState : MonoBehaviour
{ 
    public BehaviorTree behaviorTree;
    protected Blackboard blackboard; 
    public abstract void Enter(Blackboard blackboard);
    public virtual void Update() => behaviorTree.Evaluate();
    public abstract void FixedUpdate();
    public abstract void LateUpdate();
    public abstract void Exit();
}

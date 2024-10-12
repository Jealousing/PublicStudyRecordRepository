using System;
using UnityEngine;


// 노드상태 (진행중, 성공, 실패)
public enum NodeState
{
    RUNNING,
    SUCCESS,
    FAILURE
}

// 행동 트리 베이스 추상 클래스
public abstract class BehaviorTreeNode 
{
    public abstract NodeState Evaluate();
}

public abstract class BehaviorTree : MonoBehaviour
{
    protected BehaviorTreeNode rootNode; 
    public abstract void InitializeTree(Blackboard blackboard);  
    public virtual void Evaluate() => rootNode.Evaluate();
}
 

// 조건 노드 클래스
public class ConditionNode : BehaviorTreeNode
{
    private Func<bool> condition;

    public ConditionNode(Func<bool> condition)
    {
        this.condition = condition;
    }

    public override NodeState Evaluate()
    {
        return condition() ? NodeState.SUCCESS : NodeState.FAILURE;
    }
}

// 액션 노드 클래스
public class ActionNode : BehaviorTreeNode
{
    private Action action;

    public ActionNode(Action action)
    {
        this.action = action;
    }

    public override NodeState Evaluate()
    {
        action();
        return NodeState.SUCCESS;
    }
}

// 시퀀스 노드 클래스
public class SequenceNode : BehaviorTreeNode
{
    private BehaviorTreeNode[] childNodes;

    public SequenceNode(params BehaviorTreeNode[] nodes)
    {
        childNodes = nodes;
    }

    public override NodeState Evaluate()
    {
        foreach (BehaviorTreeNode node in childNodes)
        {
            if (node.Evaluate() == NodeState.FAILURE)
            {
                return NodeState.FAILURE;
            }
        }
        return NodeState.SUCCESS;
    }
}

// 셀렉터 노드 클래스
public class SelectorNode : BehaviorTreeNode
{
    private BehaviorTreeNode[] childNodes;

    public SelectorNode(params BehaviorTreeNode[] nodes)
    {
        childNodes = nodes;
    }

    public override NodeState Evaluate()
    {
        foreach (BehaviorTreeNode node in childNodes)
        {
            if (node.Evaluate() == NodeState.SUCCESS)
            {
                return NodeState.SUCCESS;
            }
        }
        return NodeState.FAILURE;
    }
}
using System;
using UnityEngine;


// ������ (������, ����, ����)
public enum NodeState
{
    RUNNING,
    SUCCESS,
    FAILURE
}

// �ൿ Ʈ�� ���̽� �߻� Ŭ����
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
 

// ���� ��� Ŭ����
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

// �׼� ��� Ŭ����
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

// ������ ��� Ŭ����
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

// ������ ��� Ŭ����
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
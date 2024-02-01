using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  ���̽� ����
     1) ��Ʈ ���(Root Node): �ൿƮ���� �������̸� �ַ� ������ ���, �����ʹ� ���� ���� ����� �ϳ��� �����Ͽ� ����
     2) ������ ���(Selector Node): ���� ���� ����߿��� �ϳ� �̻��� �����ϴ� ����̸� �ڽ� ��带 ��ȸ�ϸ� ���� ������ ù ��° ��带 ã�� �����ϰ� ���� or ���� ��ȯ
     3) ������ ���(Sequence Node): ���� ���� ��带 ���������� �����ϸ� ��� �ڽĳ�尡 �����ϸ� ������ ��ȯ�ϸ�, �ϳ��� �����ϸ� ���и� ��ȯ
     4) ���� ���(Condition Node): ������ ���Ͽ� ���� �Ǵ� ���и� ��ȯ�ϴ� ����̸� ���� �ÿ��� ���� ��带 ����ؼ� �����ϰ� ���� �ÿ��� ��Ʈ��ŷ�ϰų� �ٸ� �б⸦ Ž���մϴ�.
     5) �׼� ���(Action Node): Ư�� �ൿ�� ���� ��Ű�� ����̸� �׻� ������ ��ȯ�Ѵ�.

 *  ������ Ʈ�� �������
     ��Ʈ ��� ���� -> 
 */

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
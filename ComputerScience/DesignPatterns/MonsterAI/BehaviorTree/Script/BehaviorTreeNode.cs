using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  베이스 설명
     1) 루트 노드(Root Node): 행동트리의 시작점이며 주로 셀렉터 노드, 셀렉터는 여러 하위 노드중 하나를 선택하여 실행
     2) 셀렉터 노드(Selector Node): 여러 하위 노드중에서 하나 이상을 실행하는 노드이며 자식 노드를 순회하며 실행 가능한 첫 번째 노드를 찾아 실행하고 성공 or 실패 반환
     3) 스퀸스 노드(Sequence Node): 여러 하위 노드를 순차적으로 실행하며 모든 자식노드가 성공하면 성공을 반환하며, 하나라도 실패하면 실패를 반환
     4) 조건 노드(Condition Node): 조건을 평가하여 성공 또는 실패를 반환하는 노드이며 성공 시에는 다음 노드를 계속해서 실행하고 실패 시에는 백트래킹하거나 다른 분기를 탐색합니다.
     5) 액션 노드(Action Node): 특정 행동을 실행 시키는 노드이며 항상 성공을 반환한다.

 *  비헤비어 트리 실행순서
     루트 노드 실행 -> 
 */

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
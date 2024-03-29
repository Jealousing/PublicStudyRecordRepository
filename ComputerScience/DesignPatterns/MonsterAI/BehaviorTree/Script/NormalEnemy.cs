using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class NormalEnemy : BTEnemyInfo
{
    protected virtual void Awake()
    {
        isAnimator = TryGetComponent<Animator>(out animator);
        TryGetComponent<NavMeshAgent>(out navMeshAgent);
        TryGetComponent<Collider>(out coliderInfo);
        originalColorsAndMaterials = new List<(Color, Material)>();
        Renderer[] allRenderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in allRenderers)
        {
            foreach (Material mat in renderer.materials)
            {
                if (mat.HasProperty("_Color"))
                {
                    originalColorsAndMaterials.Add((mat.color, mat));
                }
                else
                {
                    originalColorsAndMaterials.Add((Color.black, mat));
                }
            }
        }

        Setting();
    }

    protected virtual void Start()
    {
        // 행동 트리 구조 설정

        // 순찰 노드
        SequenceNode patrolNode = new SequenceNode
        (
            new ConditionNode(() => IsPatrolTime()),
            new ActionNode(() => Patrol()),
            new ActionNode(() => RotateRandomly())
        );

        // 타겟확인 노트
        SequenceNode chasePlayerNode = new SequenceNode
        (
            new ConditionNode(() => CanSeePlayer()),
            new ActionNode(() => RotateTowardsPlayer()),
            new ActionNode(() => MoveTowardsPlayer())
        );

        // 공격 노드
        SequenceNode attackNode = new SequenceNode
        (
            new ConditionNode(() => CanAttackPlayer()),
            new ActionNode(() => StopMoving()),
            new ActionNode(() => AttackPlayer())
         );

        behaviorTree = new SelectorNode
        (
            patrolNode,
            chasePlayerNode,
            attackNode
        );
    }
    protected virtual void Update()
    {
        if (deathState != DeathState.Alive) return;
        // 매 프레임마다 행동 트리 실행
        behaviorTree.Evaluate();
    }
}

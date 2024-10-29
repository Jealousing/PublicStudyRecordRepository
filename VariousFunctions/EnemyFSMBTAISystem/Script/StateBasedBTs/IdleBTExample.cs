using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleBTExample : BehaviorTree
{
    /* BlackBoard���� �޾ƿ� ������ */


    /* �ν����Ϳ��� ������ �ɼ� */


    /* ������ */
    FSMBase info;

    private void Start()
    { 
        info = GetComponent<FSMBase>();
    }
    public override void InitializeTree(Blackboard blackboard)
    {
        // �ѹ��� �����ϸ� �Ǵ� �κ�
        if (!isInit)
        {
            isInit = true;
            this.blackboard = blackboard;

            rootNode = new SequenceNode(
                new ActionNode(()=> ChangeStateToChase())
                );
        }

        // ���Ը��� üũ�ؾ� �Ǵ� �κ�
    }

    void ChangeStateToChase()
    {
        if (this.TryGetComponent(out SearchState state))
        {
            info.ChangeState(state);
        }
    }
}

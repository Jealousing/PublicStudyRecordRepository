using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleBTExample : BehaviorTree
{
    /* BlackBoard에서 받아올 데이터 */


    /* 인스펙터에서 설정할 옵션 */


    /* 나머지 */
    FSMBase info;

    private void Start()
    { 
        info = GetComponent<FSMBase>();
    }
    public override void InitializeTree(Blackboard blackboard)
    {
        // 한번만 갱신하면 되는 부분
        if (!isInit)
        {
            isInit = true;
            this.blackboard = blackboard;

            rootNode = new SequenceNode(
                new ActionNode(()=> ChangeStateToChase())
                );
        }

        // 진입마다 체크해야 되는 부분
    }

    void ChangeStateToChase()
    {
        if (this.TryGetComponent(out SearchState state))
        {
            info.ChangeState(state);
        }
    }
}

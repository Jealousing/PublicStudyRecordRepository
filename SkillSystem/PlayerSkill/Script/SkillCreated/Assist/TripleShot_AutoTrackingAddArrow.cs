using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleShot_AutoTrackingAddArrow : AssistSkill
{
    // TripleShot�� ����ϸ� AutomaticTrackingArrows��ų�� ȭ�� �߰������� �ش罺ų�� ������ ���� �߰�
    public override void Assist(ActiveSkill activeSkill)
    {
        (activeSkill as TripleShot).autoTrackingArrowCount = 2 + this.skillData.skillInfo.skillLV * 2;
    }
}

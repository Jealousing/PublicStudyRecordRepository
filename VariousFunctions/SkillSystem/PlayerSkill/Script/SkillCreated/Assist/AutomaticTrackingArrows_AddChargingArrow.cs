using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticTrackingArrows_AddChargingArrow : AssistSkill
{
    // ��¡Ÿ�Ӹ��� ��¡�� ȭ�� ���� �÷��ִ� ��ų
    public override void Assist(ActiveSkill activeSkill)
    {
        (activeSkill as AutomaticTrackingArrows).chargingArrowNumber = 1 + this.skillData.skillInfo.skillLV;
    }
}

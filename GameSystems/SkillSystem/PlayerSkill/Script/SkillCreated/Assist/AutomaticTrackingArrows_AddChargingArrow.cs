using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticTrackingArrows_AddChargingArrow : AssistSkill
{
    // 차징타임마다 차징할 화살 갯수 늘려주는 스킬
    public override void Assist(ActiveSkill activeSkill)
    {
        (activeSkill as AutomaticTrackingArrows).chargingArrowNumber = 1 + this.skillData.skillInfo.skillLV;
    }
}

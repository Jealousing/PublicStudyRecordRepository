using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticTrackingArrows_AddMaxNumArrow : AssistSkill
{
    public override void Assist(ActiveSkill activeSkill)
    {
        AutomaticTrackingArrows temp = activeSkill as AutomaticTrackingArrows;
        temp.addMaxArrowNumber = skillData.skillInfo.skillLV;
        temp.ResetArraySizeArrow();
    }

}

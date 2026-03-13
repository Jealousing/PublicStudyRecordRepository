using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleShot_AutoTrackingAddArrow : AssistSkill
{
    // TripleShot을 사용하면 AutomaticTrackingArrows스킬의 화살 추가갯수를 해당스킬의 레벨에 따라 추가
    public override void Assist(ActiveSkill activeSkill)
    {
        (activeSkill as TripleShot).autoTrackingArrowCount = 2 + this.skillData.skillInfo.skillLV * 2;
    }
}

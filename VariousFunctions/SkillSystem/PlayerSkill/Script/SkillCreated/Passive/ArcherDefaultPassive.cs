using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherDefaultPassive : PassiveSkill
{
    public override void Apply()
    {
        PlayerInfo.GetInstance.playerStats.passiveBowAddDamage += 100*skillData.skillInfo.skillLV;
    }

}

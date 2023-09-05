using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CombatAbstract : MonoBehaviour
{
    protected CombatInfo combatInfo;
    protected IEnumerator attackCoroutine = null;
    protected int maxComboAttack = 3;
    protected int attackCount = 0;
    protected string aniName;


    private void Awake()
    {
        combatInfo = GetComponent<CombatInfo>();
    }

    protected abstract void BasicAttackSystem();
}

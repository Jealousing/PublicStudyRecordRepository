using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyPattern<T>
{
    protected Blackboard blackboard;
    public EnemyPattern(Blackboard blackboard)
    {
        this.blackboard = blackboard;
    } 
    public abstract SequenceNode SequencePattern();
    public abstract void ClearPattern();
    public abstract void AddPattern(T pattern); 
}
public class PatternCooldown
{
    public float lastActivationTime;
    public float cooldownDuration;

    public PatternCooldown(float lastActivationTime, float cooldownDuration)
    {
        this.lastActivationTime = lastActivationTime;
        this.cooldownDuration = cooldownDuration;
    }
}

public class HealthPointPattern : EnemyPattern<Tuple<float, Action>>
{
    private Queue<Tuple<float, Action>> patternHealthPoint = new Queue<Tuple<float, Action>>();
    FSMBase info;
    public HealthPointPattern(Blackboard blackboard) : base(blackboard) 
    {
        info = blackboard.Get<FSMBase>("info");
    }

    public override void ClearPattern()
    {
        patternHealthPoint.Clear();
    }

    public override void AddPattern(Tuple<float, Action> pattern)
    {
        patternHealthPoint.Enqueue(pattern);
    }

    public override SequenceNode SequencePattern()
    { 
        return new SequenceNode
            ( 
                new ConditionNode(() => CheckHealthPoint()),
                new ActionNode(() => PerformHealthPointPattern())
            );
    } 
    private bool CheckHealthPoint()
    { 
        if (patternHealthPoint.Count == 0) return false;
        // 맨앞 요소 확인
        var frontElement = patternHealthPoint.Peek();
         
        return info.HP / info.maxHP <= frontElement.Item1;
    }

    private void PerformHealthPointPattern()
    { 
        //타겟확인 및 패턴진행중인지 확인
        if (blackboard.Get<BasicInfo>("target") == null) return;
        if (blackboard.Get<bool>("isPattern")) return;

        patternHealthPoint.Dequeue().Item2?.Invoke(); 
    }
}

public class ConditionPattern : EnemyPattern<Tuple<Func<bool>, Action>>
{
    private List<Tuple<Func<bool>, Action>> patternCondition = new List<Tuple<Func<bool>, Action>>();
    Action availablePattern;
    public ConditionPattern(Blackboard blackboard) : base(blackboard) { } 
    public override void ClearPattern()
    {
        patternCondition.Clear();
    }

    public override void AddPattern(Tuple<Func<bool>, Action> pattern)
    {
        patternCondition.Add(pattern);
    }

    public override SequenceNode SequencePattern()
    { 
        return new SequenceNode
                (
                    new ConditionNode(() => CheckCondition()),
                    new ActionNode(() => PerformConditionTriggeredPattern())
                );
    }

    private bool CheckCondition()
    { 
        foreach (var temp in patternCondition)
        {
            if (temp.Item1())
            {
                availablePattern = temp.Item2;  
                return true;
            }
        } 
        availablePattern = null;
        return false;
    }
    private void PerformConditionTriggeredPattern()
    { 
        //타겟확인 및 공격중인지 확인
        if (blackboard.Get<BasicInfo>("target") == null) return;
        if (blackboard.Get<bool>("isPattern")) return; 

        availablePattern?.Invoke();
    }
}

public class CooldownPattern : EnemyPattern<Tuple<Action, PatternCooldown>>
{
    private Dictionary<Action, PatternCooldown> patternCooldowns = new Dictionary<Action, PatternCooldown>();
    Action availablePattern;
     
    public CooldownPattern(Blackboard blackboard) : base(blackboard)  {  }

    public override void ClearPattern()
    {
        patternCooldowns.Clear();
    }

    public override void AddPattern(Tuple<Action, PatternCooldown> pattern)
    {
        patternCooldowns.Add(pattern.Item1,pattern.Item2);
    }

    public override SequenceNode SequencePattern()
    {

        return new SequenceNode
            (
                new ConditionNode(() => CheckCoolTime()), 
                new ActionNode(() => PerformCoolTimePattern())
            );
    }
    private bool CheckCoolTime()
    {
        foreach (var pattern in patternCooldowns.Keys)
        {
            if (patternCooldowns.ContainsKey(pattern))
            {
                float elapsedTime = Time.time - patternCooldowns[pattern].lastActivationTime;
                if (elapsedTime >= patternCooldowns[pattern].cooldownDuration)
                {
                    availablePattern = pattern;
                    return true;
                }
            }
        }
        availablePattern = null;
        return false;
    }
    private void PerformCoolTimePattern()
    {
        //타겟확인 및 공격중인지 확인
        if (blackboard.Get<BasicInfo>("target") == null) return;
        if (blackboard.Get<bool>("isPattern")) return;

        availablePattern?.Invoke();
    }
}

public class BasicAttackPattern : EnemyPattern<Action>
{
    private List<Action> basicAttacks = new List<Action>();
    private int currentAttackIndex = 0; 

    public BasicAttackPattern(Blackboard blackboard) : base(blackboard) { }
    public override void ClearPattern()
    {
        basicAttacks.Clear();
    }

    public override void AddPattern(Action pattern)
    {
        basicAttacks.Add(pattern);
    }

    public override SequenceNode SequencePattern()
    { 
        return new SequenceNode
            (
                new ConditionNode(() => CanBasicAttack()), 
                new ActionNode(() => PerformBasicAttackPattern())
            );
    }
    private bool CanBasicAttack()
    { 
        // 공격 패턴이  있는지 확인
        if (basicAttacks.Count == 0 || currentAttackIndex >= basicAttacks.Count)
        {
            if (basicAttacks.Count == 1) return true;
            else return false;  
        }

        return true;
    }
    private void PerformBasicAttackPattern()
    { 
        //타겟확인 및 공격중인지 확인
        if (blackboard.Get<BasicInfo>("target") == null) return;
        if (blackboard.Get<bool>("isPattern")) return;
         
        basicAttacks[currentAttackIndex]?.Invoke();
         
        currentAttackIndex++;
        if (currentAttackIndex >= basicAttacks.Count)
        {
            currentAttackIndex = 0;
        }
    }
}
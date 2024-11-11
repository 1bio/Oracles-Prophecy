using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class MonsterStateMachine : StateMachine
{
    protected Monster p_monster;

    [SerializeField] protected MonsterBehavior p_currentBehavior;

    private void Awake()
    {
        p_monster = GetComponent<Monster>();
    }

    private new void Update()
    {
        base.Update();
    }

    protected void ChangeBehavior(MonsterBehavior monsterBehavior)
    {
        p_currentBehavior = monsterBehavior;

        //Debug.Log($"Current State: {p_currentBehavior}");
        var currentState = new MonsterBehaviorState(p_monster, p_currentBehavior);
        ChangeState(currentState);
    }

    public void OnSpawn()
    {
        p_monster.MonsterStateType = MonsterStateType.Spawn;
        ChangeBehavior(new MonsterBehaviorSpawn());
    }

    public void OnAttack()
    {
        p_monster.MonsterStateType = MonsterStateType.Attack;
        ChangeBehavior(new MonsterBehaviorAttack());
    }

    public void OnIdle()
    {
        p_monster.MonsterStateType = MonsterStateType.Idle;
        ChangeBehavior(new MonsterBehaviorIdle());
    }

    public void OnMove()
    {
        p_monster.MonsterStateType = MonsterStateType.Walk;
        ChangeBehavior(new MonsterBehaviorMovement());
    }

    public void OnDead()
    {
        p_monster.MonsterStateType = MonsterStateType.Death;
        ChangeBehavior(new MonsterBehaviorDead());
    }

    public void OnGotHit() 
    {
        p_monster.MonsterStateType = MonsterStateType.GotHit;
        ChangeBehavior(new MonsterBehaviorGotHit());
    }

    public void OnSkill()
    {
        p_monster.MonsterStateType = MonsterStateType.Skill;
        ChangeBehavior(new MonsterBehaviorSkill());
    }
}
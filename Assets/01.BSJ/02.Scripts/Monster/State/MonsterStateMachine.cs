using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class MonsterStateMachine : StateMachine
{
    protected Monster p_monster;

    [SerializeField] protected MonsterBehaviour p_currentBehaviour;

    private void Awake()
    {
        p_monster = GetComponent<Monster>();
    }

    private new void Update()
    {
        base.Update();
    }

    protected void ChangeBehaviour(MonsterBehaviour monsterBehaviour)
    {
        p_currentBehaviour = monsterBehaviour;

        //Debug.Log($"Current State: {p_currentBehaviour}");
        var currentState = new MonsterBehaviourState(p_monster, p_currentBehaviour);
        ChangeState(currentState);
    }

    public void OnSpawn()
    {
        p_monster.MonsterStateType = MonsterStateType.Spawn;
        ChangeBehaviour(new MonsterBehaviourSpawn());
    }

    public void OnAttack()
    {
        p_monster.MonsterStateType = MonsterStateType.Attack;
        ChangeBehaviour(new MonsterBehaviourAttack());
    }

    public void OnIdle()
    {
        p_monster.MonsterStateType = MonsterStateType.Idle;
        ChangeBehaviour(new MonsterBehaviourIdle());
    }

    public void OnMove()
    {
        p_monster.MonsterStateType = MonsterStateType.Walk;
        ChangeBehaviour(new MonsterBehaviourMovement());
    }

    public void OnDead()
    {
        p_monster.MonsterStateType = MonsterStateType.Death;
        ChangeBehaviour(new MonsterBehaviourDead());
    }

    public void OnGotHit() 
    {
        p_monster.MonsterStateType = MonsterStateType.GotHit;
        ChangeBehaviour(new MonsterBehaviourGotHit());
    }

    public void OnSkill()
    {
        p_monster.MonsterStateType = MonsterStateType.Skill;
        ChangeBehaviour(new MonsterBehaviourSkill());
    }
}
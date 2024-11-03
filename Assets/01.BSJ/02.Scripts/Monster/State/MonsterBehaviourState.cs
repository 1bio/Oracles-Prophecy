using System;

public class MonsterBehaviourState : State
{
    private Monster _monster;
    private MonsterBehaviour _behaviour;

    public MonsterBehaviourState(Monster monster, MonsterBehaviour behaviour)
    {
        this._monster = monster;
        this._behaviour = behaviour;
    }

    public override void Enter()
    {
        _behaviour.OnBehaviourStart(_monster);
    }

    public override void Tick(float deltaTime)
    {
        _behaviour.OnBehaviourUpdate(_monster);
    }

    public override void Exit()
    {
        _behaviour.OnBehaviourEnd(_monster);
    }
}
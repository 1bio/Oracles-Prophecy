using System;

public class MonsterBehaviorState : State
{
    private Monster _monster;
    private MonsterBehavior _Behavior;

    public MonsterBehaviorState(Monster monster, MonsterBehavior Behavior)
    {
        this._monster = monster;
        this._Behavior = Behavior;
    }

    public override void Enter()
    {
        _Behavior.OnBehaviorStart(_monster);
    }

    public override void Tick(float deltaTime)
    {
        _Behavior.OnBehaviorUpdate(_monster);
    }

    public override void Exit()
    {
        _Behavior.OnBehaviorEnd(_monster);
    }
}
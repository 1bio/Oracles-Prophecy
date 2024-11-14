
using UnityEngine;

public class PlayerDeadState : PlayerBaseState
{
    public readonly int DieAnimationHash = Animator.StringToHash("Die");

    public readonly float CrossFadeDuration = 0.1f;


    public PlayerDeadState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(DieAnimationHash, CrossFadeDuration);
    }
    
    public override void Tick(float deltaTime)
    {
    }

    public override void Exit()
    {
    }

}

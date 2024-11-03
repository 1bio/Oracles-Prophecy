using UnityEngine;

public class PlayerRangeAimState : PlayerRangeFreeLookState
{
    public readonly int AimAnimationHash = Animator.StringToHash("Aim@Range"); // 조준 애니메이션 해쉬


    public PlayerRangeAimState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    #region abstract Methods
    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(AimAnimationHash, CrossFadeDuration);

        Aiming();
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);

        AnimatorStateInfo currentInfo = stateMachine.Animator.GetCurrentAnimatorStateInfo(0);

        if (/*!stateMachine.InputReader.IsAiming && */ currentInfo.IsName("Aim@Range") && currentInfo.normalizedTime >= ExitTime)
        {
            stateMachine.ChangeState(new PlayerRangeFreeLookState(stateMachine));
        }
    }

    public override void Exit()
    {
    }
    #endregion
}


using UnityEngine;

public class PlayerRangeSkyFallState : PlayerRangeFreeLookState
{
    public readonly int SkyFallAnimationHash = Animator.StringToHash("SkyFall@Range"); // 연발 애니메이션 해쉬

    public PlayerRangeSkyFallState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    #region abstract Methods
    public override void Enter()
    {
        stateMachine.Animator.SetFloat(RangeVelocity, 0);

        stateMachine.Animator.CrossFadeInFixedTime(SkyFallAnimationHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);

        AnimatorStateInfo currentInfo = stateMachine.Animator.GetCurrentAnimatorStateInfo(0);

        if (currentInfo.IsName("SkyFall@Range") && currentInfo.normalizedTime >= 0.8f)
        {
            stateMachine.ChangeState(new PlayerRangeFreeLookState(stateMachine));
            return;
        }
    }

    public override void Exit()
    {
    }
    #endregion
}

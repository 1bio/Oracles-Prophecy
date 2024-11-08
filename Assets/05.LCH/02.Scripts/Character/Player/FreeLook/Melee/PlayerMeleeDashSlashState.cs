using UnityEngine;

public class PlayerMeleeDashSlashState : PlayerFreeLookState
{
    public readonly int SlashAnimationHash = Animator.StringToHash("Slash@Melee"); // 스핀 공격 애니메이션 해쉬

    public PlayerMeleeDashSlashState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }


    #region abstarct Methods
    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(SlashAnimationHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);

        AnimatorStateInfo currentInfo = stateMachine.Animator.GetCurrentAnimatorStateInfo(0);

        float normalizedTime = currentInfo.normalizedTime;

        if (currentInfo.IsName("Slash@Melee") && currentInfo.normalizedTime >= ExitTime)
        {
            stateMachine.ChangeState(new PlayerFreeLookState(stateMachine));
        }
    }

    public override void Exit()
    {
    }
    #endregion
}

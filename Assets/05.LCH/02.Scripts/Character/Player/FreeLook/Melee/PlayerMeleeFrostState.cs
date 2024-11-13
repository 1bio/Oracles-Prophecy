using UnityEngine;

public class PlayerMeleeFrostState : PlayerFreeLookState
{
    public readonly int FrostAnimationHash = Animator.StringToHash("Frost@Melee"); // 스핀 공격 애니메이션 해쉬

    public PlayerMeleeFrostState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    #region abstarct Methods
    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(FrostAnimationHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);

        AnimatorStateInfo currentInfo = stateMachine.Animator.GetCurrentAnimatorStateInfo(0);

        float normalizedTime = currentInfo.normalizedTime;

        if (currentInfo.IsName("Frost@Melee") && normalizedTime >= ExitTime)
        {
            stateMachine.ChangeState(new PlayerFreeLookState(stateMachine));
        }

    }

    public override void Exit()
    {
    }
    #endregion
}

using UnityEngine;

public class PlayerRangeRapidShotState : PlayerRangeFreeLookState
{
    public readonly int RapidShotAnimationHash = Animator.StringToHash("RapidShot@Range"); // 연발 애니메이션 해쉬

    public PlayerRangeRapidShotState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    #region abstarct Methods
    public override void Enter()
    {
        stateMachine.InputReader.RollEvent += OnRolling;

        stateMachine.Animator.CrossFadeInFixedTime(RapidShotAnimationHash, CrossFadeDuration);

        Aiming();
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);

        AnimatorStateInfo currentInfo = stateMachine.Animator.GetCurrentAnimatorStateInfo(0);

        float normalizedTime = currentInfo.normalizedTime;

        if (currentInfo.IsName("RapidShot@Range") && !stateMachine.InputReader.IsAttacking && normalizedTime >= ExitTime)
        {
            stateMachine.ChangeState(new PlayerRangeFreeLookState(stateMachine));
        }
    }

    public override void Exit()
    {
        stateMachine.InputReader.RollEvent -= OnRolling;
    }
    #endregion


    #region Event Methods
    private void OnRolling()
    {
        stateMachine.ChangeState(new PlayerRollingState(stateMachine));
    }
    #endregion
}

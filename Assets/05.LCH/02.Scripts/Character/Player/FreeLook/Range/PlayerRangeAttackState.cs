using UnityEngine;

public class PlayerRangeAttackState : PlayerRangeFreeLookState
{
    private AttackData attack;

    public readonly int AttackAnimationHash = Animator.StringToHash("Attack@Range"); // 원거리 공격 애니메이션 해쉬

    public PlayerRangeAttackState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    #region abstract Methods
    public override void Enter()
    {
        stateMachine.InputReader.RollEvent += OnRolling;

        stateMachine.Animator.CrossFadeInFixedTime(AttackAnimationHash, CrossFadeDuration);

        Aiming();
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);

        AnimatorStateInfo currentInfo = stateMachine.Animator.GetCurrentAnimatorStateInfo(0);

        float normalizedTime = currentInfo.normalizedTime;

        if (currentInfo.IsName("Attack@Range") && !stateMachine.InputReader.IsAttacking && normalizedTime >= ExitTime)
        {
            stateMachine.ChangeState(new PlayerRangeFreeLookState(stateMachine));
            return;
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

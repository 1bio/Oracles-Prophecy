using System;
using UnityEngine;

public class PlayerHeavyAttackState : PlayerFreeLookState
{
    private AttackData attack;

    private float previousFrameTime;

    private bool alreadyApplyForce;


    public PlayerHeavyAttackState(PlayerStateMachine stateMachine, int comboIndex) : base(stateMachine)
    {
        attack = DataManager.instance.playerData.attackData[comboIndex];
    }


    #region abstract Methods
    public override void Enter()
    {
        stateMachine.InputReader.RollEvent += OnRolling;

        stateMachine.Animator.CrossFadeInFixedTime(attack.AnimationName, attack.TransitionDuration);

        Aiming();

        float min = DataManager.instance.playerData.skillData[4].minDamage;
        float max = DataManager.instance.playerData.skillData[4].maxDamage;

        stateMachine.MeleeComponenet.SetAttack(min, max, attack.KnockBack);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);

        float normalizedTime = GetNormalizedTime(stateMachine.Animator);

        if (normalizedTime >= previousFrameTime && normalizedTime < 1f) // previousFrameTime <= normalizedTime < 1f
        {
            if (normalizedTime >= attack.ForceTime)
            {
                TryApplyForce();
            }

            if (stateMachine.InputReader.IsAttacking)
            {
                TryComboAttack(normalizedTime);
            }
        }
        else
        {
            // FreeLook
            if (!stateMachine.InputReader.IsAttacking)
            {
                stateMachine.ChangeState(new PlayerFreeLookState(stateMachine));
            }
        }

        previousFrameTime = normalizedTime;
    }

    public override void Exit()
    {
        stateMachine.InputReader.RollEvent -= OnRolling;

        stateMachine.WeaponToggle.DisableWeapon();

        stateMachine.WeaponTrail.DestroyTrail();
        stateMachine.ParticleEventHandler.StopParticleSystem();
    }
    #endregion


    #region Main Methods
    private float GetNormalizedTime(Animator animator) // 애니메이션 normalizedTime 값 리턴
    {
        AnimatorStateInfo currentInfo = animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextInfo = animator.GetNextAnimatorStateInfo(0);

        if (animator.IsInTransition(0) && nextInfo.IsTag("Attack"))
        {
            // 전환 중 일때 실행되는 부분
            return nextInfo.normalizedTime;
        }
        else if (!animator.IsInTransition(0) && currentInfo.IsTag("Attack"))
        {
            // 애니메이션 재생 중 실행되는 부분
            return currentInfo.normalizedTime;
        }
        else
        {
            // 애니메이션 재생 종료 후 실행되는 부분
            return 0f;
        }
    }

    // 콤보 공격 인덱스 확인 및 생성
    private void TryComboAttack(float normalizedTime)
    {
        if (attack.ComboAttackIndex == -1)
            return;

        if (normalizedTime < attack.ComboAttackTime)
            return;

        if (!stateMachine.InputReader.IsAttacking)
            return;

        stateMachine.ChangeState(new PlayerHeavyAttackState(stateMachine, attack.ComboAttackIndex));
    }

    // 공격 시 추가 힘
    private void TryApplyForce()
    {
        if (alreadyApplyForce)
            return;

        stateMachine.ForceReceiver.AddForce(stateMachine.transform.forward * attack.Force);

        alreadyApplyForce = true;
    }
    #endregion


    #region Event Methods
    private void OnRolling()
    {
        stateMachine.ChangeState(new PlayerRollingState(stateMachine));
    }
    #endregion
}

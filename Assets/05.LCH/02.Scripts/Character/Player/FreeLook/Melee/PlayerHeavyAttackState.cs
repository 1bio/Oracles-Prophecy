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
    private float GetNormalizedTime(Animator animator) // �ִϸ��̼� normalizedTime �� ����
    {
        AnimatorStateInfo currentInfo = animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextInfo = animator.GetNextAnimatorStateInfo(0);

        if (animator.IsInTransition(0) && nextInfo.IsTag("Attack"))
        {
            // ��ȯ �� �϶� ����Ǵ� �κ�
            return nextInfo.normalizedTime;
        }
        else if (!animator.IsInTransition(0) && currentInfo.IsTag("Attack"))
        {
            // �ִϸ��̼� ��� �� ����Ǵ� �κ�
            return currentInfo.normalizedTime;
        }
        else
        {
            // �ִϸ��̼� ��� ���� �� ����Ǵ� �κ�
            return 0f;
        }
    }

    // �޺� ���� �ε��� Ȯ�� �� ����
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

    // ���� �� �߰� ��
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

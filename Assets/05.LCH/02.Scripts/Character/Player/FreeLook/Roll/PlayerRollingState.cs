using MedievalKingdomUI.Scripts.Window;
using UnityEngine;

public class PlayerRollingState : PlayerBaseState
{
    public readonly int RollAnimationHash = Animator.StringToHash("Roll");

    public readonly float CrossFadeDuration = 0.1f;

    private Vector2 DodgeDirectionInput;

    private float remainingDodgeTime;

    private float currentTime;

    private float lastTime;


    public PlayerRollingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }


    #region abstract Methods 
    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(RollAnimationHash, CrossFadeDuration);

        stateMachine.Health.SetInvulnerable(true);

        //근거리 무기 이펙트 제거
        stateMachine.WeaponToggle.DisableWeapon();

 /*       stateMachine.WeaponTrail.DestroyTrail();
        stateMachine.ParticleEventHandler.StopParticleSystem();*/

        if (Time.time - stateMachine.PreviousDodgeTime < stateMachine.DodgeCooldown)
            return;

        stateMachine.SetDodgeTime(Time.time);

        DodgeDirectionInput = stateMachine.InputReader.MoveValue;

        remainingDodgeTime = stateMachine.DodgeDuration;
    }

    public override void Tick(float deltaTime)
    {
        stateMachine.ForceReceiver.RollingForce(deltaTime);

        AnimatorStateInfo currentInfo = stateMachine.Animator.GetCurrentAnimatorStateInfo(0);

        switch (AnimatedWindowController.choiceIndex)
        {
            case 0: // 전사
                if (currentInfo.IsName("Roll") && currentInfo.normalizedTime > 0.8f)
                {
                    stateMachine.ChangeState(new PlayerFreeLookState(stateMachine));
                    return;
                }
                break;

            case 1: // 궁수
                if (currentInfo.IsName("Roll") && currentInfo.normalizedTime > 0.8f)
                {
                    stateMachine.ChangeState(new PlayerRangeFreeLookState(stateMachine));
                    return;
                }
                break;
        }

        /*// FreeLook
        if (currentInfo.IsName("Roll") && currentInfo.normalizedTime > 0.8f && stateMachine.WeaponPrefabs[0].activeSelf)
        {
            stateMachine.ChangeState(new PlayerFreeLookState(stateMachine));
            return;
        }
        else if(currentInfo.IsName("Roll") && currentInfo.normalizedTime > 0.8f && stateMachine.WeaponPrefabs[1].activeSelf)
        {
            stateMachine.ChangeState(new PlayerRangeFreeLookState(stateMachine));
            return;
        }*/
    }

    public override void Exit()
    {
        stateMachine.Health.SetInvulnerable(false);
    }
    #endregion
}

using MedievalKingdomUI.Scripts.Window;
using UnityEngine;

public class PlayerImpactState : PlayerBaseState
{
    public readonly int ImpactAnimationHash = Animator.StringToHash("Impact"); // 애니메이션 해쉬

    public readonly float CrossFadeDuration = 0.1f;

    private float duration = 0.8f;

    public PlayerImpactState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }


    #region abstarct Methods
    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(ImpactAnimationHash, CrossFadeDuration);

        // 근거리 무기 이펙트 제거
        stateMachine.WeaponToggle?.DisableWeapon();

        stateMachine.WeaponTrail?.DestroyTrail();
        stateMachine.ParticleEventHandler?.StopParticleSystem();
    }


    public override void Tick(float deltaTime)
    {
        Move(deltaTime);

        duration -= deltaTime;

        switch (AnimatedWindowController.choiceIndex)
        {
            case 0: // 전사
                if (duration <= 0f)
                {
                    stateMachine.ChangeState(new PlayerFreeLookState(stateMachine)); 
                    return;
                }
                break;

            case 1:  // 궁수
                if (duration <= 0f)
                {
                    stateMachine.ChangeState(new PlayerRangeFreeLookState(stateMachine));
                    return;
                }
                break;
        }

       /* // FreeLook
        if (duration <= 0f && ClassSelectWindow.classIndex == 0) // 전사
        {
            stateMachine.ChangeState(new PlayerFreeLookState(stateMachine));
            return;
        }
        else if (duration <= 0f && stateMachine.WeaponPrefabs[1].activeSelf) // 궁수
        {
            stateMachine.ChangeState(new PlayerRangeFreeLookState(stateMachine));
            return;
        }*/
    }

    public override void Exit()
    {
    }
    #endregion
}

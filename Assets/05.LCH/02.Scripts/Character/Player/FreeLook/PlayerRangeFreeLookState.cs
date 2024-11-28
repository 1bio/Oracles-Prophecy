using MedievalKingdomUI.Scripts.Window;
using System;
using UnityEngine;

public class PlayerRangeFreeLookState : PlayerBaseState
{
    public readonly int FreeLookWithRange = Animator.StringToHash("FreeLookWithRange"); // 원거리 블렌드 트리

    public readonly int RangeVelocity = Animator.StringToHash("RangeVelocity"); // 애니메이션 파라미터

    public readonly float CrossFadeDuration = 0.1f;

    public readonly float DampTime = 0.1f;

    public readonly float ExitTime = 0.8f;

    public PlayerRangeFreeLookState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    #region abstarct Methods
    public override void Enter()
    {
        stateMachine.InputReader.RollEvent += OnRolling;

        stateMachine.InputReader.firstSkillEvent += OnFirstSkill; // 정조준
        stateMachine.InputReader.SecondSkillEvent += OnSecondSkill; // 트리플샷
        stateMachine.InputReader.thirdSkillEvent += OnThirdSkill; // 화살비

        stateMachine.Animator.CrossFadeInFixedTime(FreeLookWithRange, CrossFadeDuration, 0, 0.01f, 0f);
    }

    public override void Tick(float deltaTime)
    {
        Vector3 movement = CalculatorMovement();

        Move(movement, deltaTime); // 이동

        Rotate(movement, deltaTime); // 회전

        // Attack
        if (stateMachine.InputReader.IsAttacking && HasWeapon() && UIManager.instance.IsActiveUI())
        {
            if (SkillManager.instance.IsPassiveActive("TripleShot")) // 트리플샷 [1]
            {
                stateMachine.ChangeState(new PlayerRangeRapidShotState(stateMachine));
                return;
            }
            else
            {
                stateMachine.ChangeState(new PlayerRangeAttackState(stateMachine));
                return;
            }
        }

        // Idle & Moving
        if (stateMachine.InputReader.MoveValue == Vector2.zero)
        {
            stateMachine.Animator.SetFloat(RangeVelocity, 0f, DampTime, deltaTime);
            return;
        }
        
        stateMachine.Animator.SetFloat(RangeVelocity, 1f, DampTime, deltaTime);
    }

    public override void Exit()
    {
        stateMachine.InputReader.RollEvent -= OnRolling;

        stateMachine.InputReader.firstSkillEvent -= OnFirstSkill;
        stateMachine.InputReader.SecondSkillEvent -= OnSecondSkill;
        stateMachine.InputReader.thirdSkillEvent -= OnThirdSkill;
    }
    #endregion

    #region
    public bool HasWeapon()
    {
        switch (AnimatedWindowController.choiceIndex)
        {
            case 0:
                Transform sword = stateMachine.Player.swordTransform;

                foreach (Transform child in sword)
                {
                    if (child.CompareTag("Weapon"))
                    {
                        return true;
                    }
                }
                break;

            case 1:
                Transform bow = stateMachine.Player.bowTransform;

                foreach (Transform child in bow)
                {
                    if (child.CompareTag("Weapon"))
                    {
                        return true;
                    }
                }
                break;
        }

        return false;
    }
    #endregion

    #region Event Methods
    private void OnRolling()
    {
        stateMachine.ChangeState(new PlayerRollingState(stateMachine));
    }

    private void OnFirstSkill() // 정조준 [0]
    {
        if (SkillManager.instance.GetRemainingCooldown("ChargingShot") <= 0f && !DataManager.instance.playerData.skillData[0].isUnlock
            && DataManager.instance.playerData.statusData.currentMana >= DataManager.instance.playerData.skillData[0].useMana && HasWeapon())
        {
            stateMachine.ChangeState(new PlayerRangeAimState(stateMachine));
        }
    }

    private void OnSecondSkill() // 트리플샷 [1]
    {
        if (SkillManager.instance.GetRemainingCooldown("TripleShot") <= 0f && !DataManager.instance.playerData.skillData[1].isUnlock 
            && DataManager.instance.playerData.statusData.currentMana >= DataManager.instance.playerData.skillData[1].useMana && HasWeapon())
        {
            SkillManager.instance.SetActiveSkill(true);
        }
    }

    private void OnThirdSkill() // 화살비 [2]
    {
        if (SkillManager.instance.GetRemainingCooldown("SkyFallShot") <= 0f && !DataManager.instance.playerData.skillData[2].isUnlock 
            && DataManager.instance.playerData.statusData.currentMana >= DataManager.instance.playerData.skillData[2].useMana
            && stateMachine.Targeting.CurrentTarget != null && HasWeapon())
        {
            stateMachine.ChangeState(new PlayerRangeSkyFallState(stateMachine));
        }
    }
    #endregion


}

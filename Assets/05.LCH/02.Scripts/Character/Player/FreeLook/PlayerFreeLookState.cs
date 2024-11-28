using MedievalKingdomUI.Scripts.Window;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFreeLookState : PlayerBaseState
{
    public readonly int FreeLookWithMelee = Animator.StringToHash("FreeLookWithMelee"); // 근거리 블렌드 트리

    public readonly int Velocity = Animator.StringToHash("Velocity"); // 애니메이션 파라미터

    public readonly float CrossFadeDuration = 0.1f;

    public readonly float DampTime = 0.1f;

    public readonly float ExitTime = 0.8f;

    private int basicAttackDataIndex = 0; 

    private int heavyAttackDataIndex = 3;


    public PlayerFreeLookState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    
    }

    #region abstarct Methods
    public override void Enter()
    {
        stateMachine.InputReader.RollEvent += OnRolling;

        stateMachine.InputReader.firstSkillEvent += OnFirstSkill; // 절단
        stateMachine.InputReader.SecondSkillEvent += OnSecondSkill; // 화염칼
        stateMachine.InputReader.thirdSkillEvent += OnThirdSkill; // 빙결

        stateMachine.Animator.CrossFadeInFixedTime(FreeLookWithMelee, CrossFadeDuration);

        stateMachine.WeaponToggle.DisableWeapon();

        stateMachine.WeaponTrail.DestroyTrail();
        stateMachine.ParticleEventHandler.StopParticleSystem();
    }

    public override void Tick(float deltaTime)
    {
        Vector3 movement = CalculatorMovement();

        Move(movement, deltaTime); // 이동

        Rotate(movement, deltaTime); // 회전

        //Debug.Log(SkillManager.instance.GetRemainingCooldown("도약베기"));
        //Debug.Log(SkillManager.instance.GetRemainingCooldown("화염칼"));
        //Debug.Log(SkillManager.instance.GetRemainingCooldown("회전베기"));

        // Attacking
        if (stateMachine.InputReader.IsAttacking && HasWeapon() && UIManager.instance.IsActiveUI())
        {
            if (SkillManager.instance.IsPassiveActive("Fire Blade")) 
            {
                stateMachine.ChangeState(new PlayerHeavyAttackState(stateMachine, heavyAttackDataIndex));
                return;
            }
            else
            {
                DataManager.instance.SaveData();
                DataManager.instance.LoadData();

                stateMachine.ChangeState(new PlayerMeleeAttackState(stateMachine, basicAttackDataIndex));
                return;
            }
        }

        // Idle & Moving
        if (stateMachine.InputReader.MoveValue != Vector2.zero)
        {
            stateMachine.Animator.SetFloat(Velocity, 1f, DampTime, deltaTime);
            return;
        }

        stateMachine.Animator.SetFloat(Velocity, 0f, DampTime, deltaTime);

    }

    public override void Exit()
    {
        stateMachine.InputReader.RollEvent -= OnRolling;

        stateMachine.InputReader.firstSkillEvent -= OnFirstSkill;
        stateMachine.InputReader.SecondSkillEvent -= OnSecondSkill;
        stateMachine.InputReader.thirdSkillEvent -= OnThirdSkill;
    }
    #endregion


    #region Main Methods
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

    private void OnFirstSkill() // 절단 [3]
    {
        if (SkillManager.instance.GetRemainingCooldown("Single Slash") <= 0f && !DataManager.instance.playerData.skillData[3].isUnlock 
            && DataManager.instance.playerData.statusData.currentMana >= DataManager.instance.playerData.skillData[3].useMana && HasWeapon())
        {
            stateMachine.ChangeState(new PlayerMeleeDashSlashState(stateMachine));
        }
    }

    private void OnSecondSkill() // 화염칼 [4]
    {
        if (SkillManager.instance.GetRemainingCooldown("Fire Blade") <= 0f && !DataManager.instance.playerData.skillData[4].isUnlock
            && DataManager.instance.playerData.statusData.currentMana >= DataManager.instance.playerData.skillData[4].useMana && HasWeapon())
        {
            SkillManager.instance.SetActiveSkill(true);
        }
    }

    private void OnThirdSkill() // 빙결 [5]
    {
        if(SkillManager.instance.GetRemainingCooldown("Frost") <= 0f && !DataManager.instance.playerData.skillData[5].isUnlock 
            && DataManager.instance.playerData.statusData.currentMana >= DataManager.instance.playerData.skillData[5].useMana && HasWeapon())
        {
            stateMachine.ChangeState(new PlayerMeleeFrostState(stateMachine));
        }
    }
    #endregion
}

using SingularityGroup.HotReload;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterStateMachineController : MonsterStateMachine
{
    public float CurrentBasicAttackCooldownTime { get; set; }

    private new void Update()
    {
        base.Update();

        if (p_monster.MovementController.TargetDetector.IsTargetDetected)
        {
            CurrentBasicAttackCooldownTime += Time.deltaTime;
            p_monster.SkillController.UpdateCooldowns();
        }

        // 살아있는지 확인
        if (!IsAlive())
        {
            if (p_monster.MonsterStateType != MonsterStateType.Death)
                OnDead();
        }
        else if (p_monster.MovementController.TargetDetector.IsTargetDetected)
        {
            HandleLivingState();
        }
        else if (!p_monster.AnimationController.IsLockedInAnimation)
        {
            OnIdle();
        }
    }

    private void HandleLivingState()
    {
        // 다른 애니메이션이 실행되고 있는지 확인
        if (!p_monster.AnimationController.IsLockedInAnimation)
        {
            if (p_monster.MonsterStateType != MonsterStateType.Skill
                && p_monster.SkillController.GetAvailableSkills().Count > 0
                && p_monster.SkillController.UpdateCurrentSkillData().IsTargetWithinSkillRange)
            {

                OnSkill();
            }
            else if (p_monster.CombatController.MonsterCombatAbility.MonsterAttack.IsTargetWithinAttackRange &&
                    p_monster.CombatController.MonsterCombatAbility.MonsterAttack.TotalCount > 0)
            {
                if (p_monster.MonsterStateType != MonsterStateType.Idle
                    && p_monster.CombatController.MonsterCombatAbility.MonsterAttack.CooldownThreshold > CurrentBasicAttackCooldownTime)
                    OnIdle();
                else
                {
                    OnAttack();
                }
            }
            else if (p_monster.MonsterStateType != MonsterStateType.Walk)
            {
                OnMove();
            }
        }
    }

    public bool IsAlive()
    {
        if (p_monster.CombatController.MonsterCombatAbility.MonsterHealth.CurrentHealth > 0)
        {
            return true;
        }
        else
        {
            p_monster.CombatController.MonsterCombatAbility.IsDead = true;
            return false;
        }
    }
}

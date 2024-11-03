using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterStateMachineController : MonsterStateMachine
{
    public float CurrentBasicAttackCooldownTime { get; set; }

    private void OnEnable()
    {
        OnSpawn();
    }

    private new void Update()
    {
        base.Update();

        if (p_monster.MovementController.TargetDetector.IsTargetDetected)
        {
            CurrentBasicAttackCooldownTime += Time.deltaTime;
            p_monster.SkillController.UpdateCooldowns();
        }

        // ����ִ��� Ȯ��
        if (!IsAlive())
        {
            if (p_monster.MonsterStateType != MonsterStateType.Death)
                OnDead();

            // ������� �ʾ��� ��
            if (p_monster.gameObject.activeSelf)
            {
                StartCoroutine(DeathCheck());
            }
        }
        else if (p_monster.MovementController.TargetDetector.IsTargetDetected)
        {
            HandleLivingState();
        }
    }

    private void HandleLivingState()
    {
        // �ٸ� �ִϸ��̼��� ����ǰ� �ִ��� Ȯ��
        if (!p_monster.AnimationController.IsLockedInAnimation)
        {
            if (p_monster.MonsterStateType != MonsterStateType.Skill
                && p_monster.SkillController.GetAvailableSkills().Count > 0
                && p_monster.SkillController.UpdateCurrentSkillData().Range >= Vector3.Distance(p_monster.MovementController.Astar.TargetTransform.position, this.transform.position))
            {

                OnSkill();
            }
            else if (p_monster.CombatController.MonsterCombatAbility.MonsterAttack.IsTargetWithinAttackRange &&
                    Vector3.Distance(p_monster.MovementController.Astar.TargetTransform.position, this.transform.position)
                    <= p_monster.CombatController.MonsterCombatAbility.MonsterAttack.Range)
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

    private IEnumerator DeathCheck()
    {
        yield return new WaitForEndOfFrame();

        if (p_monster.MonsterStateType != MonsterStateType.Death &&
            !p_monster.AnimationController.AnimatorStateInfo.IsName("Death"))
        {
            OnDead();
        }

        yield return new WaitForSeconds(3f);

        p_monster.gameObject.SetActive(false);
    }
}

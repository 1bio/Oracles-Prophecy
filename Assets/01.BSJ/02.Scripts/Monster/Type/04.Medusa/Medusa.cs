using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medusa : Monster
{
    public float SkillMoveSpeed { get; private set; } = 0f;
    public bool IsMovingDuringSkill { get; private set; } = false;

    public enum JumpAttackAnimationName
    {
        JumpAttack
    }

    public enum SpinAttackAnimationName
    {
        SpinAttack
    }

    private void Update()
    {
        if (CombatController.MonsterCombatAbility.MonsterHealth.CurrentHealth > 0)
        {
            SkillAttackMove();
        }
    }

    private void SkillAttackMove()
    {
        if (IsMovingDuringSkill &&
            Vector3.Distance(transform.position, MovementController.Astar.TargetTransform.position) > 1)
            MovementController.CharacterController.SimpleMove(MovementController.Direction * SkillMoveSpeed);
    }

    // Animation Event
    public void StartSkillMove(float speed)
    {
        SkillMoveSpeed = speed;
        IsMovingDuringSkill = true;
    }

    public void StopSkillMove()
    {
        IsMovingDuringSkill = false;
    }

    public void PlayCrackVFX()
    {
        ParticleController.RePlayVFX("Crack", 60, 3);
    }
}
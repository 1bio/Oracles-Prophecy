using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medusa : Monster
{
    private float _skillMoveSpeed = 0f;
    private bool _isMovingDuringSkill = false;
    private bool _isLookAtTargetActive = false;


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
        if (_isMovingDuringSkill &&
            Vector3.Distance(transform.position, MovementController.Astar.TargetTransform.position) > 1)
            MovementController.CharacterController.SimpleMove(MovementController.Direction * _skillMoveSpeed);

        if (_isLookAtTargetActive)
        {
            MovementController.LookAtTarget(CombatController.MonsterCombatAbility.TurnSpeed);
        }
    }

    // Animation Event
    public void StartSkillMove(float speed)
    {
        _skillMoveSpeed = speed;
        _isMovingDuringSkill = true;
    }

    public void StopSkillMove()
    {
        _isMovingDuringSkill = false;
    }

    public void SetLookAtTargetActive()
    {
        _isLookAtTargetActive = _isLookAtTargetActive ? false : true;
    }

    public void PlayCrackVFX()
    {
        ParticleController.RePlayVFX("Crack", 60, 3);
    }

    public void PlayWideCrackVFX()
    {
        ParticleController.RePlayVFX("WideEffect", 0, 1);
    }
}
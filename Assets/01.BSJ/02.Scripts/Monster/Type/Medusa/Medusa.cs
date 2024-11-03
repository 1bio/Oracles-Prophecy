using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medusa : Monster
{
    private float _jumpSpeed = 10f;
    private bool _isJumping = false;

    private float _spinSpeed = 5f;
    private bool _isSpinning = false;

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
        if (_isJumping &&
            Vector3.Distance(transform.position, MovementController.Astar.TargetTransform.position) > 1)
            MovementController.CharacterController.SimpleMove(MovementController.Direction * _jumpSpeed);
        else if (_isSpinning &&
            Vector3.Distance(transform.position, MovementController.Astar.TargetTransform.position) > 1)
            MovementController.CharacterController.SimpleMove(MovementController.Direction * _spinSpeed);
    }

    // Animation Event
    public void StartJumpAttackMove()
    {
        _isJumping = true;
    }

    public void StopJumpAttackMove()
    {
        _isJumping = false;
    }

    public void StartSpinAttackMove()
    {
        _isSpinning = true;
    }

    public void StopSpinAttackMove()
    {
        _isSpinning = false;
    }

    public void PlayCrackVFX()
    {
        ParticleController.RePlayVFX("Crack", 120, 6);
    }
}
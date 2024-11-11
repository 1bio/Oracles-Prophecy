using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Monster
{
    private float _jumpSpeed = 5f;
    private bool _isJumping = false;

    public enum JumpAttackAnimation
    {
        JumpAttack
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
}

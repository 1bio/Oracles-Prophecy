using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "MedusaJumpAttack", menuName = "Data/MonsterSKillData/Medusa/JumpAttack")]
public class MedusaJumpAttack : MonsterSkillData
{
    private Transform _vfxTransform;

    private int _currentAttackCount;
    private int _maxAttackLimit;

    private HashSet<Collider> damagedPlayers = new HashSet<Collider>();
    private bool _hasAttacked = false;

    public override void ActiveSkillEnter(Monster monster)
    {
        _vfxTransform = monster.ParticleController.GetAvailableParticle("Crack").transform;
        _vfxTransform.position = monster.transform.position + monster.transform.forward;
        _vfxTransform.rotation = Quaternion.LookRotation(monster.transform.forward);

        _currentAttackCount = 0;
        _maxAttackLimit = 3;

        damagedPlayers = new HashSet<Collider>();
        _hasAttacked = false;
    }

    public override void ActiveSkillTick(Monster monster)
    {
        monster.AnimationController.AnimatorStateInfo = monster.AnimationController.Animator.GetCurrentAnimatorStateInfo(0);

        if (_currentAttackCount >= _maxAttackLimit)
            monster.AnimationController.IsLockedInAnimation = false;

        if (monster.AnimationController.AnimatorStateInfo.IsTag("JumpAttack"))
        {
            if (monster.AnimationController.AnimatorStateInfo.normalizedTime >= 0.9)
            {
                if (!_hasAttacked)
                    _currentAttackCount++;
                _hasAttacked = true;
            }
        }
        else
        {
            damagedPlayers = new HashSet<Collider>();
            _hasAttacked = false;

            monster.AnimationController.PlayIdleAnimation();
            monster.MovementController.LookAtTarget(monster.CombatController.MonsterCombatAbility.TurnSpeed);
            
            if (Vector3.Angle(monster.transform.forward, monster.MovementController.Direction) <= 3 && !_hasAttacked)
                monster.AnimationController.PlaySkillAnimation(Medusa.JumpAttackAnimationName.JumpAttack.ToString());
        }
    }

    public override void ActiveSkillExit(Monster monster)
    {

    }
}

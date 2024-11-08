using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MedusaSpinAttack", menuName = "Data/MonsterSKillData/Medusa/SpinAttack")]
public class MedusaSpinAttack : MonsterSkillData
{
    private Transform _vfxTransform;

    private bool _hasAttacked = false;

    public override void ActiveSkillEnter(Monster monster)
    {
        _vfxTransform = monster.ParticleController.GetAvailableParticle("SpinEffect").transform;
        _vfxTransform.SetParent(monster.transform);
        _vfxTransform.localPosition = Vector3.up;
        _vfxTransform.localRotation = Quaternion.identity;

        _hasAttacked = false;
    }

    public override void ActiveSkillTick(Monster monster)
    {
        monster.AnimationController.AnimatorStateInfo = monster.AnimationController.Animator.GetCurrentAnimatorStateInfo(0);

        if (!monster.AnimationController.AnimatorStateInfo.IsName(Medusa.SpinAttackAnimationName.SpinAttack.ToString()))
        {
            monster.MovementController.LookAtTarget(monster.CombatController.MonsterCombatAbility.TurnSpeed * 2);
            if (Vector3.Angle(monster.transform.forward, monster.MovementController.Direction) <= 3 && !_hasAttacked)
            {
                _vfxTransform = monster.ParticleController.GetAvailableParticle("SpinEffect").transform;
                _vfxTransform.SetParent(monster.transform);
                _vfxTransform.localPosition = Vector3.up;
                _vfxTransform.localRotation = Quaternion.identity;
                monster.AnimationController.PlaySkillAnimation(Medusa.SpinAttackAnimationName.SpinAttack.ToString());
            }
        }
        else
        {
            if (monster.AnimationController.AnimatorStateInfo.normalizedTime > 0.9f)
                monster.AnimationController.IsLockedInAnimation = false;
        }
    }

    public override void ActiveSkillExit(Monster monster)
    {
    
    }
}

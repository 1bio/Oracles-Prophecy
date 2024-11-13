using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DashAttack", menuName = "Data/MonsterSKillData/Medusa/Serpentine/DashAttack")]
public class MadusaDashAttack : MonsterSkillData
{
    public enum DashAttackAnimationName
    {
        DashAttack
    }

    private Transform _vfxTransform;

    private bool _hasAttacked = false;

    public override void ActiveSkillEnter(Monster monster)
    {
        _vfxTransform = monster.ParticleController.GetAvailableParticle("WideEffect").transform;
        _vfxTransform.position = monster.transform.position + monster.transform.forward;
        _vfxTransform.rotation = Quaternion.LookRotation(monster.transform.forward);

        _hasAttacked = false;
    }   

    public override void ActiveSkillTick(Monster monster)
    {
        monster.AnimationController.AnimatorStateInfo = monster.AnimationController.Animator.GetCurrentAnimatorStateInfo(0);

        if (!monster.AnimationController.AnimatorStateInfo.IsName(DashAttackAnimationName.DashAttack.ToString()) && !_hasAttacked)
        {
            monster.AnimationController.PlayIdleAnimation();
            monster.MovementController.LookAtTarget(monster.CombatController.MonsterCombatAbility.TurnSpeed);

            if (Vector3.Angle(monster.transform.forward, monster.MovementController.Direction) <= 3)
            {
                monster.AnimationController.PlaySkillAnimation(DashAttackAnimationName.DashAttack.ToString());
                _hasAttacked = true;
            }
        }
    }

    public override void ActiveSkillExit(Monster monster)
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FireThrow", menuName = "Data/MonsterSKillData/Minotaur/FireThrow")]
public class MinotaurFireThrow : MonsterSkillData
{
    private bool _hasAttacked = false;

    public override void ActiveSkillEnter(Monster monster)
    {
        _hasAttacked = false;
    }

    public override void ActiveSkillTick(Monster monster)
    {
        monster.AnimationController.AnimatorStateInfo = monster.AnimationController.Animator.GetCurrentAnimatorStateInfo(0);

        monster.MovementController.LookAtTarget(monster.CombatController.MonsterCombatAbility.TurnSpeed);

        if (!monster.AnimationController.AnimatorStateInfo.IsName("FireThrower") && !_hasAttacked)
        {
            monster.AnimationController.PlaySkillAnimation("FireThrower");

            _hasAttacked = true;
        }
    }

    public override void ActiveSkillExit(Monster monster)
    {
        
    }
}

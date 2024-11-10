using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FollowingShot", menuName = "Data/MonsterSKillData/Weeper/FollowingShot")]
public class WeeperFollowingShot : MonsterSkillData
{
    private bool _hasAttacked = false;

    public override void ActiveSkillEnter(Monster monster)
    {
        _hasAttacked = false;
    }

    public override void ActiveSkillTick(Monster monster)
    {
        monster.AnimationController.AnimatorStateInfo = monster.AnimationController.Animator.GetCurrentAnimatorStateInfo(0);
        
        if (!monster.AnimationController.AnimatorStateInfo.IsName("WeeperShot") && !_hasAttacked)
        {
            monster.MovementController.LookAtTarget(monster.CombatController.MonsterCombatAbility.TurnSpeed);

            if (Vector3.Angle(monster.transform.forward, monster.MovementController.Direction) <= 3 && !_hasAttacked)
            {
                monster.AnimationController.PlaySkillAnimation("WeeperShot");
                _hasAttacked = true;
            }
        }
    }

    public override void ActiveSkillExit(Monster monster)
    {

    }
}

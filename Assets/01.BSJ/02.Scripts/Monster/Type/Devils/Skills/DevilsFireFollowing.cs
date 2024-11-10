using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FireFollowing", menuName = "Data/MonsterSKillData/Devils/FireFollowing")]
public class DevilsFireFollowing : MonsterSkillData
{
    private float _currentTime = 0;
    private float _limitTime = 0;

    private bool _hasAttacked = false;
    private bool _isEnded = false;

    public override void ActiveSkillEnter(Monster monster)
    {
        _currentTime = 0;
        _limitTime = CastTime;

        _hasAttacked = false;
        _isEnded = false;
    }

    public override void ActiveSkillTick(Monster monster)
    {
        monster.AnimationController.AnimatorStateInfo = monster.AnimationController.Animator.GetCurrentAnimatorStateInfo(0);

        if (!monster.AnimationController.AnimatorStateInfo.IsTag("Cast"))
        {
            monster.MovementController.LookAtTarget(monster.CombatController.MonsterCombatAbility.TurnSpeed);

            if (Vector3.Angle(monster.transform.forward, monster.MovementController.Direction) <= 3 && !_hasAttacked)
            {
                monster.AnimationController.PlaySkillAnimation("CastStart");
                _hasAttacked = true;
            }
        }
        else
        {
            _currentTime += Time.deltaTime;

            if (_currentTime > _limitTime && !_isEnded)
            {
                monster.AnimationController.PlaySkillAnimation("CastEnd");
                _isEnded = true;
            }
        }
    }

    public override void ActiveSkillExit(Monster monster)
    {
        
    }
}

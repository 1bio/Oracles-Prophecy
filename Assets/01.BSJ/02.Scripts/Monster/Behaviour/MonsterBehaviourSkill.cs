using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBehaviourSkill : MonsterBehaviour
{
    private Monster _monster;
    private MonsterSkillData _skillData;

    public override void OnBehaviourStart(Monster monster)
    {
        monster.AnimationController.IsLockedInAnimation = true;

        _monster = monster;
        _skillData = monster.SkillController.CurrentSkillData;
        _skillData.ActiveSkillEnter(monster);
    }

    public override void OnBehaviourUpdate(Monster monster)
    {
        if (!monster.StateMachineController.IsAlive())
            monster.StateMachineController.OnDead();

        monster.AnimationController.AnimatorStateInfo = monster.AnimationController.Animator.GetCurrentAnimatorStateInfo(0);
        _skillData.ActiveSkillTick(monster);
    }

    public override void OnBehaviourEnd(Monster monster)
    {
        _skillData?.ActiveSkillExit(monster);
        monster.SkillController.CurrentSkillData.CooldownTimer = 0f;

        monster.MovementController.Astar?.StartPathCalculation(monster.transform.position, monster.MovementController.Astar.TargetTransform.position);
    }
}

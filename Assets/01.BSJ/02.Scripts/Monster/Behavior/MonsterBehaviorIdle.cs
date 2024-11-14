using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBehaviorIdle : MonsterBehavior
{
    private Monster _monster;
    public override void OnBehaviorStart(Monster monster)
    {
        _monster = monster;
        _monster.CombatController.Health.SetHealth(monster.CombatController.MonsterCombatAbility.MonsterHealth.CurrentHealth);

        monster.AnimationController.PlayIdleAnimation();
    }

    public override void OnBehaviorUpdate(Monster monster)
    {
        if (monster.MovementController.TargetDetector.IsTargetDetected)
            monster.MovementController.LookAtTarget(monster.CombatController.MonsterCombatAbility.TurnSpeed);
    }

    public override void OnBehaviorEnd(Monster monster)
    {

    }
}

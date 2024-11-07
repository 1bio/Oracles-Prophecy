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

        monster.CombatController.Health.ImpactEvent += OnImpact;
    }

    public override void OnBehaviorUpdate(Monster monster)
    {
        monster.MovementController.LookAtTarget(monster.CombatController.MonsterCombatAbility.TurnSpeed);
        monster.MovementController.Astar.StartPathCalculation(monster.transform.position, monster.MovementController.Astar.TargetTransform.position);
    }

    public override void OnBehaviorEnd(Monster monster)
    {
        monster.CombatController.Health.ImpactEvent -= OnImpact;
    }

    private void OnImpact()
    {
        _monster.StateMachineController.OnGotHit();
    }
}

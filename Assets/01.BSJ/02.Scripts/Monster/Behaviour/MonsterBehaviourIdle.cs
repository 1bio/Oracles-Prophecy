using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBehaviourIdle : MonsterBehaviour
{
    private Monster _monster;
    public override void OnBehaviourStart(Monster monster)
    {
        _monster = monster;
        _monster.CombatController.Health.SetHealth(monster.CombatController.MonsterCombatAbility.MonsterHealth.CurrentHealth);

        monster.AnimationController.PlayIdleAnimation();

        monster.CombatController.Health.ImpactEvent += OnImpact;
    }

    public override void OnBehaviourUpdate(Monster monster)
    {
        monster.MovementController.LookAtTarget(monster.CombatController.MonsterCombatAbility.TurnSpeed);
        monster.MovementController.Astar.StartPathCalculation(monster.transform.position, monster.MovementController.Astar.TargetTransform.position);
    }

    public override void OnBehaviourEnd(Monster monster)
    {
        monster.CombatController.Health.ImpactEvent -= OnImpact;
    }

    private void OnImpact()
    {
        _monster.StateMachineController.OnGotHit();
    }
}

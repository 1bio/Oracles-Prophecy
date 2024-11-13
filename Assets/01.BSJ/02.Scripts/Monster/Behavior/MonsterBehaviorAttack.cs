using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterBehaviorAttack : MonsterBehavior
{
    private Monster _monster;
    private bool _hasAttacked = false;
    private float _currentTime = 0;
    private float _attackAngleThreshold = 5f;

    public override void OnBehaviorStart(Monster monster)
    {
        _monster = monster;
        _monster.CombatController.Health.SetHealth(monster.CombatController.MonsterCombatAbility.MonsterHealth.CurrentHealth);

        monster.CombatController.Health.ImpactEvent += OnImpact;
    }

    public override void OnBehaviorUpdate(Monster monster)
    {
        if (!monster.StateMachineController.IsAlive())
            monster.StateMachineController.OnDead();

        _currentTime += Time.deltaTime;

        if (!monster.AnimationController.IsLockedInAnimation)
        {
            monster.MovementController.LookAtTarget(monster.CombatController.MonsterCombatAbility.TurnSpeed);
        }

        if (Vector3.Angle(monster.transform.forward, monster.MovementController.Direction) <= _attackAngleThreshold && !_hasAttacked)
        {
            monster.AnimationController.PlayAttackAnimation(monster.CombatController.MonsterCombatAbility.MonsterAttack.TotalCount);
            
            _hasAttacked = true;
        }
    }

    public override void OnBehaviorEnd(Monster monster)
    {
        monster.MovementController.Astar.StartPathCalculation(monster.transform.position, monster.MovementController.Astar.TargetTransform.position);
        monster.StateMachineController.CurrentBasicAttackCooldownTime = 0;

        monster.CombatController.Health.ImpactEvent -= OnImpact;
    }

    private void OnImpact()
    {
        if (!_monster.AnimationController.IsLockedInAnimation)
        {
            _monster.StateMachineController.OnGotHit();
        }
        //_monster.MonsterStateMachineController.OnGotHit();
    }
}

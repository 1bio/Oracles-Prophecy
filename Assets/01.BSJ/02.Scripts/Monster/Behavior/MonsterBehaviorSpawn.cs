using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class MonsterBehaviorSpawn : MonsterBehavior
{
    private Monster _monster;
    private float _currentTime;
    private float _spawnDuration = 1f;

    public override void OnBehaviorStart(Monster monster)
    {
        _currentTime = 0f;
        _monster = monster;

        //monster.AnimationController.ObjectFadeInOut.StartFadeOut(0);
        monster.AnimationController.PlayIdleAnimation();
        //monster.AnimationController.ObjectFadeInOut.StartFadeIn(_spawnDuration);

        //monster.MonsterCombatController.Health.ImpactEvent += OnImpact;
    }

    public override void OnBehaviorUpdate(Monster monster)
    {
        _currentTime += Time.deltaTime;

        if (_currentTime > _spawnDuration * 1.5f)
            monster.AnimationController.IsLockedInAnimation = false;
    }

    public override void OnBehaviorEnd(Monster monster)
    {
        //monster.MonsterCombatController.Health.ImpactEvent -= OnImpact;
    }

    private void OnImpact()
    {
        _monster.StateMachineController.OnGotHit();
    }
}

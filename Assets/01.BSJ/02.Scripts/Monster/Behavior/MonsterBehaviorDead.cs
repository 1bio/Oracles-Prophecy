using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBehaviorDead : MonsterBehavior
{
    private GameObject _monsterObj;

    public override void OnBehaviorStart(Monster monster)
    {
        monster.ParticleController?.AllClearVFXs();
        monster.AnimationController.PlayDeathAnimation();
        _monsterObj = monster.gameObject;

        monster.LootItemController.DropLootItems(monster.transform.position);

        // 몬스터 Data에 있는 string 타입으로 접근
        DataManager.instance.ExpUp(monster);
    }

    public override void OnBehaviorUpdate(Monster monster)
    {
        monster.AnimationController.AnimatorStateInfo = monster.AnimationController.Animator.GetCurrentAnimatorStateInfo(0);

        if (monster.AnimationController.AnimatorStateInfo.IsName("Death") &&
            monster.AnimationController.AnimatorStateInfo.normalizedTime >= 0.9f)
        {
            monster.AnimationController.IsLockedInAnimation = false;
            _monsterObj.SetActive(false);
        }
    }

    public override void OnBehaviorEnd(Monster monster)
    {
        
    }
}
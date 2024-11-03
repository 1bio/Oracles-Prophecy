using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBehaviourDead : MonsterBehaviour
{
    private GameObject _monsterObj;

    public override void OnBehaviourStart(Monster monster)
    {
        monster.ParticleController?.AllClearVFXs();
        monster.AnimationController.PlayDeathAnimation();
        _monsterObj = monster.gameObject;

        monster.LootItemController.DropLootItems(monster.transform.position);
        
        DataManager.instance.ExpUp(_monsterObj.gameObject.name);
    }

    public override void OnBehaviourUpdate(Monster monster)
    {
        monster.AnimationController.AnimatorStateInfo = monster.AnimationController.Animator.GetCurrentAnimatorStateInfo(0);

        if (monster.AnimationController.AnimatorStateInfo.IsName("Death") &&
            monster.AnimationController.AnimatorStateInfo.normalizedTime >= 0.9f)
        {
            monster.AnimationController.IsLockedInAnimation = false;
            _monsterObj.SetActive(false);
        }
    }

    public override void OnBehaviourEnd(Monster monster)
    {
        
    }
}
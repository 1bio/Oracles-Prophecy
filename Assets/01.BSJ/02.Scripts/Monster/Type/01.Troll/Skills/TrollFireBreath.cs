using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "TrollFireBreath", menuName = "Data/MonsterSKillData/Troll/FireBreath")]
public class TrollFireBreath : MonsterSkillData
{
    private Troll _troll;
    private Transform _vfxTransform;

    private bool _hasAttacked = false;

    public override void ActiveSkillEnter(Monster monster)
    {
        _troll = (Troll)monster;

        monster.ParticleController.VFXTransform["RedFlameThrower"] = _troll.FirePositionTransform;

        _vfxTransform = monster.ParticleController.GetAvailableParticle("RedFlameThrower").transform;
        _vfxTransform.SetParent(_troll.FirePositionTransform);
        _vfxTransform.position = _troll.FirePositionTransform.position;
        _vfxTransform.rotation = _troll.FirePositionTransform.rotation;

        _hasAttacked = false;
    }

    public override void ActiveSkillTick(Monster monster)
    {
        monster.MovementController.LookAtTarget(monster.CombatController.MonsterCombatAbility.TurnSpeed);

        if (!_hasAttacked)
        {
            monster.AnimationController.PlaySkillAnimation(Troll.FireBreathAnimationName.FireBreath.ToString());
            _hasAttacked = true;
        }
    }

    public override void ActiveSkillExit(Monster monster)
    {
        

    }
}
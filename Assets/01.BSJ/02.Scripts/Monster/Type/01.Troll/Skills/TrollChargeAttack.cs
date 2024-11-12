using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "ChargeAttack", menuName = "Data/MonsterSKillData/Troll/ChargeAttack")]
public class TrollChargeAttack : MonsterSkillData
{
    private Transform _vfxTransform;

    private float _hitSphere;

    private HashSet<Collider> damagedPlayers = new HashSet<Collider>();
    private bool _hasAttacked = false;
    private bool _hasHit = false;

    public override void ActiveSkillEnter(Monster monster)
    {
        _hitSphere = 2f;

        damagedPlayers = new HashSet<Collider>();
        _hasAttacked = false;
        _hasHit = false;
    }

    public override void ActiveSkillTick(Monster monster)
    {
        if (!monster.AnimationController.AnimatorStateInfo.IsName(Troll.ChargeAttackAnimationName.ChargeAttack.ToString()))
        {
            monster.MovementController.LookAtTarget(monster.CombatController.MonsterCombatAbility.TurnSpeed);
        }
        else
        {
            ParticleSystem particleSystem = monster.ParticleController.GetAvailableParticle("SmokeCircle");

            if (particleSystem != null)
            {
                _vfxTransform = particleSystem.transform;
                _vfxTransform.position = monster.transform.position + new Vector3(0, 0.2f,0) + monster.transform.forward * _hitSphere;
                particleSystem = monster.ParticleController.CurrentParticleSystem;
                if (particleSystem != null && particleSystem.isPlaying && !_hasHit)
                {
                    Hit(_vfxTransform.position, monster);
                }
            }
        }

        if (!_hasAttacked)
        {
            monster.AnimationController.PlaySkillAnimation(Troll.ChargeAttackAnimationName.ChargeAttack.ToString());
            _hasAttacked = true;
        }
    }

    public override void ActiveSkillExit(Monster monster)
    {
        
    }
    private void Hit(Vector3 position, Monster monster)
    {
        int layermask = (1 << LayerMask.NameToLayer(GameLayers.Player.ToString()));
        Collider[] colliders = Physics.OverlapSphere(position, _hitSphere, layermask);

        foreach (Collider collider in colliders)
        {
            if (damagedPlayers.Add(collider))
            {
                collider.gameObject.GetComponent<Health>()?.TakeDamage(monster.SkillController.CurrentSkillData.Damage, true);
            }
        }
        _hasHit = true;
    }
}

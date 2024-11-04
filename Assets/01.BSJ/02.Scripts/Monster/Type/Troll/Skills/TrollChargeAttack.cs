using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "TrollChargeAttack", menuName = "Data/MonsterSKillData/Troll/ChargeAttack")]
public class TrollChargeAttack : MonsterSkillData
{
    private Transform _vfxTransform;

    private Transform _swordObjectTransform;
    private string _swordHierarchyPath = "Ogre_root/Hips/Spine/Spine1/Spine2/RightShoulder/RightArm/RightForeArm/RightHand/Right_weapon/Sword";

    private float _hitSphere;

    private HashSet<Collider> damagedPlayers = new HashSet<Collider>();
    private bool _hasAttacked = false;
    private bool _hasHit = false;

    public override void ActiveSkillEnter(Monster monster)
    {
        _vfxTransform = monster.ParticleController.GetAvailableParticle("SmokeCircle").transform;

        _swordObjectTransform = monster.transform.Find(_swordHierarchyPath);

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
            ParticleSystem particleSystem = monster.ParticleController.CurrentParticleSystem;

            if (particleSystem != null)
            {
                _vfxTransform = particleSystem.transform;
                _vfxTransform.position = monster.transform.position + monster.transform.forward;

                if (particleSystem.isPlaying && !_hasHit)
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

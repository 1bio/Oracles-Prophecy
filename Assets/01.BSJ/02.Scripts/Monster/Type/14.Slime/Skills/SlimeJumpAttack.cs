using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "JumpAttack", menuName = "Data/MonsterSKillData/Slime/JumpAttack")]
public class SlimeJumpAttack : MonsterSkillData
{
    private Transform _vfxTransform;
    private Vector3 _targetPosition;

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
        monster.AnimationController.AnimatorStateInfo = monster.AnimationController.Animator.GetCurrentAnimatorStateInfo(0);

        if (!monster.AnimationController.AnimatorStateInfo.IsName(Slime.JumpAttackAnimation.JumpAttack.ToString()))
        {
            monster.MovementController.LookAtTarget(monster.CombatController.MonsterCombatAbility.TurnSpeed);
            _targetPosition = monster.MovementController.Astar.TargetTransform.position;
        }
        else
        {
            ParticleSystem particleSystem = monster.ParticleController.GetAvailableParticle("SmokeCircle");

            if (particleSystem != null)
            {
                _vfxTransform = particleSystem.transform;
                _vfxTransform.position = monster.transform.position;
                particleSystem = monster.ParticleController.CurrentParticleSystem;
                if (particleSystem != null && particleSystem.isPlaying && !_hasHit)
                {
                    GroundHit(_vfxTransform.position, monster);
                }
            }
        }

        if (!_hasAttacked)
        {
            monster.AnimationController.PlaySkillAnimation(Slime.JumpAttackAnimation.JumpAttack.ToString());
            _hasAttacked = true;
        }
    }

    public override void ActiveSkillExit(Monster monster)
    {

    }

    private void GroundHit(Vector3 position, Monster monster)
    {
        int layerMask = (1 << LayerMask.NameToLayer(GameLayers.Ground.ToString()));
        if (Physics.Raycast(monster.transform.position, Vector3.down, 1, layerMask))
        {
            Hit(position, monster);
        }
    }

    private void Hit(Vector3 position, Monster monster)
    {
        int layerMask = (1 << LayerMask.NameToLayer(GameLayers.Player.ToString()));
        Collider[] colliders = Physics.OverlapSphere(position, _hitSphere, layerMask);

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

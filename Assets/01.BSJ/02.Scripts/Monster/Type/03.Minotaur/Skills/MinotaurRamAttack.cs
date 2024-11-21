using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "MinotaurRamAttack", menuName = "Data/MonsterSKillData/Minotaur/RamAttack")]
public class MinotaurRamAttack : MonsterSkillData
{
    private Minotaur _minotaur;
    private Transform _vfxTransform;

    private int _currentAttackCount;
    private int _maxAttackLimit;    // 최대 공격 횟수

    private bool _hasRamStarted;
    private bool _hasHitObject;

    // Raycast 관련
    private RaycastHit _hit;
    private float _maxDistance;
    private float _detectionRadius;

    //private Indicator _indicator;

    private void InitializeValues(Monster monster)
    {
        _minotaur = (Minotaur)monster;
        
        // VFX 초기화
        _vfxTransform = monster.ParticleController.VFX["SmokeCircle"][0].transform;
        _vfxTransform.SetParent(monster.gameObject.transform);
        _vfxTransform.localPosition = Vector3.zero;

        _currentAttackCount = 0;
        _maxAttackLimit = 3;

        _hasRamStarted = false;
        _hasHitObject = false;

        _maxDistance = 1f;
        _detectionRadius = 0.5f;
    }

    public override void ActiveSkillEnter(Monster monster)
    {
        InitializeValues(monster);
        
        //_indicator = monster.GetComponentInChildren<Indicator>(true);
        /*Debug.Log($"ActiveSkillEnter called. _hasRamStarted: {_hasRamStarted}");*/
    }

    public override void ActiveSkillTick(Monster monster)
    {
        if (!_hasRamStarted)
        {
            StartRamAttack(monster);
        }
        /*Debug.Log($"_currentAttackCount: {_currentAttackCount}");*/
        HandleAnimationState(monster);
    }

    public override void ActiveSkillExit(Monster monster)
    {

    }

    private void StartRamAttack(Monster monster)
    {
        //_indicator.gameObject.SetActive(true);

        monster.MovementController.CharacterController.SimpleMove(Vector3.zero);
        monster.AnimationController.PlaySkillAnimation(Minotaur.RamAttackAnimationName.RamStart.ToString());
        _hasRamStarted = true;
    }

    private void HandleAnimationState(Monster monster)
    {
        AnimatorStateInfo stateInfo = monster.AnimationController.AnimatorStateInfo;

        if (stateInfo.IsName(Minotaur.RamAttackAnimationName.RamStart.ToString()))
        {
            monster.MovementController.CharacterController.SimpleMove(Vector3.zero);
            monster.MovementController.LookAtTarget(monster.CombatController.MonsterCombatAbility.TurnSpeed);
        }
        else if (stateInfo.IsName(Minotaur.RamAttackAnimationName.RamRun.ToString()))
        {
            if (IsInFanShapeDetection(monster))
            {
                HandleRaycastHit(monster);
            }
            else
            {
                _hasHitObject = false;
            }
            monster.MovementController.CharacterController.SimpleMove(monster.transform.forward * monster.CombatController.MonsterCombatAbility.MoveSpeed * 3f);
        }
        else if (stateInfo.IsName(Minotaur.RamAttackAnimationName.RamWall.ToString())
            && stateInfo.normalizedTime >= 0.8f)
        {
            _hasRamStarted = false;
        }
    }

    private bool IsInFanShapeDetection(Monster monster)
    {
        Vector3 direction = monster.transform.forward;

        int layerMask = (1 << LayerMask.NameToLayer(GameLayers.Player.ToString())) |
                        (1 << LayerMask.NameToLayer(GameLayers.Obstacle.ToString()));

        if (Physics.SphereCast(monster.transform.position, _detectionRadius, direction, out _hit, _maxDistance, layerMask))
        {
            if (_hit.collider.gameObject.layer == LayerMask.NameToLayer(GameLayers.Player.ToString()) ||
                _hit.collider.gameObject.layer == LayerMask.NameToLayer(GameLayers.Obstacle.ToString()))
            {
                return true;
            }
        }
        return false;
    }

    private void HandleRaycastHit(Monster monster)
    {
        AnimatorStateInfo stateInfo = monster.AnimationController.Animator.GetCurrentAnimatorStateInfo(0);

        if (_hit.collider.gameObject.layer == LayerMask.NameToLayer(GameLayers.Player.ToString()))
        {
            if (!_hasHitObject &&
                !stateInfo.IsName(Minotaur.RamAttackAnimationName.RamAttack.ToString()))
            {
                monster.MovementController.CharacterController.SimpleMove(Vector3.zero);
                monster.AnimationController.PlaySkillAnimation(Minotaur.RamAttackAnimationName.RamAttack.ToString());
                _hasHitObject = true;

                _hit.collider.gameObject.GetComponentInParent<Health>().TakeDamage(monster.SkillController.CurrentSkillData.Damage, true);

                monster.CameraShake.ShakeCamera(2, 0.5f);
            }
        }
        else if (_hit.collider.gameObject.layer == LayerMask.NameToLayer(GameLayers.Obstacle.ToString()))
        {
            if (!_hasHitObject &&
                !stateInfo.IsName(Minotaur.RamAttackAnimationName.RamWall.ToString()) &&
                !stateInfo.IsName(Minotaur.RamAttackAnimationName.RamAttack.ToString()))
            {
                _currentAttackCount++;
                monster.MovementController.CharacterController.SimpleMove(Vector3.zero);

                if (CheckIfAttackCountExceedsLimit())
                {
                    monster.AnimationController.PlaySkillAnimation(Minotaur.RamAttackAnimationName.RamAttack.ToString());
                }
                else
                {
                    monster.AnimationController.PlaySkillAnimation(Minotaur.RamAttackAnimationName.RamWall.ToString());
                }

                monster.CameraShake.ShakeCamera(2, 0.5f);
                //_indicator.gameObject.SetActive(false);
                _hasHitObject = true;
            }
        }
    }

    private bool CheckIfAttackCountExceedsLimit()
    {
        return _currentAttackCount >= _maxAttackLimit;
    }
}
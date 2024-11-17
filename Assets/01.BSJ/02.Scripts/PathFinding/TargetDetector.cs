using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class TargetDetector : MonoBehaviour
{
    private Monster _monster;
    private Health _playerHealth;

    // Raycast ฐüทร
    private RaycastHit _hit;
    [SerializeField] private float _detectionDistance = 5f;
    private float _fanAngle = 50f;
    private float _fanCount = 25f;

    private float _currentTime = 0;
    private float _updateTime = 100f;

    public bool IsTargetDetected { get; set; } = false;
    public float DetectionDistance { get => _detectionDistance; }

    private void Awake()
    {
        _monster = GetComponent<Monster>();
        IsTargetDetected = false;
        _playerHealth = GameObject.Find("Player")?.GetComponent<Health>();
    }

    private void FixedUpdate()
    {
        if (!IsTargetDetected)
        {
            if (IsInFanShapeDetection(_detectionDistance))
            {
                IsTargetDetected = true;
            }
        }
        else
        {
            BGMAudioManager.SetIsCombat(IsTargetDetected);

            if (_monster.SkillController.CurrentSkillData != null)
            {
                _monster.SkillController.CurrentSkillData.IsTargetWithinSkillRange =
                IsInFanShapeDetection(_monster.SkillController.CurrentSkillData.Range);
            }

            _monster.CombatController.MonsterCombatAbility.MonsterAttack.IsTargetWithinAttackRange =
               IsInFanShapeDetection(_monster.CombatController.MonsterCombatAbility.MonsterAttack.Range);

            if (IsInFanShapeDetection(_detectionDistance))
                _currentTime = 0;
            else
                _currentTime += Time.deltaTime;
        }

        if (_currentTime > _updateTime)
        {
            IsTargetDetected = IsInFanShapeDetection(_detectionDistance);
            BGMAudioManager.SetIsCombat(IsTargetDetected);
            _currentTime = 0;
        }

        if (!_monster.StateMachineController.IsAlive())
        {
            IsTargetDetected = false;
            BGMAudioManager.SetIsCombat(IsTargetDetected);
        }
    }

    public bool IsInFanShapeDetection(float detectionDistance)
    {
        int layerMask = (1 << LayerMask.NameToLayer(GameLayers.Player.ToString())) |
                        (1 << LayerMask.NameToLayer(GameLayers.Obstacle.ToString()));

        Vector3 startPos = transform.position + new Vector3(0, 1f, 0);

        for (int i = 0; i < _fanCount; i++)
        {
            float angle = 0;
            if (i > 0)
            {
                angle = (i % 2 == 0) ? angle - (i * (_fanAngle / (_fanCount - 1))) : angle + (i * (_fanAngle / (_fanCount - 1)));
            }
            Vector3 direction = Quaternion.Euler(0, angle, 0) * transform.forward;

            if (Physics.Raycast(startPos, direction, out _hit, detectionDistance, layerMask))
            {
                //Debug.Log(_hit.collider.gameObject.name);
                Debug.DrawRay(startPos, direction * detectionDistance, Color.red);

                if (_hit.distance > detectionDistance)
                    return false;

                if (_hit.collider.gameObject.layer == LayerMask.NameToLayer(GameLayers.Player.ToString()))
                {
                    return true;
                }
                else if (_hit.collider.gameObject.layer == LayerMask.NameToLayer(GameLayers.Obstacle.ToString()))
                {
                    return false;
                }
            }
            else
            {
                Debug.DrawRay(startPos, direction * detectionDistance, Color.yellow);
            }
        }
        return false;
    }
}

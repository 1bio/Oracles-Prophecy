using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MonsterAttack
{
    [SerializeField] private float _damage;
    [SerializeField] private float _range;
    [SerializeField] private float _cooldownThreshold;
    [SerializeField] private int _totalCount;

    private bool _isTargetWithinAttackRange = false;
    private bool _isEnableWeapon = false;

    public MonsterAttack(float damage, float range, float cooldownThreshold, int totalCount, bool isTargetWithinAttackRange, bool isEnableWeapon)
    {
        _damage = damage;
        _range = range;
        _cooldownThreshold = cooldownThreshold;
        _totalCount = totalCount;
        _isTargetWithinAttackRange = isTargetWithinAttackRange;
        _isEnableWeapon = isEnableWeapon;
    }

    public float Damage { get => _damage; set => _damage = value; }
    public float Range { get => _range; set => _range = value; }
    public float CooldownThreshold { get => _cooldownThreshold; set => _cooldownThreshold = value; }
    public int TotalCount { get => _totalCount; set => _totalCount = value; }
    public bool IsTargetWithinAttackRange { get => _isTargetWithinAttackRange; set => _isTargetWithinAttackRange = value; }
    public bool IsEnableWeapon { get => _isEnableWeapon; set => _isEnableWeapon = value; }
}

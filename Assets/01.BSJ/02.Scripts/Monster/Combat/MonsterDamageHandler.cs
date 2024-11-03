using System.Collections;
using UnityEngine;

public class MonsterDamageHandler : MonoBehaviour
{
    private Monster _monster;
    private Health _playerHealth;

    public void SetMonster(Monster monster)
    {
        _monster = monster;
    }

    private void Awake()
    {
        _monster = GetComponentInParent<Monster>();
        _playerHealth = GameObject.Find("Player").GetComponent<Health>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_monster.CombatController.MonsterCombatAbility.MonsterAttack.IsEnableWeapon &&
            other.gameObject.layer == LayerMask.NameToLayer(GameLayers.Player.ToString()))
        {
            Debug.Log("Player Hit");
            _playerHealth?.TakeDamage(_monster.CombatController.MonsterCombatAbility.MonsterAttack.Damage, true);
        }
    }
}

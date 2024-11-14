using System.Collections;
using UnityEngine;

public class MonsterDamageHandler : MonoBehaviour
{
    private Monster _monster;
    private Health _playerHealth;
    private CameraShake _cameraShake;

    public void SetMonster(Monster monster)
    {
        _monster = monster;
        _cameraShake = monster.GetComponent<CameraShake>();
    }

    private void Awake()
    {
        _monster = GetComponentInParent<Monster>();
        _playerHealth = GameObject.Find("Player")?.GetComponent<Health>();
        _cameraShake = GetComponentInParent<CameraShake>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_monster.CombatController.MonsterCombatAbility.MonsterAttack.IsEnableWeapon &&
            other.gameObject.layer == LayerMask.NameToLayer(GameLayers.Player.ToString()))
        {
            Debug.Log("Player Hit");
            _playerHealth?.TakeDamage(_monster.CombatController.MonsterCombatAbility.MonsterAttack.Damage, true);
            _cameraShake.ShakeCamera(1.5f, 0.3f);
        }
    }
}

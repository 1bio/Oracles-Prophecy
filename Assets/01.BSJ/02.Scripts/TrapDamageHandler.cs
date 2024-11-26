using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapDamageHandler : MonoBehaviour
{
    [SerializeField] private float _damage;
    [SerializeField] private float _damageInterval;
    private float _nextDamageTime = 0;

    private void OnTriggerStay(Collider other)
    {
        if (Time.time < _nextDamageTime) return;

        if (other.gameObject.layer == LayerMask.NameToLayer(GameLayers.Player.ToString()))
        {
            Health playerHealth = other.gameObject.GetComponent<Health>();

            playerHealth?.TakeDamage(_damage, true);

            _nextDamageTime = Time.time + _damageInterval;
        }
    }
}

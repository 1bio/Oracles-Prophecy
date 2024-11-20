using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapDamageHandler : MonoBehaviour
{
    [SerializeField] private float _damage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(GameLayers.Player.ToString()))
        {
            Health playerHealth = other.gameObject.GetComponent<Health>();

            playerHealth?.TakeDamage(_damage, true);
        }
    }
}

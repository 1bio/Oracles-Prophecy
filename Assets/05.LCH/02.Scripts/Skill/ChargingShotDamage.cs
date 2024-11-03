using UnityEngine;

public class ChargingShotDamage : MonoBehaviour
{
    private float damage;

    private void Start()
    {
        damage = DataManager.instance.playerData.skillData[0].damage;
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.TryGetComponent<Health>(out Health health))
        {
            health?.TakeDamage(damage, false);
        }
    }
}

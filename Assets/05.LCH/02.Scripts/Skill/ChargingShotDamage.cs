using UnityEngine;

public class ChargingShotDamage : MonoBehaviour
{
    private float damage;

    private void OnParticleCollision(GameObject other)
    {
        if (other.TryGetComponent<Health>(out Health health))
        {
            float min = DataManager.instance.playerData.skillData[0].minDamage;
            float max = DataManager.instance.playerData.skillData[0].maxDamage;
            damage = Random.Range(min, max);

            health?.TakeDamage(damage, false);
        }
    }
}

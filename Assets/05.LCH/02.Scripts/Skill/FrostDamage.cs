using UnityEngine;

public class FrostDamage : MonoBehaviour
{
    private float damage;

    private void Start()
    {
        damage = DataManager.instance.playerData.skillData[5].damage;
    }

    private void OnParticleCollision(GameObject other)
    {
        other.GetComponent<Health>().TakeDamage(damage, false);
    }

}

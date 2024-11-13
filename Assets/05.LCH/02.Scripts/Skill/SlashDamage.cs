using UnityEngine;

public class SlashDamage : MonoBehaviour
{
    private float damage;

    private void Start()
    {
        damage = DataManager.instance.playerData.skillData[3].damage;
    }

    private void OnParticleCollision(GameObject other)
    {
        other.GetComponent<Health>().TakeDamage(damage, false);
    }

}

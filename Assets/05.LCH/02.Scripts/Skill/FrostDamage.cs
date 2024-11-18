using UnityEngine;

public class FrostDamage : MonoBehaviour
{
    private float damage;

    private int hitCount = 3;

    private void Start()
    {
        damage = DataManager.instance.playerData.skillData[5].damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        Health health = other.GetComponent<Health>();

        for (int i = 0; i < hitCount; i++)
        {
            health.TakeDamage(damage, false);
        }
    }

}

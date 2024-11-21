using UnityEngine;

public class FrostDamage : MonoBehaviour
{
    private float damage;
    private float knockBack;

    private int hitCount = 3;

    private void Start()
    {
        damage = DataManager.instance.playerData.skillData[5].damage;
        knockBack = DataManager.instance.playerData.skillData[5].knockBack;
    }

    private void OnTriggerEnter(Collider other)
    {
        Health health = other.GetComponent<Health>();

        if (other.gameObject.TryGetComponent<ForceReceiver>(out ForceReceiver forceReceiver))
        {
            if (other.CompareTag("Player"))
                return;

            Vector3 direction = (other.transform.position - transform.position).normalized;

            forceReceiver.AddForce(direction * knockBack);
        }

        for (int i = 0; i < hitCount; i++)
        {
            health.TakeDamage(damage, false);
        }
    }

}

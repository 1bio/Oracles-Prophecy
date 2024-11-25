using UnityEngine;

public class FrostDamage : MonoBehaviour
{
    [SerializeField] private Collider collider;

    private float damage;
    private float knockBack;

    private int hitCount = 3;

    private void Start()
    {
        knockBack = DataManager.instance.playerData.skillData[5].knockBack;
    }

    private void OnDisable()
    {
        collider.enabled = true;   
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
            float min = DataManager.instance.playerData.skillData[5].minDamage;
            float max = DataManager.instance.playerData.skillData[5].minDamage;
            damage = Random.Range(min, max);

            health.TakeDamage(damage, false);
        }

        collider.enabled = false;
    }

}

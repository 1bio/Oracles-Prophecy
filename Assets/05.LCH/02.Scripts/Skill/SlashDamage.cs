using System.Collections;
using UnityEngine;

public class SlashDamage : MonoBehaviour
{
    private float damage;
    private float knockBack;

    private int hitCount = 2;

    private void OnEnable()
    {
        knockBack = DataManager.instance.playerData.skillData[3].knockBack;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<ForceReceiver>(out ForceReceiver forceReceiver))
        {
            if (other.CompareTag("Player"))
                return;

            Vector3 direction = (other.transform.position - transform.position).normalized;

            forceReceiver.AddForce(direction * knockBack);
        }

        if (other.gameObject.TryGetComponent<Health>(out Health health))
        {
            for (int i = 0; i < hitCount; i++)
            {
                float min = DataManager.instance.playerData.skillData[3].minDamage;
                float max = DataManager.instance.playerData.skillData[3].maxDamage;
                damage = Random.Range(min, max);

                health?.TakeDamage(damage, false);

                StartCoroutine(Deactivate());
            }
        }
    }

    IEnumerator Deactivate()
    {
        yield return new WaitForSeconds(0.5f);

        gameObject.SetActive(false);
    }
}

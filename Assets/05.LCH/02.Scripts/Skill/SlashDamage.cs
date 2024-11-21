using System.Collections;
using UnityEngine;

public class SlashDamage : MonoBehaviour
{
    private float damage;
    private float knockBack;

    private int hitCount = 2;

    private void OnEnable()
    {
        damage = DataManager.instance.playerData.skillData[3].damage;
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

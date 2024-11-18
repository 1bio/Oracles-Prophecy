using System.Collections;
using UnityEngine;

public class SlashDamage : MonoBehaviour
{
    private float damage;
    private int hitCount = 2;

    private void Start()
    {
        damage = DataManager.instance.playerData.skillData[3].damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent<Health>(out Health health))
        {
            for (int i = 0; i < hitCount; i++)
            {
                health.TakeDamage(damage, false);

                StartCoroutine(Deactivate());
            }
        }
    }

    IEnumerator Deactivate()
    {
        yield return new WaitForSeconds(1f);

        gameObject.SetActive(false);
    }
}

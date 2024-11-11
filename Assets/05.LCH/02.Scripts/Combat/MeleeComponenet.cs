using UnityEngine;

public class MeleeComponenet : MonoBehaviour
{
    [SerializeField] private CameraShake cameraShake;

    private float damage;
    private float knockBack;

    // 공격력 및 넉백
    public void SetAttack(float minDamage, float maxDamage/*, float damage, float knockBack*/)
    {
        this.damage = Mathf.Floor(Random.Range(minDamage, maxDamage));
        /*this.damage = damage;
        this.knockBack = knockBack;*/
    }


    #region Collision Methods
    private void OnTriggerEnter(Collider other) // 몬스터 피격 처리
    {
        if (other.TryGetComponent<ForceReceiver>(out ForceReceiver forceReceiver))
        {
            Vector3 direction = (other.transform.position - transform.position).normalized;

            forceReceiver.AddForce(direction * knockBack);
        }

        if (other.TryGetComponent<Health>(out Health health))
        {
            if (other.CompareTag("Player"))
                return;
            
            if (health != null && health.isAlive)
            {
                cameraShake.ShakeCamera(1.2f, 0.2f);

                health.TakeDamage(damage, false);
            }
        }
    }
    #endregion
}

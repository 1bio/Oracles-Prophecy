using UnityEngine;

public class RangeComponent : MonoBehaviour
{
    [SerializeField] private Transform shootingTransform;

    [SerializeField] private float projectileSpeed; // 투사체 속도

    // 애니메이션 이벤트
    public void Shoot() // 일반 공격
    {
        GameObject projectile = PoolManager.instance.Get(0);

        projectile.transform.position = shootingTransform.position;
        projectile.transform.rotation = shootingTransform.rotation;

        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        rb.velocity = shootingTransform.transform.forward * projectileSpeed;
    }
}

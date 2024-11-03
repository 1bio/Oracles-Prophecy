using UnityEngine;

public class RangeComponent : MonoBehaviour
{
    [SerializeField] private Transform shootingTransform;

    [SerializeField] private float projectileSpeed; // ����ü �ӵ�

    // �ִϸ��̼� �̺�Ʈ
    public void Shoot() // �Ϲ� ����
    {
        GameObject projectile = PoolManager.instance.Get(0);

        projectile.transform.position = shootingTransform.position;
        projectile.transform.rotation = shootingTransform.rotation;

        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        rb.velocity = shootingTransform.transform.forward * projectileSpeed;
    }
}

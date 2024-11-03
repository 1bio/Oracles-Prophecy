using System.Collections;
using UnityEngine;
using UnityEngine.Timeline;

public class Projectile : MonoBehaviour
{
    [SerializeField] private GameObject trail;
    [SerializeField] private float delay = 1f;

    WaitForSeconds wait;

    private Rigidbody arrow_rigid;
    private Collider arrow_col;

    private float damage;
    private float knockBack;


    private void OnEnable()
    {
        damage = DataManager.instance.playerData.rangeAttackData.Damage;

        wait = new WaitForSeconds(delay); // 캐싱

        arrow_rigid = GetComponent<Rigidbody>();
        arrow_col = GetComponent<Collider>();

        arrow_rigid.isKinematic = false;
        arrow_col.isTrigger = false;
        trail.SetActive(true);
    }


    #region Collision Methods
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Health>(out Health health))
        {
            if (collision.gameObject.CompareTag("Player"))
                return;

            if (collision.gameObject.CompareTag("Projectile"))
                return;

            arrow_rigid.isKinematic = true; // 물리 연산 중지
            arrow_col.isTrigger = true; // 충돌 중지
            StartCoroutine(TrailOffDelay()); // 트레일 끄기


            // 충돌 지점으로 위치 이동
            transform.position = collision.contacts[0].point;

            transform.SetParent(collision.transform); 

            health.TakeDamage(damage, false);

            Debug.Log("Hit!");

            return;
        }

        // 벽이나 다른 오브젝트에 맞을 경우
        arrow_rigid.isKinematic = true; // 물리 연산 중지
        arrow_col.isTrigger = true; // 충돌 중지
        StartCoroutine(TrailOffDelay()); // 트레일 끄기
    }


    IEnumerator TrailOffDelay()
    {
        yield return wait;
        trail.SetActive(false);
    }
    #endregion
}


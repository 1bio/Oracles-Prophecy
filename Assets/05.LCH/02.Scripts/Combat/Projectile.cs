using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Camera camera;

    // 트레일
    [SerializeField] private GameObject trail;
    [SerializeField] private float delay = 1f;

    WaitForSeconds wait;

    private Rigidbody arrow_rigid;
    private Collider arrow_col;

    private float damage;
    private float knockBack;


    private void Awake()
    {
        camera = Camera.main;

        wait = new WaitForSeconds(delay); 
    }

    private void OnEnable()
    {
        StatusData status = DataManager.instance.playerData.statusData;

        this.damage = Random.Range(status.minDamage, status.maxDamage);

        arrow_rigid = GetComponent<Rigidbody>();
        arrow_col = GetComponent<Collider>();

        arrow_rigid.isKinematic = false;
        arrow_col.isTrigger = false;
        trail.SetActive(true);
    }

    private void Update()
    {
        //ArrowRange(); 
    }

    #region Main Methods
    private void OnCollisionEnter(Collision collision) // 몬스터 피격 시
    {
        if (collision.gameObject.TryGetComponent<Health>(out Health health))
        {
            if (collision.gameObject.CompareTag("Player"))
                return;

            if (collision.gameObject.CompareTag("Projectile"))
                return;

            // 피격 시 화살 비활성화
            health.TakeDamage(damage, false);
            gameObject.SetActive(false);
            Debug.Log("Hit!");

            /*arrow_rigid.isKinematic = true; // 물리 연산 중지
            arrow_col.isTrigger = true; // 충돌 중지
            StartCoroutine(TrailOffDelay()); // 트레일 끄기

            // 충돌 지점으로 위치 이동
            transform.position = collision.contacts[0].point;

            transform.SetParent(collision.transform); 

            health.TakeDamage(damage, false);

            Debug.Log("Arrow Hit!");*/

            return;
        }

        // 벽이나 다른 오브젝트에 맞을 경우
        arrow_rigid.isKinematic = true; // 물리 연산 중지
        arrow_col.isTrigger = true; // 충돌 중지
        StartCoroutine(TrailOffDelay()); // 트레일 끄기
    }

    // 화살 사정거리(수정 예정)
    public void ArrowRange()
    {
        Vector3 screenPoint = camera.WorldToViewportPoint(transform.position);

        if(screenPoint.x < -1.2f || screenPoint.x > 1.2f || screenPoint.y < -1.2f || screenPoint.y > 1.2f)
        {
            gameObject.SetActive(false);
        }
    }

    // 화살 트레일 딜레이 후 제거
    IEnumerator TrailOffDelay()
    {
        yield return wait;
        trail.SetActive(false);
    }
    #endregion
}


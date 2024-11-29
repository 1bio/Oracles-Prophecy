using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Camera camera;

    // 트레일
    [SerializeField] private GameObject trail;

    private float damage;
    private float knockBack;

    private void Awake()
    {
        camera = Camera.main;
    }

    private void OnEnable()
    {
        StatusData status = DataManager.instance.playerData.statusData;
        this.damage = Random.Range(status.minDamage, status.maxDamage);

        //trail.SetActive(true);
    }

    private void Update()
    {
        ArrowRange();
    }

    #region Main Methods
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            return;

        if (other.CompareTag("Wall"))
        {
            gameObject.SetActive(false);
            //StartCoroutine(TrailOffDelay(3f));
            return;
        }

        if (other.CompareTag("Obstacle"))
        {
            gameObject.SetActive(false);
            //StartCoroutine(TrailOffDelay(3f));
            return;
        }

        if (other.gameObject.TryGetComponent<Health>(out Health health))
        {
            // 피격 시 화살 비활성화
            Debug.Log("Hit!");
            health.TakeDamage(damage, false);
            gameObject.SetActive(false);
            //StartCoroutine(TrailOffDelay(3f));
            return;
        }
    }

    // 화살 사정거리(수정 예정)
    public void ArrowRange()
    {
        Vector3 screenPoint = camera.WorldToViewportPoint(transform.position);

        if(screenPoint.x < -1.0f || screenPoint.x > 1.0f || screenPoint.y < -1.0f || screenPoint.y > 1.0f)
        {
            gameObject.SetActive(false);
        }
    }

    // 화살 트레일 딜레이 후 제거
    IEnumerator TrailOffDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        trail.SetActive(false);
    }
    #endregion
}


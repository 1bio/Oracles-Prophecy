using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterParticleDamageHandler : MonoBehaviour
{
    private Monster _monster;
    private Health _playerHealth;

    private bool _canTakeDamage = true;
    [Header(" # 피격 시 파티클 비활성화")]
    [SerializeField] private bool _shutDownIfHitPlayer = false;
    [SerializeField] private bool _shutDownIfHitObstacle = false;

    [Header(" # 타겟 추적 여부")]
    [SerializeField] private bool _isFollowing = false;

    [Header(" # 피격 시 파티클 생성 여부")]
    [SerializeField] private bool _spawnVFXOnHit = false;
    [SerializeField] private GameObject _nextVFXPrefab;

    [SerializeField] private float _damageInterval = 1.5f;
    [SerializeField] private float _moveSpeed = 0;

    public void SetMonster(Monster monster)
    {
        _monster = monster;
    }

    private void Awake()
    {
        _playerHealth = GameObject.Find("Player").GetComponent<Health>();
    }

    private void Update()
    {
        ParticleSystem particleSystem = this.GetComponent<ParticleSystem>();
        if (particleSystem != null)
        {
            particleSystem.transform.position += particleSystem.transform.forward * _moveSpeed * Time.deltaTime;

            if (_isFollowing)
            {
                Vector3 direction = _playerHealth.gameObject.transform.position - transform.position;
                particleSystem.transform.rotation = Quaternion.LookRotation(direction);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        ProcessCollisionEffect(other.gameObject);
    }

    private void OnParticleCollision(GameObject other)
    {
        ProcessCollisionEffect(other);
    }

    private void ProcessCollisionEffect(GameObject other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(GameLayers.Player.ToString()) && _canTakeDamage)
        {
            if (_playerHealth != null)
            {
                StartCoroutine(DealDamageOverTime(_playerHealth));

                if (_shutDownIfHitPlayer)
                {
                    ParticleSystem particleSystem = this.gameObject.GetComponent<ParticleSystem>();
                    particleSystem.Stop();
                    particleSystem.Clear();
                    particleSystem.time = 0;

                    if (_nextVFXPrefab != null)
                        StartCoroutine(Explode(_nextVFXPrefab, particleSystem.transform.position, 3f));
                }
            }
        }
        else if (_shutDownIfHitObstacle && other.gameObject.layer == LayerMask.NameToLayer(GameLayers.Obstacle.ToString()))
        {
            ParticleSystem particleSystem = this.gameObject.GetComponent<ParticleSystem>();
            particleSystem.Stop();
            particleSystem.Clear();
            particleSystem.time = 0;

            if (_nextVFXPrefab != null)
                StartCoroutine(Explode(_nextVFXPrefab, particleSystem.transform.position, 3f));
        }
    }

    private IEnumerator DealDamageOverTime(Health health)
    {
        _canTakeDamage = false;

        health.TakeDamage(_monster.SkillController.CurrentSkillData.Damage, true);

        yield return new WaitForSeconds(_damageInterval);

        _canTakeDamage = true;
    }

    IEnumerator Explode(GameObject obj, Vector3 position, float delay)
    {
        GameObject explosion = Instantiate(obj, position, Quaternion.identity);
        yield return new WaitForSeconds(delay);
        Destroy(explosion);
    }
}

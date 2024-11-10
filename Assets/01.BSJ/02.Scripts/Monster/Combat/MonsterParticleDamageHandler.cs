using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterParticleDamageHandler : MonoBehaviour
{
    private Monster _monster;
    private Health _playerHealth;
    private ParticleSystem particleSystem;

    private bool _canTakeDamage = true;
    [Header(" # 피격 시 파티클 비활성화")]
    [SerializeField] private bool _shutDownIfHitPlayer = false;
    [SerializeField] private bool _shutDownIfHitObstacle = false;

    [Header(" # 타겟 추적 여부")]
    [SerializeField] private bool _isFollowing = false;

    [Header(" # 피격 시 파티클 생성 여부")]
    [SerializeField] private bool _spawnVFXOnHit = false;
    [SerializeField] private GameObject _nextVFXPrefab;

    [Header(" # 비활성화 타이머")]
    [SerializeField] private float _deactivationTime = 0;
    [SerializeField] private float _deactivationTimeVariance = 0;

    [Header(" # 대기 시간")]
    [SerializeField] private float _stayTime = 0;
    private float _currentTime = 0;

    [SerializeField] private float _damageInterval = 1.5f;
    [SerializeField] private float _moveSpeed = 0;
    [SerializeField] private float _rotationSpeed = 0;

    public void SetMonster(Monster monster)
    {
        _monster = monster;
    }

    private void Awake()
    {
        _playerHealth = GameObject.Find("Player").GetComponent<Health>();
        particleSystem = this.GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (particleSystem != null && particleSystem.isPlaying)
        {
            if (_currentTime <= _stayTime)
                _currentTime += Time.deltaTime;
            else
            {
                if (_isFollowing)
                {
                    Vector3 targetPosition = _playerHealth.gameObject.transform.position + new Vector3(0, 0.5f, 0);
                    Vector3 direction = targetPosition - particleSystem.transform.position;

                    Quaternion targetRotation = Quaternion.LookRotation(direction);
                    particleSystem.transform.rotation = Quaternion.Slerp(
                        particleSystem.transform.rotation,
                        targetRotation,
                        _rotationSpeed * Time.deltaTime
                    );
                }
                particleSystem.transform.position += particleSystem.transform.forward * _moveSpeed * Time.deltaTime;

                float randomDeactivationTime = Random.Range(Mathf.Max(_deactivationTime - _deactivationTimeVariance, 0), _deactivationTime + _deactivationTimeVariance);
                StartCoroutine(Deactivate(particleSystem, randomDeactivationTime));
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        ProcessCollisionEffect(other.gameObject);
    }

    private void OnParticleCollision(GameObject other)
    {
        ProcessCollisionEffect(other);
    }

    private void ProcessCollisionEffect(GameObject other)
    {
        if (_spawnVFXOnHit && _nextVFXPrefab != null && particleSystem.isPlaying)
            StartCoroutine(Explode(_nextVFXPrefab, particleSystem.transform.position, 3f));


        if (other.gameObject.layer == LayerMask.NameToLayer(GameLayers.Player.ToString()) && _canTakeDamage)
        {
            if (_playerHealth != null && particleSystem.isPlaying)
            {
                StartCoroutine(DealDamageOverTime(_playerHealth));

                if (_shutDownIfHitPlayer)
                {
                    particleSystem.Stop(true);
                    particleSystem.Clear();
                    particleSystem.time = 0;
                }
            }
        }
        else if (_shutDownIfHitObstacle && other.gameObject.layer == LayerMask.NameToLayer(GameLayers.Obstacle.ToString()))
        {
            particleSystem.Stop(true);
            particleSystem.Clear();
            particleSystem.time = 0;
        }
        else
        {
            particleSystem.Stop(true);
            particleSystem.Clear();
            particleSystem.time = 0;
        }

        _currentTime = 0f;
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

    IEnumerator Deactivate(ParticleSystem particleSystem, float delay)
    {
        if (delay <= 0f)
            yield break;

        yield return new WaitForSeconds(delay);

        if (_spawnVFXOnHit && _nextVFXPrefab != null && particleSystem.isPlaying)
            StartCoroutine(Explode(_nextVFXPrefab, particleSystem.transform.position, 3f));

        particleSystem.Stop(true);
        particleSystem.Clear();
        particleSystem.time = 0;

        _currentTime = 0f;
    }
}

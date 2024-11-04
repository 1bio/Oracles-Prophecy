using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterParticleDamageHandler : MonoBehaviour
{
    private Monster _monster;
    private Health _playerHealth;

    private bool _canTakeDamage = true;
    [SerializeField] private bool _shutDownIfHitPlayer = false;
    [SerializeField] private bool _shutDownIfHitObstacle = false;
    [SerializeField] private bool _isFollowing = false;
    [SerializeField] private float _damageInterval = 1.5f;
    [SerializeField] private float _moveSpeed = 0;

    public void SetMonster(Monster monster)
    {
        _monster = monster;
    }

    private void Awake()
    {
        _monster = GetComponentInParent<Monster>();
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
                }
            }
        }
        else if (_shutDownIfHitObstacle && other.gameObject.layer == LayerMask.NameToLayer(GameLayers.Obstacle.ToString()))
        {
            ParticleSystem particleSystem = this.gameObject.GetComponent<ParticleSystem>();
            particleSystem.Stop();
            particleSystem.Clear();
            particleSystem.time = 0;
        }
    }

    private IEnumerator DealDamageOverTime(Health health)
    {
        _canTakeDamage = false;

        health.TakeDamage(_monster.SkillController.CurrentSkillData.Damage, true);

        yield return new WaitForSeconds(_damageInterval);

        _canTakeDamage = true;
    }
}

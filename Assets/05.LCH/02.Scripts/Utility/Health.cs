using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Health : MonoBehaviour
{
    private float currentHealth;

    private bool isInvunerable;
    public bool isAlive { get; private set; } = true;
    private bool isDead = false;

    [SerializeField] private GameObject bloodVFX;
    [SerializeField] private PlayerHealthMonitor playerhealthMonitor;

    // 피격 카운트 필드
    public int hitCount;
    private int groggyCount = 3;

    private float currentTime;
    private float lastImpactTime;

    private float hitDurationCoolDown = 1f; // 1초 후에 hitCount 초기화

    // 이벤트 필드
    public event Action ImpactEvent;
    public event Action DeadEvent;

    private void Start()
    {
        lastImpactTime = hitDurationCoolDown;
    }

    private void Update()
    {
        CheckCoolDown();
    }

    #region Main Methods
    public void SetHealth(float currentHealth)
    {
        this.currentHealth = currentHealth;
    }

    public void SetInvulnerable(bool isInvunerable) // 무적 상태 적용
    {
        this.isInvunerable = isInvunerable;
    }

    private void CheckCoolDown() // 카운트 체크
    {
        currentTime = Time.time;
        
        if (currentTime - lastImpactTime >= hitDurationCoolDown)
        {
            hitCount = 0;
        }
    }

    public void TakeDamage(float damage, bool IsPlayer)
    {
        ImpactEvent?.Invoke();

        // 플레이어 체력 초기화
        if (IsPlayer)
        {
            if (isInvunerable)
                return;

            // 피격 횟수 로직
            float currentImpactTime = Time.time;
            lastImpactTime = currentImpactTime;
            hitCount++;

            float defends =  1 - (DataManager.instance.playerData.statusData.defense / 100f) > 0 ? DataManager.instance.playerData.statusData.defense / 100f : 0;

            currentHealth = Mathf.Max(currentHealth - (damage * (1 - defends)), 0);

            DataManager.instance.playerData.statusData.currentHealth = currentHealth;

            playerhealthMonitor.OnImpact();

            if (currentHealth <= 0)
            {
                Dead();
            }
        }
        else // 몬스터 체력 초기화
        {
            Monster monster = GetComponent<Monster>();

            MonsterHealthOnMouse.Monster = monster;
            MonsterHealthOnMouse.CurrentTime = 0;

            currentHealth = monster.CombatController.MonsterCombatAbility.MonsterHealth.CurrentHealth;

            isAlive = currentHealth > 0;
            if (isAlive)
            {
                currentHealth = Mathf.Max(currentHealth - damage, 0);
                monster.CombatController.MonsterCombatAbility.MonsterHealth.CurrentHealth = currentHealth;

                FloatingText(damage);
            }

            if (!monster.MovementController.TargetDetector.IsTargetDetected)
                monster.MovementController.TargetDetector.IsTargetDetected = true;
        }

        PlayBloodVFX();
    }

    private void PlayBloodVFX()
    {
        ParticleSystem[] bloods = bloodVFX.GetComponentsInChildren<ParticleSystem>();

        foreach (ParticleSystem blood in bloods)
        {
            blood.transform.position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);

            blood.Play();
        }
    }

    // 데미지 텍스트
    private void FloatingText(float damage)
    {
        GameObject damageText = PoolManager.instance.Get(1);

        if (damageText.GetComponentInChildren<TextMeshProUGUI>() != null)
        {
            damageText.GetComponentInChildren<TextMeshProUGUI>().text = Mathf.Floor(damage).ToString();

            RectTransform rectTransform = damageText.GetComponentInChildren<RectTransform>();

            float randomX = UnityEngine.Random.Range(transform.position.x - 1f, transform.position.x + 1f);
            float randomZ = UnityEngine.Random.Range(transform.position.z - 1f, transform.position.z + 1f);

            rectTransform.position = new Vector3(randomX, transform.position.y + 3f, randomZ); // Y축 보정
        }
    }

    // GameManager에서 이벤트 실행, 로비로 가기, UI 패널 열기 등 
    private void Dead()
    {
        if (isDead)
            return;

        if(currentHealth <= 0)
        {
            isDead = true;

            DeadEvent?.Invoke();
        }
    }

    // hitCount 확인
   /* private void Groggy()
    {
        // 피격 횟수 로직
        float currentImpactTime = Time.time;
        lastImpactTime = currentImpactTime;
        hitCount++;

        if (hitCount >= groggyCount)
        {
            GroggyEvent?.Invoke();

            hitCount = 0;
        }
    }*/
    #endregion
}

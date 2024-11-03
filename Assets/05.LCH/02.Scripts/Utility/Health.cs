using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Health : MonoBehaviour
{
    private float currentHealth;

    // 피격 카운트 필드
    private int hitCount;
    private int groggyCount = 3;

    private float currentTime;
    private float lastImpactTime;

    private float hitDurationCoolDown = 1f; // 1초 후에 hitCount 초기화

    private bool isInvunerable;

    // 이벤트 필드
    public event Action ImpactEvent;
    public event Action GroggyEvent;
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

        Groggy(); 

        if (currentHealth == 0)
        {
            Dead();
        }

        // 플레이어 체력 초기화
        if (IsPlayer)
        {
            if (isInvunerable)
                return;

            currentHealth = Mathf.Max(currentHealth - damage, 0);

            DataManager.instance.playerData.statusData.currentHealth = currentHealth;

            return;
        }
        else // 몬스터 체력 초기화
        {
            FloatingText(damage);

            Monster monster = GetComponent<Monster>();
            currentHealth = Mathf.Max(monster.CombatController.MonsterCombatAbility.MonsterHealth.CurrentHealth - damage, 0);
            monster.CombatController.MonsterCombatAbility.MonsterHealth.CurrentHealth = currentHealth;
            if (!monster.MovementController.TargetDetector.IsTargetDetected)
                monster.MovementController.TargetDetector.IsTargetDetected = true;
        }
    }

    public void Delay(float damage)
    {
        StartCoroutine(EventDelay(damage));
    }

    // 이벤트 딜레이(연속 피격 스킬용)
    IEnumerator EventDelay(float damage)
    {
        for (int i = 0; i < 5; i++)
        {
            TakeDamage(damage, false);
            yield return new WaitForSeconds(0.3f);
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

    // hitCount 확인
    private void Groggy()
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
    }

    private void Dead()
    {
        DeadEvent?.Invoke();

        // GameManager에서 이벤트 실행, 로비로 가기, UI 패널 열기 등 
    }
    #endregion
}

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FireBlade : MonoBehaviour
{
    private SkillData fireBlade;
    public TextMeshProUGUI[] fireBladeTexts;

    public Image icon_skill;
    public GameObject icon_lock;

    public int limitLevel;

    #region Initialized Methods
    public SkillData GetFireBladeData() // 화염칼 데이터
    {
        return DataManager.instance.playerData.skillData[4];
    }

    private void Awake()
    {
        fireBlade = GetFireBladeData();

        UpdateUI();
    }
    #endregion

    private void Update()
    {
        UpdateUI();
    }

    #region Main Methods
    public void FireBlade_LevelUp() // 버튼 이벤트
    {
        // 스킬 포인트가 없으면 반환
        if (DataManager.instance.playerData.statusData.skillPoint <= 0)
            return;

        if (DataManager.instance.playerData.statusData.level < limitLevel)
            return;

        // 스킬 잠금 해제
        if (fireBlade.level == 0)
        {
            // 스킬 아이콘 투명도 조절
            Color color = icon_skill.color;
            color.a = 0.8f;
            icon_skill.color = color;

            icon_lock.SetActive(false);
            fireBlade.isUnlock = false;

            Debug.Log("화염칼 얻음!");
        }

        DataManager.instance.SkillLevelUp("Fire Blade", 1);

        UIManager.instance.AddSkillSlot(4);
        SkillManager.instance.AddSkill(fireBlade.skillName, fireBlade.coolDown);
    }

    // 텍스트 업데이트
    public void UpdateUI()
    {
        //fireBladeTexts[0].text = fireBlade.skillName; // 스킬 이름
        //fireBladeTexts[1].text = $"레벨 {fireBlade.level}"; // 레벨
        //fireBladeTexts[2].text = $"쿨타임: {Mathf.Floor(fireBlade.coolDown)}초"; // 쿨타임
        //fireBladeTexts[3].text = fireBlade.description; // 설명
        //fireBladeTexts[4].text = $"공격력 +{Mathf.Floor((fireBlade.multipleDamage - 1) * 100)}%"; // 공격력 증가율 
        //fireBladeTexts[5].text = $"쿨타임 {Mathf.Floor((fireBlade.multipleCoolDown - 1) * 100)}%"; // 쿨타임 감소율

        if (fireBlade.level > 0)
        {
            fireBlade.isUnlock = false;
            icon_lock.SetActive(false);

            // 스킬 아이콘 투명도 조절
            Color color = icon_skill.color;
            color.a = 0.8f;
            icon_skill.color = color;
        }

        fireBladeTexts[0].text = $"{fireBlade.level}"; // 레벨
        fireBladeTexts[1].text = fireBlade.skillName; // 스킬 이름
        fireBladeTexts[2].text = fireBlade.description; // 설명
        fireBladeTexts[3].text = $"Damage {Mathf.Floor(fireBlade.minDamage)} - {Mathf.Floor(fireBlade.maxDamage)}"; // 공격력 증가율 
        fireBladeTexts[4].text = $"CoolDown {Mathf.Floor(fireBlade.coolDown)}"; // 쿨타임 감소율
        fireBladeTexts[5].text = $"Mana {Mathf.Floor(fireBlade.useMana)}";
    }
    #endregion
}

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkyFallShot : MonoBehaviour
{
    private SkillData skyfallShot;
    public TextMeshProUGUI[] skyfallShotTexts;

    public Image icon_skill;
    public GameObject icon_lock;

    public int limitLevel;

    #region Initialized Methods
    public SkillData GetSkyFallShotData() // 화염칼 데이터
    {
        return DataManager.instance.playerData.skillData[2];
    }

    private void Awake()
    {
        skyfallShot = GetSkyFallShotData();

        UpdateUI();
    }
    #endregion

    private void Update()
    {
        UpdateUI();
    }

    #region Main Methods
    public void SkyFallShot_LevelUp() // 버튼 이벤트
    {
        // 스킬 포인트가 없으면 반환
        if (DataManager.instance.playerData.statusData.skillPoint <= 0)
            return;

        if (DataManager.instance.playerData.statusData.level < limitLevel)
            return;

        // 스킬 잠금 해제
        if (skyfallShot.level == 0)
        {
            // 스킬 아이콘 투명도 조절
            Color color = icon_skill.color;
            color.a = 0.8f;
            icon_skill.color = color;

            icon_lock.SetActive(false);
            skyfallShot.isUnlock = false;

            Debug.Log("화살비 얻음!");
        }

        DataManager.instance.SkillLevelUp("SkyFallShot", 1);

        UIManager.instance.AddSkillSlot(2);
        SkillManager.instance.AddSkill(skyfallShot.skillName, skyfallShot.coolDown);
    }

    // 텍스트 업데이트
    private void UpdateUI()
    {
        if (skyfallShot.level > 0)
        {
            icon_lock.SetActive(false);

            // 스킬 아이콘 투명도 조절
            Color color = icon_skill.color;
            color.a = 0.8f;
            icon_skill.color = color;
        }

        skyfallShotTexts[0].text = $"{skyfallShot.level}"; // 레벨
        skyfallShotTexts[1].text = skyfallShot.skillName; // 스킬 이름
        skyfallShotTexts[2].text = skyfallShot.description; // 설명
        skyfallShotTexts[3].text = $"Damage {Mathf.Floor(skyfallShot.minDamage)} - {Mathf.Floor(skyfallShot.maxDamage)}"; // 공격력 증가율 
        skyfallShotTexts[4].text = $"CoolDown {Mathf.Floor(skyfallShot.coolDown)}"; // 쿨타임 감소율
        skyfallShotTexts[5].text = $"Mana {Mathf.Floor(skyfallShot.useMana)}";
    }
    #endregion
}

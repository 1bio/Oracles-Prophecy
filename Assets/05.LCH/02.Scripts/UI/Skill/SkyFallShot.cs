using TMPro;
using UnityEngine;

public class SkyFallShot : MonoBehaviour
{
    private SkillData skyfallShot;
    public TextMeshProUGUI[] skyfallShotTexts;

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


    #region Main Methods
    public void SkyFallShot_LevelUp() // 버튼 이벤트
    {
        // 스킬 잠금 해제
        if (skyfallShot.level == 0)
        {
            skyfallShot.isUnlock = false;
            SkillManager.instance.AddSkill(skyfallShot.skillName, skyfallShot.coolDown);
            UIManager.instance.AddSkillSlot(2);
            Debug.Log("화살비 얻음!");
        }

        DataManager.instance.SkillLevelUp("화살비", 1);

        UpdateUI();
    }

    // 텍스트 업데이트
    private void UpdateUI()
    {
        skyfallShotTexts[0].text = $"{skyfallShot.level}"; // 레벨
        skyfallShotTexts[1].text = skyfallShot.skillName; // 스킬 이름
        skyfallShotTexts[2].text = skyfallShot.description; // 설명
        skyfallShotTexts[3].text = $"Damage {Mathf.Floor(skyfallShot.minDamage)} - {Mathf.Floor(skyfallShot.maxDamage)}"; // 공격력 증가율 
        skyfallShotTexts[4].text = $"CoolDown {Mathf.Floor(skyfallShot.coolDown)}"; // 쿨타임 감소율
        skyfallShotTexts[5].text = $"Mana: {Mathf.Floor(skyfallShot.useMana)}";
    }
    #endregion
}

using TMPro;
using UnityEngine;

public class Slash : MonoBehaviour
{
    private SkillData slash;
    public TextMeshProUGUI[] slashTexts;

    #region Initialized Methods
    public SkillData GetDashSlashData() // 절단 데이터
    {
        return DataManager.instance.playerData.skillData[3];
    }

    private void Awake()
    {
        slash = GetDashSlashData();

        UpdateUI();
    }
    #endregion


    #region Main Methods
    public void DashSlash_LevelUp() // 버튼 이벤트
    {
        // 스킬 잠금 해제
        if (slash.level == 0)
        {
            slash.isUnlock = false;
            SkillManager.instance.AddSkill(slash.skillName, slash.coolDown);
            UIManager.instance.AddSkillSlot(3);
            Debug.Log("절단 얻음!");
        }

        DataManager.instance.SkillLevelUp("절단", 1);
        UpdateUI();
    }

    // 텍스트 업데이트
    private void UpdateUI()
    {
        //dashSlashTexts[0].text = dashSlash.skillName; // 스킬 이름
        //dashSlashTexts[1].text = $"레벨 {dashSlash.level}"; // 레벨
        //dashSlashTexts[2].text = $"쿨타임: {Mathf.Floor(dashSlash.coolDown)}초"; // 쿨타임
        //dashSlashTexts[3].text = dashSlash.description; // 설명
        //dashSlashTexts[4].text = $"공격력 +{Mathf.Floor((dashSlash.multipleDamage - 1) * 100)}%"; // 공격력 증가율 
        //dashSlashTexts[5].text = $"쿨타임 {Mathf.Floor((dashSlash.multipleCoolDown - 1) * 100)}%"; // 쿨타임 감소율

        slashTexts[0].text = $"{slash.level}"; // 레벨
        slashTexts[1].text = slash.skillName; // 스킬 이름
        slashTexts[2].text = slash.description; // 설명
        slashTexts[3].text = $"Damage {Mathf.Floor(slash.minDamage)} - {Mathf.Floor(slash.maxDamage)}"; // 공격력 증가율 
        slashTexts[4].text = $"CoolDown {Mathf.Floor(slash.coolDown)}"; // 쿨타임 감소율
        slashTexts[5].text = $"Mana: {Mathf.Floor(slash.useMana)}";
    }
    #endregion
}

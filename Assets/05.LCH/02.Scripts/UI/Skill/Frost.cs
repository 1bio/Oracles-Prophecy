using TMPro;
using UnityEngine;

public class Frost : MonoBehaviour
{
    private SkillData frost;
    public TextMeshProUGUI[] frostTexts;

    #region Initialized Methods
    public SkillData GetFrostData() // 빙결 데이터
    {
        return DataManager.instance.playerData.skillData[5];
    }

    private void Awake()
    {
        frost = GetFrostData();

        UpdateUI();
    }
    #endregion


    #region Main Methods
    public void Frost_LevelUp() // 버튼 이벤트
    {
        // 스킬 잠금 해제
        if (frost.level == 0)
        {
            frost.isUnlock = false;
            SkillManager.instance.AddSkill(frost.skillName, frost.coolDown);
            UIManager.instance.AddSkillSlot(5);
            Debug.Log("빙결 얻음!");
        }

        DataManager.instance.SkillLevelUp("빙결", 1);
        UpdateUI(); // 차징샷 UI
    }

    // 텍스트 업데이트
    private void UpdateUI()
    {
        //spinSlashTexts[0].text = frost.skillName; // 스킬 이름
        //spinSlashTexts[1].text = $"레벨 {frost.level}"; // 레벨
        //spinSlashTexts[2].text = $"쿨타임: {Mathf.Floor(frost.coolDown)}초"; // 쿨타임
        //spinSlashTexts[3].text = frost.description; // 설명
        //spinSlashTexts[4].text = $"공격력 +{Mathf.Floor((frost.multipleDamage - 1) * 100)}%"; // 공격력 증가율 
        //spinSlashTexts[5].text = $"쿨타임 {Mathf.Floor((frost.multipleCoolDown - 1) * 100)}%"; // 쿨타임 감소율

        frostTexts[0].text = $"{frost.level}"; // 레벨
        frostTexts[1].text = frost.skillName; // 스킬 이름
        frostTexts[2].text = frost.description; // 설명
        frostTexts[3].text = $"Damage {Mathf.Floor(frost.minDamage)} - {Mathf.Floor(frost.maxDamage)}"; // 공격력 증가율 
        frostTexts[4].text = $"CoolDown {Mathf.Floor(frost.coolDown)}"; // 쿨타임 감소율
        frostTexts[5].text = $"Mana: {Mathf.Floor(frost.useMana)}";
    }
    #endregion
}

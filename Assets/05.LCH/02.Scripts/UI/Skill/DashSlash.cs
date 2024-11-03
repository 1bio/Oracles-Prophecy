using TMPro;
using UnityEngine;

public class DashSlash : MonoBehaviour
{
    private SkillData dashSlash;
    public TextMeshProUGUI[] dashSlashTexts;

    #region Initialized Methods
    public SkillData GetDashSlashData() // 도약베기 데이터
    {
        return DataManager.instance.playerData.skillData[3];
    }

    private void Awake()
    {
        dashSlash = GetDashSlashData();

        UpdateUI();
    }
    #endregion


    #region Main Methods
    public void DashSlash_LevelUp() // 버튼 이벤트
    {
        // 스킬 잠금 해제
        if (dashSlash.level == 0)
        {
            dashSlash.isUnlock = false;
            SkillManager.instance.AddSkill(dashSlash.skillName, dashSlash.coolDown);
            UIManager.instance.AddSkillSlot(3);
            Debug.Log("도약베기 얻음!");
        }

        DataManager.instance.SkillLevelUp("도약베기", 1);
        UIManager.instance.SelectWindow(false);

        UpdateUI(); 
    }

    // 텍스트 업데이트
    private void UpdateUI()
    {
        dashSlashTexts[0].text = dashSlash.skillName; // 스킬 이름
        dashSlashTexts[1].text = $"레벨 {dashSlash.level}"; // 레벨
        dashSlashTexts[2].text = $"쿨타임: {Mathf.Floor(dashSlash.coolDown)}초"; // 쿨타임
        dashSlashTexts[3].text = dashSlash.description; // 설명
        dashSlashTexts[4].text = $"공격력 +{Mathf.Floor((dashSlash.multipleDamage - 1) * 100)}%"; // 공격력 증가율 
        dashSlashTexts[5].text = $"쿨타임 {Mathf.Floor((dashSlash.multipleCoolDown - 1) * 100)}%"; // 쿨타임 감소율
    }
    #endregion
}

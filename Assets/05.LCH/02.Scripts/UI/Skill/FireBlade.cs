using TMPro;
using UnityEngine;

public class FireBlade : MonoBehaviour
{
    private SkillData fireBlade;
    public TextMeshProUGUI[] fireBladeTexts;

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


    #region Main Methods
    public void FireBlade_LevelUp() // 버튼 이벤트
    {
        // 스킬 잠금 해제
        if (fireBlade.level == 0)
        {
            fireBlade.isUnlock = false;
            SkillManager.instance.AddSkill(fireBlade.skillName, fireBlade.coolDown);
            UIManager.instance.AddSkillSlot(4);
            Debug.Log("화염칼 얻음!");
        }

        DataManager.instance.SkillLevelUp("화염칼", 1);
        UIManager.instance.SelectWindow(false);

        UpdateUI();
    }

    // 텍스트 업데이트
    public void UpdateUI()
    {
        fireBladeTexts[0].text = fireBlade.skillName; // 스킬 이름
        fireBladeTexts[1].text = $"레벨 {fireBlade.level}"; // 레벨
        fireBladeTexts[2].text = $"쿨타임: {Mathf.Floor(fireBlade.coolDown)}초"; // 쿨타임
        fireBladeTexts[3].text = fireBlade.description; // 설명
        fireBladeTexts[4].text = $"공격력 +{Mathf.Floor((fireBlade.multipleDamage - 1) * 100)}%"; // 공격력 증가율 
        fireBladeTexts[5].text = $"쿨타임 {Mathf.Floor((fireBlade.multipleCoolDown - 1) * 100)}%"; // 쿨타임 감소율
    }
    #endregion
}

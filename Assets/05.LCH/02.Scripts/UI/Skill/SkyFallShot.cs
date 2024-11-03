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
        UIManager.instance.SelectWindow(false);

        UpdateUI();
    }

    // 텍스트 업데이트
    private void UpdateUI()
    {
        skyfallShotTexts[0].text = skyfallShot.skillName; // 스킬 이름
        skyfallShotTexts[1].text = $"레벨 {skyfallShot.level}"; // 레벨
        skyfallShotTexts[2].text = $"쿨타임: {Mathf.Floor(skyfallShot.coolDown)}초"; // 쿨타임
        skyfallShotTexts[3].text = skyfallShot.description; // 설명
        skyfallShotTexts[4].text = $"공격력 +{Mathf.Floor((skyfallShot.multipleDamage - 1) * 100)}%"; // 공격력 증가율 
        skyfallShotTexts[5].text = $"쿨타임 {Mathf.Floor((skyfallShot.multipleCoolDown - 1) * 100)}%"; // 쿨타임 감소율
    }
    #endregion
}

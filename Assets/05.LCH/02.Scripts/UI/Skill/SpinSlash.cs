using TMPro;
using UnityEngine;

public class SpinSlash : MonoBehaviour
{
    private SkillData spinSlash;
    public TextMeshProUGUI[] spinSlashTexts;

    #region Initialized Methods
    public SkillData GetSpinSlashData() // 회전베기 데이터
    {
        return DataManager.instance.playerData.skillData[5];
    }

    private void Awake()
    {
        spinSlash = GetSpinSlashData();

        UpdateUI();
    }
    #endregion


    #region Main Methods
    public void SpinSlash_LevelUp() // 버튼 이벤트
    {
        // 스킬 잠금 해제
        if (spinSlash.level == 0)
        {
            spinSlash.isUnlock = false;
            SkillManager.instance.AddSkill(spinSlash.skillName, spinSlash.coolDown);
            UIManager.instance.AddSkillSlot(5);
            Debug.Log("회전베기 얻음!");
        }

        DataManager.instance.SkillLevelUp("회전베기", 1);
        UIManager.instance.SelectWindow(false);

        UpdateUI(); // 차징샷 UI
    }

    // 텍스트 업데이트
    private void UpdateUI()
    {
        spinSlashTexts[0].text = spinSlash.skillName; // 스킬 이름
        spinSlashTexts[1].text = $"레벨 {spinSlash.level}"; // 레벨
        spinSlashTexts[2].text = $"쿨타임: {Mathf.Floor(spinSlash.coolDown)}초"; // 쿨타임
        spinSlashTexts[3].text = spinSlash.description; // 설명
        spinSlashTexts[4].text = $"공격력 +{Mathf.Floor((spinSlash.multipleDamage - 1) * 100)}%"; // 공격력 증가율 
        spinSlashTexts[5].text = $"쿨타임 {Mathf.Floor((spinSlash.multipleCoolDown - 1) * 100)}%"; // 쿨타임 감소율
    }
    #endregion
}

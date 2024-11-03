using TMPro;
using UnityEngine;

public class ChargingShot : MonoBehaviour
{
    private SkillData chargingShot;
    public TextMeshProUGUI[] chargingShotTexts; 

    #region Initialized Methods
    public SkillData GetChargingShotData() // 차징샷 데이터
    {
        return DataManager.instance.playerData.skillData[0];
    }

    private void Awake()
    {
        chargingShot = GetChargingShotData();

        UpdateUI();
    }
    #endregion


    #region Main Methods
    public void ChargingShot_LevelUp() // 버튼 이벤트
    {
        // 스킬 잠금 해제
        if (chargingShot.level == 0)
        {
            chargingShot.isUnlock = false;
            SkillManager.instance.AddSkill(chargingShot.skillName, chargingShot.coolDown);
            UIManager.instance.AddSkillSlot(0);
            Debug.Log("정조준 얻음!");
        }

        DataManager.instance.SkillLevelUp("정조준", 1);
        UIManager.instance.SelectWindow(false);

        UpdateUI(); 
    }

    // 텍스트 업데이트
    private void UpdateUI()
    {
        chargingShotTexts[0].text = chargingShot.skillName; // 스킬 이름
        chargingShotTexts[1].text = $"레벨 {chargingShot.level}"; // 레벨
        chargingShotTexts[2].text = $"쿨타임: {Mathf.Floor(chargingShot.coolDown)}초"; // 쿨타임
        chargingShotTexts[3].text = chargingShot.description; // 설명
        chargingShotTexts[4].text = $"공격력 +{Mathf.Floor((chargingShot.multipleDamage - 1) * 100)}%"; // 공격력 증가율 
        chargingShotTexts[5].text = $"쿨타임 {Mathf.Floor((chargingShot.multipleCoolDown - 1) * 100)}%"; // 쿨타임 감소율
    }
    #endregion
}

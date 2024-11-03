using TMPro;
using UnityEngine;

public class TripleShot : MonoBehaviour
{
    private SkillData tripleShot;
    public TextMeshProUGUI[] tripleShotTexts; 

    #region Initialized Methods
    public SkillData GetTripleShotData() // 트리플샷 데이터
    {
        return DataManager.instance.playerData.skillData[1];
    }

    private void Awake()
    {
        tripleShot = GetTripleShotData();

        UpdateUI();
    }
    #endregion


    #region Main Methods
    public void TripleShot_LevelUp() // 버튼 이벤트
    {
        // 스킬 잠금 해제
        if (tripleShot.level == 0)
        {
            tripleShot.isUnlock = false;
            SkillManager.instance.AddSkill(tripleShot.skillName, tripleShot.coolDown);
            UIManager.instance.AddSkillSlot(1);
            Debug.Log("트리플샷 얻음!");
        }

        DataManager.instance.SkillLevelUp("트리플샷", 1);
        UIManager.instance.SelectWindow(false);

        UpdateUI(); 
    }

    // 텍스트 업데이트
    private void UpdateUI()
    {
        tripleShotTexts[0].text = tripleShot.skillName; // 스킬 이름
        tripleShotTexts[1].text = $"레벨 {tripleShot.level}"; // 레벨
        tripleShotTexts[2].text = $"쿨타임: {Mathf.Floor(tripleShot.coolDown)}초"; // 쿨타임
        tripleShotTexts[3].text = tripleShot.description; // 설명
        tripleShotTexts[4].text = $"공격력 +{Mathf.Floor((tripleShot.multipleDamage - 1) * 100)}%"; // 공격력 증가율 
        tripleShotTexts[5].text = $"쿨타임 {Mathf.Floor((tripleShot.multipleCoolDown - 1) * 100)}%"; // 쿨타임 감소율
    }
    #endregion
}

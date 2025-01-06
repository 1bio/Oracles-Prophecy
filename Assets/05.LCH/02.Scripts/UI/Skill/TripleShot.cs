using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TripleShot : MonoBehaviour
{
    private SkillData tripleShot;
    public TextMeshProUGUI[] tripleShotTexts;

    public Image icon_skill;
    public GameObject icon_lock;

    public int limitLevel;

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

    private void Update()
    {
        UpdateUI();
    }

    #region Main Methods
    public void TripleShot_LevelUp() // 버튼 이벤트
    {
        // 스킬 포인트가 없으면 반환
        if (DataManager.instance.playerData.statusData.skillPoint <= 0)
            return;

        if (DataManager.instance.playerData.statusData.level < limitLevel)
            return;

        // 스킬 잠금 해제
        if (tripleShot.level == 0)
        {
            SkillManager.instance.AddSkill(tripleShot.skillName, tripleShot.coolDown);

            // 스킬 아이콘 투명도 조절
            Color color = icon_skill.color;
            color.a = 0.8f;
            icon_skill.color = color;

            icon_lock.SetActive(false);
            tripleShot.isUnlock = false;

            Debug.Log("트리플샷 얻음!");

        }

        DataManager.instance.SkillLevelUp("TripleShot");

        UIManager.instance.AddSkillSlot(1);
    }

    // 텍스트 업데이트
    private void UpdateUI()
    {
        if (tripleShot.level > 0)
        {
            icon_lock.SetActive(false);

            // 스킬 아이콘 투명도 조절
            Color color = icon_skill.color;
            color.a = 0.8f;
            icon_skill.color = color;
        }

        tripleShotTexts[0].text = $"{tripleShot.level}"; // 레벨
        tripleShotTexts[1].text = tripleShot.skillName; // 스킬 이름
        tripleShotTexts[2].text = tripleShot.description; // 설명
        tripleShotTexts[3].text = $"Damage {Mathf.Floor(tripleShot.minDamage)} - {Mathf.Floor(tripleShot.maxDamage)}"; // 공격력 증가율 
        tripleShotTexts[4].text = $"CoolDown {Mathf.Floor(tripleShot.coolDown)}"; // 쿨타임 감소율
        tripleShotTexts[5].text = $"Mana {Mathf.Floor(tripleShot.useMana)}";
    }
    #endregion
}

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Frost : MonoBehaviour
{
    private SkillData frost;
    public TextMeshProUGUI[] frostTexts;

    public Image icon_skill;
    public GameObject icon_lock;

    public int limitLevel;


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


    private void Update()
    {
        UpdateUI(); // 차징샷 UI
    }

    #region Main Methods
    public void Frost_LevelUp() // 버튼 이벤트
    {
        // 스킬 포인트가 없으면 반환
        if (DataManager.instance.playerData.statusData.skillPoint <= 0)
            return;

        if (DataManager.instance.playerData.statusData.level < limitLevel)
            return;

        // 스킬 잠금 해제
        if (frost.level == 0)
        {
            // 스킬 아이콘 투명도 조절
            Color color = icon_skill.color;
            color.a = 0.8f;
            icon_skill.color = color;

            icon_lock.SetActive(false);
            frost.isUnlock = false;
           
            SkillManager.instance.AddSkill(frost.skillName, frost.coolDown);
            UIManager.instance.AddSkillSlot(5);
            Debug.Log("빙결 얻음!");
        }

        DataManager.instance.SkillLevelUp("빙결", 1);
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

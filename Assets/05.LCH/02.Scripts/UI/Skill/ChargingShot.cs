using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChargingShot : MonoBehaviour
{
    private SkillData chargingShot;
    public TextMeshProUGUI[] chargingShotTexts;

    public Image icon_skill;
    public GameObject icon_lock;

    public int limitLevel;

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

    private void Update()
    {
        UpdateUI();
    }

    #region Main Methods
    public void ChargingShot_LevelUp() // 버튼 이벤트
    {
        // 스킬 포인트가 없으면 반환
        if (DataManager.instance.playerData.statusData.skillPoint <= 0)
            return;

        if (DataManager.instance.playerData.statusData.level < limitLevel)
            return;

        // 스킬 잠금 해제
        if (chargingShot.level == 0)
        {
            // 스킬 아이콘 투명도 조절
            Color color = icon_skill.color;
            color.a = 0.8f;
            icon_skill.color = color;

            icon_lock.SetActive(false);
            chargingShot.isUnlock = false;

            Debug.Log("정조준 얻음!");
        }

        DataManager.instance.SkillLevelUp("ChargingShot");

        UIManager.instance.AddSkillSlot(0);
        SkillManager.instance.AddSkill(chargingShot.skillName, chargingShot.coolDown);
    }

    // 텍스트 업데이트
    private void UpdateUI()
    {
        if (chargingShot.level > 0)
        {
            icon_lock.SetActive(false);

            // 스킬 아이콘 투명도 조절
            Color color = icon_skill.color;
            color.a = 0.8f;
            icon_skill.color = color;
        }

        chargingShotTexts[0].text = $"{chargingShot.level}"; // 레벨
        chargingShotTexts[1].text = chargingShot.skillName; // 스킬 이름
        chargingShotTexts[2].text = chargingShot.description; // 설명
        chargingShotTexts[3].text = $"Damage {Mathf.Floor(chargingShot.minDamage)} - {Mathf.Floor(chargingShot.maxDamage)}"; // 공격력 증가율 
        chargingShotTexts[4].text = $"CoolDown {Mathf.Floor(chargingShot.coolDown)}"; // 쿨타임 감소율
        chargingShotTexts[5].text = $"Mana {Mathf.Floor(chargingShot.useMana)}";
    }
    #endregion
}

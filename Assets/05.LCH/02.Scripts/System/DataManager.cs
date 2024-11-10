using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    // <==== Data ====>
    public PlayerData playerData;

    private StatusData statusData;
    private List<SkillData> skillData;
    private List<AttackData> attackData;
    private RangeAttackData rangeAttackData;

    private float limitExp = 30f; // 경험치 제한(레벨 업 시 경험치 증가)
    

    // 데이터 초기화
    #region Initialized Methods
    private void Initialized()
    {
        statusData = new StatusData(1, 0f, 5f, 100f, 100f, 0f);

        skillData = new List<SkillData>
        {
            // 원거리
            new SkillData("정조준", 0, 5f, 10f, 5f, 0, 0, true, "1초 동안 힘을 모아 \r\n강력한 화살을 발사합니다", 1.15f, 0.9f, 0f, false, 20f), // 스킬 1 [0]
            new SkillData("트리플샷", 0, 5f, 10f, 5f, 0, 0, true, "기본 공격 시 10초 동안 3발의 \r\n화살을 연속으로 발사합니다", 1.1f, 0.95f, 5f, true, 20f), // 스킬 2 [1]
            new SkillData("화살비", 0, 5f, 10f, 5f, 0, 0, true, "하늘에 떨어지는 화살을 \r\n여러 번 발사합니다", 1.05f, 0.95f, 0f, false, 20f), // 스킬 3 [2]

            // 근거리
            new SkillData("절단", 0,  5f, 10f, 6f, 0, 0, true, "짧게 도약하며 \r\n근처의 적들을 공격합니다", 1.07f, 0.85f, 0f, false, 20f), // 스킬 1 [3]
            new SkillData("화염칼", 0,  5f, 10f, 10f, 5f, 0.35f, true, "12초동안 근거리 무기를 \r\n불로 강화하여 공격합니다", 1.13f, 0.95f, 5f, true, 20f), // 스킬 2 [4]
            new SkillData("빙결", 0,  5f, 10f, 5f, 0, 0, true, "강력한 빙결의 힘을 방출합니다", 1.10f, 0.9f, 0f, false, 20f), // 스킬 3 [5]
        };
        
        attackData = new List<AttackData>()
        {
            new AttackData("Attack1@Melee", 1, 0.6f, 0.1f, 0.25f, 0.05f, 10f, 0f), // 근거리 공격 1
            new AttackData("Attack2@Melee", 2, 0.5f, 0.1f, 0.25f, 0.05f, 10f, 0f), // 근거리 공격 2
            new AttackData("Attack3@Melee", -1, 0, 0.1f, 0.25f, 0.05f, 20f, 0f), // 근거리 공격 3

            new AttackData("HeavyAttack1@Melee", 4, 0.6f, 0.1f, 0.25f, 0.35f, 20f, 0f), // 근거리 패시브 공격 1
            new AttackData("HeavyAttack2@Melee", 5, 0.5f, 0.1f, 0.25f, 0.35f, 20f, 0f), // 근거리 패시브 공격 2
            new AttackData("HeavyAttack3@Melee", -1, 0, 0.1f, 0.25f, 0.35f, 35f, 0f) // 근거리 패시브 공격 3
        };

        rangeAttackData = new RangeAttackData("None", 0, 0f, 0f, 0f, 0f, 10f, 0.3f); // 원거리 공격

        playerData = new PlayerData(statusData, skillData, attackData, rangeAttackData);
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        Initialized();
    }
    #endregion


    #region Main Methods
    // 경험치 증가
    public void ExpUp(Monster monster)
    {
        switch (monster)
        {
            case Troll:
                playerData.statusData.exp += Random.Range(1f, 7f);
                break;
            case Minotaur:
                playerData.statusData.exp += Random.Range(2f, 5f);
                break;
            case Medusa:
                playerData.statusData.exp += Random.Range(10f, 20f);
                break;
        }

        if (playerData.statusData.exp >= limitExp)
        {
            LevelUp(25f, 50f);

            limitExp += 10f;
        }

    }

    // 경험치 레벨업
    public void LevelUp(float addHealth, float addMana) 
    {
        StatusData player = playerData.statusData;
        
        player.level += 1;
        player.exp = 0f;

        // 추가 체력만큼 최대 체력 및 마나 증가
        player.maxHealth += addHealth;
        player.maxMana += addMana;

        // 플레이어 레벨 업 후 체력 및 마나 회복
        player.currentHealth = player.maxHealth; 
        player.currentMana = player.maxMana; 

    }

    // 스킬 레벨 업
    public void SkillLevelUp(string skillName, int level)
    {
        foreach (SkillData skill in playerData.skillData)
        {
            if (skill.skillName != skillName)
                continue;

            skill.level += level;
            //skill.damage *= skill.multipleDamage;
            skill.coolDown *= skill.multipleCoolDown;
            break; 
        }
    }

    // 스킬 마나 사용
    public void UseMana(string skillName)
    {
        foreach (SkillData skill in playerData.skillData)
        {
            if (skill.skillName != skillName)
                continue;

            if (skill.useMana > statusData.currentMana)
                return;

            statusData.currentMana -= skill.useMana;
        }
    }
    #endregion


    #region Save & Load
    // Data -> Json
    public void SaveData()
    {
        string json = JsonUtility.ToJson(playerData, true);
        string filePath = Path.Combine(Application.dataPath, "Resources", "playerData.json");

        File.WriteAllText(filePath, json);
    }

    // Json -> Data
    public void LoadData()
    {
        string filePath = Path.Combine(Application.dataPath, "Resources", "playerData.json");

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            playerData = JsonUtility.FromJson<PlayerData>(json);
        }
        else
        {
            Debug.LogWarning("저장된 게임 데이터가 없습니다");
        }
    }
    #endregion
}

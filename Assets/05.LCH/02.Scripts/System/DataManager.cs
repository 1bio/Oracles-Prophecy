using MedievalKingdomUI.Scripts.Window;
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
    public void Initialized()
    {
        statusData = new StatusData(1, 0f, 5f, 100f, 100f, 8f, 15f, 0f);

        skillData = new List<SkillData>
        {
            // 원거리
            new SkillData("ChargingShot", 0, 20f, 40f, 0f, 12f, 0, 0, true, "Fires a powerful arrow after \r\ngathering strength for one second", 1.15f, 0.9f, 0f, false, 15f), // 스킬 1 [0]
            new SkillData("TripleShot", 0, 10f, 20f, 0f , 9f, 0, 0, true, "Fires three arrows in quick \r\nsuccession with a basic attack", 1.1f, 0.95f, 8f, true, 10f), // 스킬 2 [1]
            new SkillData("SkyFallShot", 0, 10f, 20f ,0f, 18f, 0, 0, true, "Fires multiple arrows that \r\nfall from the sky", 1.05f, 0.95f, 0f, false, 25f), // 스킬 3 [2]

            // 근거리
            new SkillData("Single Slash", 0, 30f, 50f, 10f, 14f, 0, 0, true, "Performs a powerful attack \r\nover a wide area", 1.07f, 0.85f, 0f, false, 15f), // 스킬 1 [3]
            new SkillData("Fire Blade", 0, 20f, 40f, 0f, 10f, 5f, 0.35f, true, "Enhances a melee weapon with \r\nfire to perform an attack", 1.13f, 0.95f, 8f, true, 10f), // 스킬 2 [4]
            new SkillData("Frost", 0, 40f, 60f, 10f, 16f, 0, 0, true, "Instantly releases the power of frost", 1.10f, 0.9f, 0f, false, 25f), // 스킬 3 [5]
        };
        
        attackData = new List<AttackData>()
        {
            new AttackData("Attack1@Melee", 1, 0.6f, 0.1f, 0.25f, 0.05f, 0f), // 근거리 공격 1
            new AttackData("Attack2@Melee", 2, 0.5f, 0.1f, 0.25f, 0.05f, 0f), // 근거리 공격 2
            new AttackData("Attack3@Melee", -1, 0, 0.1f, 0.25f, 0.05f, 0f), // 근거리 공격 3

            new AttackData("HeavyAttack1@Melee", 4, 0.6f, 0.1f, 0.25f, 0.35f, 0f), // 근거리 패시브 공격 1
            new AttackData("HeavyAttack2@Melee", 5, 0.5f, 0.1f, 0.25f, 0.35f, 0f), // 근거리 패시브 공격 2
            new AttackData("HeavyAttack3@Melee", -1, 0, 0.1f, 0.25f, 0.35f, 0f) // 근거리 패시브 공격 3
        };

        rangeAttackData = new RangeAttackData("None", 0, 0f, 0f, 0f, 0f, 0.3f); // 원거리 공격

        playerData = new PlayerData(statusData, skillData, attackData, rangeAttackData);
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);
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
                playerData.statusData.exp += Random.Range(15f, 25f);
                break;
            case Minotaur:
                playerData.statusData.exp += Random.Range(20f, 30f);
                break;
            case Medusa:
                playerData.statusData.exp += Random.Range(35f, 50f);
                break;
            case CobraSnake:
                playerData.statusData.exp += Random.Range(5f, 15f);
                break;
            case Devils:
                playerData.statusData.exp += Random.Range(20f, 30f);
                break;
            case Weeper:
                playerData.statusData.exp += Random.Range(5f, 15f);
                break;
            case SerpentWarrior:
                playerData.statusData.exp += Random.Range(8f, 15f);
                break;

        }

        // 레벨 업
        if (playerData.statusData.exp >= limitExp)
        {
            LevelUp(25f, 50f, 3f, 3f);

            limitExp += 15f;
        }
    }

    // 레벨업
    public void LevelUp(float addHealth, float addMana, float addMinDamage, float addMaxDamage) 
    {
        StatusData player = playerData.statusData;
        
        player.level += 1;
        player.exp = 0f;

        // 추가 체력만큼 최대 체력 및 마나 증가
        player.maxHealth += addHealth;
        player.maxMana += addMana;
        player.minDamage += addMinDamage;
        player.maxDamage += addMaxDamage;

        playerData.statusData.skillPoint += 1;
    }

    // 스킬 레벨 업
    public void SkillLevelUp(string skillName, int level)
    {
        foreach (SkillData skill in playerData.skillData)
        {
            if (skill.skillName != skillName)
                continue;

            skill.level += level;
            skill.minDamage += 3;
            skill.maxDamage += 3;
            skill.coolDown *= skill.multipleCoolDown;

            playerData.statusData.skillPoint -= 1;
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

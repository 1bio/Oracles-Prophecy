using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSkillController
{
    public MonsterSkillController(MonsterSkillData[] monsterSkillDatas)
    {
        MonsterSkillDatas = new MonsterSkillData[monsterSkillDatas.Length];
        for (int i = 0; i < monsterSkillDatas.Length; i++)
        {
            MonsterSkillDatas[i] = monsterSkillDatas[i].CreateInstance();
        }
    }

    public MonsterSkillData[] MonsterSkillDatas {  get; private set; }
    public List<MonsterSkillData> AvailableSkills { get; private set; }
    public MonsterSkillData CurrentSkillData { get; set; }

    public List<MonsterSkillData> GetAvailableSkills()
    {
        AvailableSkills = new List<MonsterSkillData>();
        for (int i = 0; i < MonsterSkillDatas.Length; i++)
        {
            if (MonsterSkillDatas[i].CooldownTimer >= MonsterSkillDatas[i].CooldownThreshold)
                AvailableSkills.Add(MonsterSkillDatas[i]);
        }
        return AvailableSkills;
    }

    public MonsterSkillData UpdateCurrentSkillData()
    {
        CurrentSkillData = AvailableSkills[Random.Range(0, AvailableSkills.Count)];
        return CurrentSkillData;
    }

    public void UpdateCooldowns()
    {
        for (int i = 0; i < MonsterSkillDatas.Length; i++)
        {
            MonsterSkillDatas[i].CooldownTimer += Time.deltaTime;
        }
    }
}

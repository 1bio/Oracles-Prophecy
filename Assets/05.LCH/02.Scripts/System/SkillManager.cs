using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;

    private Dictionary<string, float> skills = new Dictionary<string, float>();
    private Dictionary<string, float> activePassives = new Dictionary<string, float>();

    bool isActvie;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        CheckPassive();
    }


    // 패시브 액티브 확인
    public void SetActiveSkill(bool isActive)
    {
        this.isActvie = isActive;
    }

    // 스킬 추가
    public void AddSkill(string skillName, float coolDown)
    {
        skills.Add(skillName, coolDown);

        /*StartCooldown(skillName);*/
    }

    // 패시브 스킬 가져오기
    public List<SkillData> GetPassiveSkills()
    {
        List<SkillData> passiveSkills = new List<SkillData>();

        foreach (SkillData skill in DataManager.instance.playerData.skillData)
        {
            if (skill.isPassive && !skill.isUnlock) // 스킬이 패시브면
            {
                passiveSkills.Add(skill); // 리스트에 추가
            }
        }
        return passiveSkills;
    }

    // 데이터 매니저 스킬 쿨타임 반환
    public float ReturnCoolDown(string skillName)
    {
        foreach (SkillData skill in DataManager.instance.playerData.skillData)
        {
            if (skill.skillName == skillName)
            {
                return skill.coolDown;
            }
        }
        return 0;
    }

    // 스킬 쿨타임 시작
    public void StartCooldown(string skillName)
    {
        if (skills.ContainsKey(skillName))
        {
            skills[skillName] = Time.time + ReturnCoolDown(skillName);
            //DataManager.instance.UseMana(skillName);
        }
    }

    // 남은 스킬 시간 반환
    public float GetRemainingCooldown(string skillName)
    {
        if (skills.ContainsKey(skillName))
        {
            float remainingTime = skills[skillName] - Time.time;
            return Mathf.Max(remainingTime, 0);
        }
        else
        {
            return 0;
        }
    }

    // 스킬 사용 가능 여부 
    public bool IsSkillOnCooldown(string skillName)
    {
        if (skills.ContainsKey(skillName))
        {
            return Time.time < skills[skillName];
        }
        return false;
    }

    // 패시브 스킬 발동 여부 확인
    private void CheckPassive()
    {
        // 현재 활성화된 패시브 스킬 체크
        List<SkillData> passiveSkills = GetPassiveSkills();

        foreach (SkillData skill in passiveSkills)
        {
            // 패시브 시간이 종료
            if (activePassives.ContainsKey(skill.skillName) && Time.time >= activePassives[skill.skillName])
            {
                isActvie = false;

                StartCooldown(skill.skillName); // 쿨타임 시작

                activePassives.Remove(skill.skillName); // 패시브 비활성화

                Debug.Log($"{skill.skillName} 패시브 비활성화!");
            }

            // 패시브 스킬 발동
            if (!activePassives.ContainsKey(skill.skillName) && GetRemainingCooldown(skill.skillName) <= 0 && isActvie)
            {
                ActivatePassiveSkill(skill);
            }
        }
    }

    // 패시브 스킬 발동
    public void ActivatePassiveSkill(SkillData passiveSkill)
    {
        activePassives[passiveSkill.skillName] = Time.time + passiveSkill.duration;
        Debug.Log($"{passiveSkill.skillName} 패시브 활성화!");
    }

    // 패시브가 활성화 됐는지 반환
    public bool IsPassiveActive(string skillName)
    {
        if(skillName == null)
        {
            return false;
        }
        else
        {
            return activePassives.ContainsKey(skillName);
        }

    }
}

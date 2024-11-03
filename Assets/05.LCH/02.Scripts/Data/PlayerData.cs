using System.Collections.Generic;

[System.Serializable]
public class PlayerData 
{
    public StatusData statusData;
    public List<SkillData> skillData;
    public List<AttackData> attackData;
    public RangeAttackData rangeAttackData;


    public PlayerData(StatusData statusData, List<SkillData> skillData, List<AttackData> attackData, RangeAttackData rangeAttackData)
    {
        this.statusData = statusData;
        this.skillData = skillData;
        this.attackData = attackData;
        this.rangeAttackData = rangeAttackData;
    }
}

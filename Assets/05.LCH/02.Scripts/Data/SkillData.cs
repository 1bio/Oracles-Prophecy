[System.Serializable]
public class SkillData 
{
    public string skillName;
    public int level;
    public float minDamage;
    public float maxDamage;
    public float damage;
    public float coolDown;
    public float force;
    public float forceTime;
    public bool isUnlock;
    public string description;
    public float multipleDamage;
    public float multipleCoolDown;
    public float duration;
    public bool isPassive;
    public float useMana;

    public SkillData(string skillName, int level, float minDamage, float maxDamage, float coolDown, float force, float forceTime, bool isUnlock, string description, float multipleDamage, float multipleCoolDown, float duration, bool isPassive, float useMana)
    {
        this.skillName = skillName;
        this.level = level;
        this.minDamage = minDamage;
        this.maxDamage = maxDamage;
        damage = UnityEngine.Random.Range(minDamage, maxDamage);
        this.coolDown = coolDown;
        this.force = force;
        this.forceTime = forceTime;
        this.isUnlock = isUnlock;
        this.description = description;
        this.multipleDamage = multipleDamage;
        this.multipleCoolDown = multipleCoolDown;
        this.duration = duration;
        this.isPassive = isPassive;
        this.useMana = useMana;
    }
}

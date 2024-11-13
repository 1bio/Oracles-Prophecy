[System.Serializable]
public class StatusData 
{
    public int level;
    public float exp;
    public int skillPoint;
    public float moveSpeed;
    public float currentHealth;
    public float maxHealth;
    public float currentMana;
    public float maxMana;
    public float minDamage;
    public float maxDamage;
    public float damage;
    public float defense;

    public StatusData(int level, float exp, float moveSpeed, float maxHealth, float maxMana, float minDamage, float maxDamage, float defense)
    {
        this.level = level;
        this.exp = exp;
        if(level % 3 == 0) { skillPoint += 1; } // 레벨 3레벨씩 스킬포인트 1 증가
        this.moveSpeed = moveSpeed;
        this.maxHealth = maxHealth;
        this.currentHealth = maxHealth;
        this.maxMana = maxMana;
        this.currentMana = maxMana;
        this.minDamage = minDamage;
        this.maxDamage = maxDamage;
        this.defense = defense;
    }
}

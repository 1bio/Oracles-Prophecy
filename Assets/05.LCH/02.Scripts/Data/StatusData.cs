[System.Serializable]
public class StatusData 
{
    public int level;
    public float moveSpeed;
    public float currentHealth;
    public float maxHealth;
    public float currentMana;
    public float maxMana;
    public float defense;
    public float exp;

    public StatusData(int level, float exp, float moveSpeed, float maxHealth, float maxMana, float defense)
    {
        this.level = level;
        this.exp = exp;
        this.moveSpeed = moveSpeed;
        this.maxHealth = maxHealth;
        this.currentHealth = maxHealth;
        this.maxMana = maxMana;
        this.currentMana = maxMana;
        this.defense = defense;
    }
}

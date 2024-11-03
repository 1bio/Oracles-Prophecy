[System.Serializable]
public class StatusData 
{
    public int level;
    public float moveSpeed;
    public float currentHealth;
    public float maxHealth;
    public float mana;
    public float defense;
    public float exp;

    public StatusData(int level, float exp, float moveSpeed, float maxHealth, float mana, float defense)
    {
        this.level = level;
        this.exp = exp;
        this.moveSpeed = moveSpeed;
        this.maxHealth = maxHealth;
        this.currentHealth = maxHealth;
        this.mana = mana;
        this.defense = defense;
    }
}

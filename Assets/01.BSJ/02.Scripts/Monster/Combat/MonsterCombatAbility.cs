using UnityEngine;

public class MonsterCombatAbility : IMonsterCombat
{
    public MonsterCombatAbility(MonsterStatData statData)
    {
        MonsterHealth = statData.CreateInstance().MonsterHealth;
        MonsterAttack = statData.CreateInstance().MonsterAttack;

        MoveSpeed = statData.CreateInstance().MoveSpeed;
        TurnSpeed = statData.CreateInstance().TurnSpeed;

        IsDead = statData.CreateInstance().IsDead;
    }
    public MonsterHealth MonsterHealth { get; private set; }
    public MonsterAttack MonsterAttack { get; private set; }

    public float MoveSpeed { get; private set; }
    public float TurnSpeed { get; private set; }

    public bool IsDead { get; set; }
    
}

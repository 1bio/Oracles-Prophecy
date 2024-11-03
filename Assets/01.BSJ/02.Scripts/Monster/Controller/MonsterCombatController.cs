public class MonsterCombatController
{
    public MonsterCombatController(MonsterStatData statData, Health health)
    {
        MonsterCombatAbility = new MonsterCombatAbility(statData);
        Health = health;

        MonsterCombatAbility.MonsterHealth.InitializeHealth();
    }

    public MonsterCombatAbility MonsterCombatAbility { get; private set; }
    public Health Health { get; private set; }
}

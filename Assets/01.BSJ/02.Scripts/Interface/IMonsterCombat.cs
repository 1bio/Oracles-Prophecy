public interface IMonsterCombat
{
    MonsterHealth MonsterHealth { get; }
    MonsterAttack MonsterAttack { get; }
    //MonsterTargetDistance MonsterTargetDistance { get; }
    float MoveSpeed { get; }
    float TurnSpeed { get; }
    bool IsDead { get; set; }
}


[System.Serializable]
public class RangeAttackData : AttackData
{
    public RangeAttackData(
        string animationName, 
        int comboAttackIndex, 
        float comboAttackTime,
        float transitionDuration,
        float force, 
        float forceTime,
        float damage,
        float knockBack)
         :
         base(animationName,
            comboAttackIndex,
            comboAttackTime,
            transitionDuration,
            force,
            forceTime,
            damage,
            knockBack)
    {
    }
}

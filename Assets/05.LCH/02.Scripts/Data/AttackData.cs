[System.Serializable]
public class AttackData
{
    public string AnimationName { get; set; }  // 애니메이션 이름

    public int ComboAttackIndex = -1; // 콤보 애니메이션 인덱스

    public float ComboAttackTime; // 콤보 공격 시전 시간

    public float TransitionDuration;// 애니메이션 전환 시간

    public float Force; // 공격 시 이동 하는 속도

    public float ForceTime; // 시전 시간

    public float minDamage;

    public float maxDamage;

    public float Damage;// 데미지

    public float KnockBack; // 넉백 속도


    public AttackData(string animationName, int comboAttackIndex, float comboAttackTime, float transitionDuration, float force, /*float damage, */float forceTime, float knockBack)
    {
        this.AnimationName = animationName;
        this.ComboAttackIndex = comboAttackIndex;
        this.ComboAttackTime = comboAttackTime;
        this.TransitionDuration = transitionDuration;
        this.Force = force;
        this.ForceTime = forceTime;
        //this.Damage = damage;
        this.KnockBack = knockBack;
    }
}

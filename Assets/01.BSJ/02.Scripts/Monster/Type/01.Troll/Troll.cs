using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Troll : Monster
{
    // Fire Breath
    public enum FireBreathAnimationName
    {
        FireBreath
    }

    public Transform FirePositionTransform { get; private set; }

    // Charge Attack
    public enum ChargeAttackAnimationName
    {
        ChargeAttack
    }

    public new void Awake()
    {
        base.Awake();

        FirePositionTransform = transform.Find("FirePos");
    }
}
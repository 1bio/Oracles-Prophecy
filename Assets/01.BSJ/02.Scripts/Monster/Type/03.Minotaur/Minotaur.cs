using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minotaur : Monster
{
    public enum RamAttackAnimationName
    {
        RamStart,
        RamRun,
        RamAttack,
        RamWall
    }

    public void PlayFireVFX()
    {
        ParticleController.RePlayVFX("FireThrower", 360, 18);
    }
}

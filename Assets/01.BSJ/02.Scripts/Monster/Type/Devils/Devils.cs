using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Devils : Monster
{

    public void PlayFireVFX()
    {
        ParticleController.RePlayVFX("FireFollowing", 100, 5);
    }
}

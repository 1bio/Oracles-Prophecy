using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weeper : Monster
{
    public Transform FirePositionTransform { get; private set; }

    public new void Awake()
    {
        base.Awake();

        FirePositionTransform = transform.Find("FirePosition");
    }
}

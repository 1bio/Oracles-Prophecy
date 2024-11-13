using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public abstract class MonsterBehavior
{
    public abstract void OnBehaviorStart(Monster monster);
    public abstract void OnBehaviorUpdate(Monster monster);
    public abstract void OnBehaviorEnd(Monster monster);
}
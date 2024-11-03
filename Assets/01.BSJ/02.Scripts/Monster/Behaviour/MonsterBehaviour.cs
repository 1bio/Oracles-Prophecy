using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public abstract class MonsterBehaviour
{
    public abstract void OnBehaviourStart(Monster monster);
    public abstract void OnBehaviourUpdate(Monster monster);
    public abstract void OnBehaviourEnd(Monster monster);
}
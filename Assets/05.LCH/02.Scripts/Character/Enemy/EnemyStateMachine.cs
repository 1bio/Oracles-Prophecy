using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : StateMachine
{
    [field: SerializeField] public ForceReceiver ForceReceiver;

    [field: SerializeField] public Health Health;


    [field: SerializeField] public CharacterController Controller;


    [field: SerializeField] public float MoveSpeed;


    private void Start()
    {
        ChangeState(new EnemyFreeLookState(this));   
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFreeLookState : EnemyBaseState
{
    // BlendTree �ִϸ��̼� ����
    public readonly int Locomotion = Animator.StringToHash("Locomotion"); // ���Ÿ� ���� Ʈ��

    public readonly int Velocity = Animator.StringToHash("Velocity"); // �ִϸ��̼� �Ķ����

    public readonly float CrossFadeDuration = 0.1f;

    public readonly float DampTime = 0.1f;

    public EnemyFreeLookState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine)
    {
    }

    public override void Enter()
    {
    }


    public override void Tick(float deltaTime)
    {
        Move(deltaTime);
    }

    public override void Exit()
    {
    }
}

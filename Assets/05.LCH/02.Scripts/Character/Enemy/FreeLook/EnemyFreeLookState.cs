using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFreeLookState : EnemyBaseState
{
    // BlendTree 애니메이션 변수
    public readonly int Locomotion = Animator.StringToHash("Locomotion"); // 원거리 블렌드 트리

    public readonly int Velocity = Animator.StringToHash("Velocity"); // 애니메이션 파라미터

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

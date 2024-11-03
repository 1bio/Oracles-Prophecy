using UnityEngine;

public abstract class EnemyBaseState : State
{
    protected EnemyStateMachine stateMachine;

    public EnemyBaseState(EnemyStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    // 이동
    protected void Move(Vector3 motion, float deltaTime)
    {
        stateMachine.Controller.Move((motion + stateMachine.ForceReceiver.movement) * stateMachine.MoveSpeed * deltaTime);
    }

    // 이동(넉백과 같은 물리적인 힘)
    protected void Move(float deltaTime)
    {
        Move(Vector3.zero, deltaTime);
    }

}

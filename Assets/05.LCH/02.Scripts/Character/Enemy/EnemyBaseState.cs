using UnityEngine;

public abstract class EnemyBaseState : State
{
    protected EnemyStateMachine stateMachine;

    public EnemyBaseState(EnemyStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    // �̵�
    protected void Move(Vector3 motion, float deltaTime)
    {
        stateMachine.Controller.Move((motion + stateMachine.ForceReceiver.movement) * stateMachine.MoveSpeed * deltaTime);
    }

    // �̵�(�˹�� ���� �������� ��)
    protected void Move(float deltaTime)
    {
        Move(Vector3.zero, deltaTime);
    }

}

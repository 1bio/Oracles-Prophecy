using UnityEngine;

public abstract class PlayerBaseState : State
{
    protected PlayerStateMachine stateMachine; 

    public PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    #region Main Methods
    // 입력 처리
    public Vector3 CalculatorMovement()
    {
        Vector3 forward = stateMachine.CameraTransform.forward;
        Vector3 right = stateMachine.CameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        return forward * stateMachine.InputReader.MoveValue.y
            + right * stateMachine.InputReader.MoveValue.x;
    }

    // 이동
    protected void Move(Vector3 motion, float deltaTime)
    {
        stateMachine.Controller.Move((motion + stateMachine.ForceReceiver.movement) * DataManager.instance.playerData.statusData.moveSpeed * deltaTime);
    }

    // 이동(넉백과 같은 물리적인 힘의 이동)
    protected void Move(float deltaTime)
    {
        Move(Vector3.zero, deltaTime);
    }


    // 회전(이동 키 입력 방향)
    protected void Rotate(Vector3 movement, float deltaTime)
    {
        if (stateMachine.InputReader.MoveValue == Vector2.zero)
            return;
            
        movement.y = 0f;
        
        stateMachine.transform.rotation = Quaternion.Lerp(
            stateMachine.transform.rotation,
            Quaternion.LookRotation(movement),
            deltaTime * stateMachine.RotationSpeed);
    }

    // 회전(마우스 공격 방향)
    protected void Aiming()
    {
        /*if (stateMachine.InputReader.IsAiming == false)
            return;*/

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Plane plane = new Plane(Vector3.up, Vector3.up);

        if (plane.Raycast(ray, out float distance))
        {
            Vector3 direction = ray.GetPoint(distance) - stateMachine.transform.position;
            stateMachine.transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        }
    }


    // 삭제 예정
    /*    // 공격 시 회전 보정
        protected void FaceTarget()
        {
            if (!stateMachine.Targeting.CurrentTarget)
                return;

            Vector3 direction = stateMachine.Targeting.CurrentTarget.transform.position - stateMachine.transform.position;

            direction.y = 0f;

            stateMachine.transform.rotation = Quaternion.LookRotation(direction);
        }
    */

    /*// 자동 회전
    protected void AutoRotate(float deltaTime)
    {
        if (stateMachine.Targeting.CurrentTarget == null)
            return;

        if (stateMachine.InputReader.isAutoRotate == false)
            return;

        Vector3 direction = stateMachine.Targeting.CurrentTarget.transform.position - stateMachine.transform.position;
        direction.y = 0;

        stateMachine.transform.rotation = Quaternion.Lerp(
                    stateMachine.transform.rotation,
                    Quaternion.LookRotation(direction),
                    deltaTime * 25);
    }*/
    #endregion
}

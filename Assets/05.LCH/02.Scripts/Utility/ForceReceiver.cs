using System;
using UnityEngine;
using UnityEngine.TextCore.Text;

/// <summary>
/// 넉백, 구르기 가속 등 물리 처리용 클래스
/// </summary>

public class ForceReceiver : MonoBehaviour
{
    [SerializeField] private CharacterController controller;

    [SerializeField] private float rollSpeed;

    public Vector3 movement => impact + Vector3.up * verticalVelocity;

    private Vector3 impact;
    private Vector3 dampingVelocity;

    private float verticalVelocity; 
    private float drag = 0.3f;

    private void Update()
    {
        if(controller.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }

        impact = Vector3.SmoothDamp(impact, Vector3.zero, ref dampingVelocity, drag);
    }


    #region Main Methods
    // 구르기
    public void RollingForce(float deltaTime)
    {
        controller.Move(transform.forward * rollSpeed * deltaTime);
    }

    // 근접 공격 시 가속 및 넉백
    public void AddForce(Vector3 force)
    {
        impact += force;
    }
    #endregion
}

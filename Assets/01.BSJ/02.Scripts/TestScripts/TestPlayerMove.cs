using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float runSpeed = 8f;
    public float gravity = -9.81f;

    private CharacterController characterController;
    private Animator _animator;
    private Vector3 velocity;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        /*Move();*/
        _animator.SetFloat("Locomotion", 2);
        characterController.Move(transform.forward * moveSpeed * Time.deltaTime);

    }

    /* private void Move()
     {
         float moveX = Input.GetAxis("Horizontal");
         float moveZ = Input.GetAxis("Vertical");

         // 움직임 방향 벡터 계산
         Vector3 move = transform.right * moveX + transform.forward * moveZ;

         // 이동 속도 설정
         float currentSpeed = moveSpeed;

         // 걷기와 달리기 전환
         if (Input.GetKey(KeyCode.LeftShift))
         {
             currentSpeed = runSpeed;
             _animator.SetFloat("Locomotion", 2); // 달리기 애니메이션
         }
         else if (move != Vector3.zero)
         {
             _animator.SetFloat("Locomotion", 1); // 걷기 애니메이션
         }
         else
         {
             _animator.SetFloat("Locomotion", 0); // Idle 애니메이션
         }

         // 플레이어 움직임 적용
         characterController.Move(move * currentSpeed * Time.deltaTime);

         // 중력 처리
         if (characterController.isGrounded && velocity.y < 0)
         {
             velocity.y = -2f;
         }

         velocity.y += gravity * Time.deltaTime;
         characterController.Move(velocity * Time.deltaTime);
     }*/
}

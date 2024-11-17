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

         // ������ ���� ���� ���
         Vector3 move = transform.right * moveX + transform.forward * moveZ;

         // �̵� �ӵ� ����
         float currentSpeed = moveSpeed;

         // �ȱ�� �޸��� ��ȯ
         if (Input.GetKey(KeyCode.LeftShift))
         {
             currentSpeed = runSpeed;
             _animator.SetFloat("Locomotion", 2); // �޸��� �ִϸ��̼�
         }
         else if (move != Vector3.zero)
         {
             _animator.SetFloat("Locomotion", 1); // �ȱ� �ִϸ��̼�
         }
         else
         {
             _animator.SetFloat("Locomotion", 0); // Idle �ִϸ��̼�
         }

         // �÷��̾� ������ ����
         characterController.Move(move * currentSpeed * Time.deltaTime);

         // �߷� ó��
         if (characterController.isGrounded && velocity.y < 0)
         {
             velocity.y = -2f;
         }

         velocity.y += gravity * Time.deltaTime;
         characterController.Move(velocity * Time.deltaTime);
     }*/
}

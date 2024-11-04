using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float gravity = -9.81f;

    private CharacterController characterController;
    private Vector3 velocity;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        // �Է� �޾ƿ���
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // �̵� ���� ����
        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        // �÷��̾� �̵�
        characterController.Move(move * moveSpeed * Time.deltaTime);

        // �߷� ����
        if (characterController.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }
}

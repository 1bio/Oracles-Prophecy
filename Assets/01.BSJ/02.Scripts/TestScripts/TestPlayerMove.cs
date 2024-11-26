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
        _animator.SetFloat("Locomotion", 1);
        characterController.Move(transform.forward * moveSpeed * Time.deltaTime);

    }

}

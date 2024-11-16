using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordingMedusaH : MonoBehaviour
{
    private CharacterController characterController;
    private Animator _animator;
    public float mSpeed;
    public bool _jump;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.E))
            _animator.SetTrigger("Play");

        if (Input.GetKeyUp(KeyCode.R))
        {
            _jump = false;
            _animator.SetTrigger("T");
        }

        if (_jump)
        {
            characterController.Move(transform.forward * mSpeed * Time.deltaTime);
        }
        
    }

    public void Jump()
    {
        _jump = _jump ? false : true;
    }
}

using UnityEngine;
using UnityEngine.InputSystem;
using System;

[Serializable]
public class InputReader : MonoBehaviour, InputActions.IPlayerActions
{
    public InputActions InputActions { get; private set; }

    public Vector2 MoveValue { get; private set; }

    public bool IsAttacking { get; private set; } = false;

    public bool IsRolling { get; private set; } = false;

    public bool IsTransition { get; private set; } = false;

    public event Action RollEvent;

    public event Action firstSkillEvent;
    public event Action SecondSkillEvent;
    public event Action thirdSkillEvent;


    private void Awake()
    {
        InputActions = new InputActions();

        InputActions.Player.SetCallbacks(this);

        InputActions.Player.Enable();
    }

    private void OnDestroy()
    {
        InputActions.Player.Disable();
    }


    #region Callback Methods
    // Moving
    void InputActions.IPlayerActions.OnMove(InputAction.CallbackContext context)
    {
        MoveValue = context.ReadValue<Vector2>();
    }

    // Attacking
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            IsAttacking = true;
        }
        else if (context.canceled)
        {
            IsAttacking = false;
        }
    }

    // Rolling
    public void OnRoll(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        RollEvent?.Invoke();
    }

    // Skill 1
    public void OnSkill1(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        firstSkillEvent?.Invoke();
    }

    // Skill 2
    public void OnSkill2(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        SecondSkillEvent?.Invoke();
    }
    #endregion


    // Skill 3
    public void OnSkill3(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        thirdSkillEvent?.Invoke();
    }

}

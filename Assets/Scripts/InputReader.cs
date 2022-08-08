using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, Controls.IPlayerActions
{
    public event Action DodgeEvent;
    public event Action JumpEvent;
    public event Action TargetEvent;
    public event Action CancelEvent;
    public event Action InteractEvent;
    public event Action OpenInventoryEvent;
    public Vector2 MovementValue { get; private set; }
    public bool IsAttacking { get; private set; }
    public bool IsBlocking { get; private set; }

    public bool UIOpen { get; private set; }
    
    Controls controls;

    void Start()
    {
        controls = new Controls();
        controls.Player.SetCallbacks(this);
        controls.Player.Enable();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnDestroy() 
    {
        controls?.Player.Disable();
    }

    public void OnDodge(InputAction.CallbackContext context)
    {
        if (!context.performed || UIOpen) return;
        DodgeEvent?.Invoke();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        JumpEvent?.Invoke();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (UIOpen) return;
        MovementValue = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        
    }

    public void OnTarget(InputAction.CallbackContext context)
    {
        if (!context.performed || UIOpen) return;
        TargetEvent?.Invoke();
    }

    public void OnCancel(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        CancelEvent?.Invoke();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed && !UIOpen) IsAttacking = true;
        else if (context.canceled) IsAttacking = false;
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!context.performed || UIOpen) return;
        InteractEvent?.Invoke();
    }

    public void OnBlock(InputAction.CallbackContext context)
    {
        if (context.performed && !UIOpen) IsBlocking = true;
        else if (context.canceled) IsBlocking = false;
    }

    public void OnInventory(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        UIOpen = !UIOpen;
        if (UIOpen)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else 
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        OpenInventoryEvent?.Invoke();
    }
}

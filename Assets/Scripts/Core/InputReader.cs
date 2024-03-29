using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, Controls.IPlayerActions
{
    public event Action DodgeEvent;
    public event Action JumpEvent;
    public event Action SneakEvent;
    public event Action TargetEvent;
    public event Action PauseEvent;
    public event Action InteractEvent;
    public event Action OpenInventoryEvent;
    public event Action ConfirmEvent;
    public event Action HotKey1Event;
    public event Action HotKey2Event;
    public event Action HotKey3Event;
    public event Action HotKey4Event;
    public Vector2 MovementValue { get; private set; }
    public bool IsAttacking { get; private set; }
    public bool IsBlocking { get; private set; }

    public bool UIOpen;
    
    Controls controls;

    void Start()
    {
        controls = new Controls();
        controls.Player.SetCallbacks(this);
        controls.Player.Enable();
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
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

    public void OnSneak(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        SneakEvent?.Invoke();
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
        OpenInventoryEvent?.Invoke();
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        PauseEvent?.Invoke();
    }

    public void OnConfirm(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        ConfirmEvent?.Invoke();
    }

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void OnHotKey1(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        HotKey1Event?.Invoke();
    }

    public void OnHotKey2(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        HotKey2Event?.Invoke();
    }

    public void OnHotKey3(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        HotKey3Event?.Invoke();
    }

    public void OnHotKey4(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        HotKey4Event?.Invoke();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, Controls.IPlayerActions
{
    public event Action DodgeEvent;
    public event Action JumpEvent;
    
    Controls controls;

    void Start()
    {
        controls = new Controls();
        controls.Player.SetCallbacks(this);
        controls.Player.Enable();
    }

    private void OnDestroy() 
    {
        controls.Player.Disable();
    }

    public void OnDodge(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        DodgeEvent?.Invoke();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        JumpEvent?.Invoke();
    }
}

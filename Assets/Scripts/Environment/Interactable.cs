using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour 
{
    public event Action<Interactable> OnDestroyed;
    public bool CanInteract = true;

    public abstract void Interact(PlayerStateMachine stateMachine);
    private void OnDestroy()
    {
        OnDestroyed?.Invoke(this);
    }
}

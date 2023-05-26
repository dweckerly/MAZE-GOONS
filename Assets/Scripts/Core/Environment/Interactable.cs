using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InteractableType
{
    Body,
    Chest,
    Door,
    Gold,
    Lever,
    PickUp,
    Statue
}

public abstract class Interactable : MonoBehaviour 
{
    public event Action<Interactable> OnDestroyed;
    public bool CanInteract = true;
    public abstract InteractableType type { get; }

    public abstract void Interact(PlayerStateMachine stateMachine);

    private void OnDestroy()
    {
        OnDestroyed?.Invoke(this);
    }
}

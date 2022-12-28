using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour 
{
    public event Action<Interactable> OnDestroyed;
    public bool CanInteract = true;
    public GameObject promptCanvas;

    public abstract void Interact(PlayerStateMachine stateMachine);

    public void ShowPrompt()
    {
        if (promptCanvas != null) promptCanvas.SetActive(true);
    }

    public void HidePrompt()
    {
        if (promptCanvas != null) promptCanvas.SetActive(false);
    }

    private void OnDestroy()
    {
        OnDestroyed?.Invoke(this);
    }
}

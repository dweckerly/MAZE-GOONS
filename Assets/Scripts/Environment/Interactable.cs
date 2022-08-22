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
        promptCanvas.SetActive(true);
    }

    public void HidePrompt()
    {
        promptCanvas.SetActive(false);
    }

    private void OnDestroy()
    {
        OnDestroyed?.Invoke(this);
    }
}

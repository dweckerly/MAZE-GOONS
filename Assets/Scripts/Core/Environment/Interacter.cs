using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interacter : MonoBehaviour
{
    private Camera mainCamera;
    private List<Interactable> interactables = new List<Interactable>();
    public Interactable Interaction;

    public delegate void OnInteractWithUI(Loot loot);
    public event OnInteractWithUI OnInteractEventWithUI;
    public event Action OnMakeOffering;

    public delegate void OnDetectInteractable(Interactable interactable);
    public event OnDetectInteractable OnDetectInteractableEvent;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<Interactable>(out Interactable interactable)) return;
        if (interactable.CanInteract) 
        {
            interactables.Add(interactable);
            interactable.OnDestroyed += RemoveTarget;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent<Interactable>(out Interactable interact)) return;
        RemoveTarget(interact);
    }

    private void LateUpdate() 
    {
        FindClosestInteraction();
        OnDetectInteractableEvent?.Invoke(Interaction);
    }

    private void FindClosestInteraction()
    {
        if (interactables.Count == 0) return;
        Interactable closestInteraction = null;
        float closestInteractionDistance = Mathf.Infinity;
        foreach (Interactable interactable in interactables)
        {
            Vector2 viewPosition = mainCamera.WorldToViewportPoint(interactable.transform.position);
            if (viewPosition.x < 0 || viewPosition.x > 1 || viewPosition.y < 0 || viewPosition.y > 1) continue;
            Vector2 distanceToCenter = viewPosition - new Vector2(0.5f, 0.5f);
            if (distanceToCenter.sqrMagnitude < closestInteractionDistance)
            {
                closestInteraction = interactable;
                closestInteractionDistance = distanceToCenter.sqrMagnitude;
            }
        }
        Interaction = closestInteraction;
    }

    public bool SelectInteraction()
    {
        if (Interaction == null || !Interaction.CanInteract) return false;
        if (Interaction is Loot) OnInteractEventWithUI?.Invoke((Loot)Interaction);
        if (Interaction is OfferingStatue) OnMakeOffering?.Invoke();
        return true;
    }

    public void Cancel()
    {
        if (Interaction == null) return;
        Interaction = null;
    }

    private void RemoveTarget(Interactable interactable)
    {
        if (Interaction == interactable) Interaction = null;
        interactable.OnDestroyed -= RemoveTarget;
        interactables.Remove(interactable);
    }
}

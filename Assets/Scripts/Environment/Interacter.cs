using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interacter : MonoBehaviour
{
    private Camera mainCamera;
    private List<Interactable> interactables = new List<Interactable>();
    public Interactable Interaction;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<Interactable>(out Interactable interact)) return;
        interactables.Add(interact);
        interact.OnDestroyed += RemoveTarget;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent<Interactable>(out Interactable interact)) return;
        RemoveTarget(interact);
    }

    public bool SelectInteraction()
    {
        if (interactables.Count == 0) return false;
        Interactable closestInteraction = null;
        float closestInteractionDistance = Mathf.Infinity;
        foreach (Interactable interact in interactables)
        {
            Vector2 viewPosition = mainCamera.WorldToViewportPoint(interact.transform.position);
            if (viewPosition.x < 0 || viewPosition.x > 1 || viewPosition.y < 0 || viewPosition.y > 1) continue;
            Vector2 distanceToCEnter = viewPosition - new Vector2(0.5f, 0.5f);
            if (distanceToCEnter.sqrMagnitude < closestInteractionDistance)
            {
                closestInteraction = interact;
                closestInteractionDistance = distanceToCEnter.sqrMagnitude;
            }
        }
        if (closestInteraction == null) return false;
        Interaction = closestInteraction;
        return true;
    }

    public void Cancel()
    {
        if (Interaction == null) return;
        Interaction = null;
    }

    private void RemoveTarget(Interactable interact)
    {
        if (Interaction == interact) Interaction = null;
        interact.OnDestroyed -= RemoveTarget;
        interactables.Remove(interact);
    }
}

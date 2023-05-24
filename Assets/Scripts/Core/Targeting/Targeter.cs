using System;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Targeter : MonoBehaviour
{
    public CinemachineVirtualCamera CombatCamera;
    public Transform DefaultTarget;
    private Camera mainCamera;
    private List<Target> targets = new List<Target>();
    public Target CurrentTarget { get; private set; }

    public Action TargetAction;

    private void Start() 
    {
        mainCamera = Camera.main;    
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (!other.TryGetComponent<Target>(out Target target)) return;
        targets.Add(target);
        target.OnDestroyed += RemoveTarget;
    }

    private void OnTriggerExit(Collider other) 
    {
        if (!other.TryGetComponent<Target>(out Target target)) return;
        RemoveTarget(target);
    }

    public bool SelectTarget()
    {
        if (targets.Count == 0) return false;
        Target closestTarget = null;
        float closestTargetDistance = Mathf.Infinity;
        foreach (Target target in targets)
        {
            Vector2 viewPosition = mainCamera.WorldToViewportPoint(target.transform.position);
            if (!target.targetRenderer.isVisible) continue;
            Vector2 distanceToCenter = viewPosition - new Vector2(0.5f, 0.5f);
            if (distanceToCenter.sqrMagnitude < closestTargetDistance)
            {
                closestTarget = target;
                closestTargetDistance = distanceToCenter.sqrMagnitude;
            }
        }
        if (closestTarget == null) return false;
        CurrentTarget = closestTarget;
        CurrentTarget.ShowHealth();
        TargetAction.Invoke();
        CombatCamera.LookAt = closestTarget.transform;
        return true;
    }

    public void Cancel()
    {
        if (CurrentTarget == null) return;
        TargetAction?.Invoke();
        CombatCamera.LookAt = DefaultTarget;
        CurrentTarget.HideHealth();
        CurrentTarget = null;
    }

    private void RemoveTarget(Target target)
    {
        target.HideHealth();
        if (CurrentTarget == target)
        {
            TargetAction?.Invoke();
            CombatCamera.LookAt = DefaultTarget;
            CurrentTarget = null;
        }
        target.OnDestroyed -= RemoveTarget;
        targets.Remove(target);
    }
}

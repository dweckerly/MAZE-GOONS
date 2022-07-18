using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Targeter : MonoBehaviour
{
    [SerializeField] private CinemachineTargetGroup cinTargetGroup;
    private Camera mainCamera;
    private List<Target> targets = new List<Target>();
    public Target CurrentTarget { get; private set; }

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
            if (viewPosition.x < 0 || viewPosition.x > 1 || viewPosition.y < 0 || viewPosition.y > 1) continue;
            Vector2 distanceToCEnter = viewPosition - new Vector2(0.5f, 0.5f);
            if (distanceToCEnter.sqrMagnitude < closestTargetDistance)
            {
                closestTarget = target;
                closestTargetDistance = distanceToCEnter.sqrMagnitude;
            }
        }
        if (closestTarget == null) return false;
        CurrentTarget = closestTarget;
        cinTargetGroup.AddMember(CurrentTarget.transform, 1f, 2f);
        return true;
    }

    public void Cancel()
    {
        if (CurrentTarget == null) return;
        cinTargetGroup.RemoveMember(CurrentTarget.transform);
        CurrentTarget = null;
    }

    private void RemoveTarget(Target target)
    {
        if (CurrentTarget == target)
        {
            cinTargetGroup.RemoveMember(CurrentTarget.transform);
            CurrentTarget = null;
        }
        target.OnDestroyed -= RemoveTarget;
        targets.Remove(target);
    }
}

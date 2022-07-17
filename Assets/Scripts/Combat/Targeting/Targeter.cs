using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Targeter : MonoBehaviour
{
    [SerializeField] private CinemachineTargetGroup cinTargetGroup;
    private List<Target> targets = new List<Target>();
    public Target CurrentTarget { get; private set; }

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
        CurrentTarget = targets[0];
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

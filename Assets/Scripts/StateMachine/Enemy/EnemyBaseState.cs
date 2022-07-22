using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBaseState : State
{
    protected EnemyStateMachine stateMachine;

    protected const float CrossFadeDuration = 0.1f;

    public EnemyBaseState(EnemyStateMachine _stateMachine)
    {
        stateMachine = _stateMachine;
    }

    protected bool IsInDetectionRange(GameObject go)
    {
        float toDistanceSqr = (go.transform.position - stateMachine.transform.position).sqrMagnitude;
        return toDistanceSqr <= stateMachine.DetectionRange * stateMachine.DetectionRange;
    }

    protected void Move(float deltaTime)
    {
        Move(Vector3.zero, deltaTime);
    }

    protected void Move(Vector3 movement, float deltaTime)
    {
        stateMachine.Controller.Move((movement + stateMachine.ForceReceiver.Movement) * deltaTime);
    }
}

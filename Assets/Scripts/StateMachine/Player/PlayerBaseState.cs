using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState : State
{
    protected PlayerStateMachine stateMachine;

    public PlayerBaseState(PlayerStateMachine _stateMachine)
    {
        stateMachine = _stateMachine;
    }

    protected void Move(Vector3 movement, float deltaTime)
    {
        stateMachine.Controller.Move((movement + stateMachine.ForceReceiver.Movement) * deltaTime);
    }

    protected void FaceTarget()
    {
        if (stateMachine.Targeter.CurrentTarget == null) return;
        Vector3 lookPosition = stateMachine.Targeter.CurrentTarget.transform.position - stateMachine.transform.position;
        lookPosition.y = 0f;
        stateMachine.transform.rotation = Quaternion.LookRotation(lookPosition);
    }
}

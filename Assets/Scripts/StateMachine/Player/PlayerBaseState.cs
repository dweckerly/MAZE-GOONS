using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class  PlayerBaseState : State
{
    protected PlayerStateMachine stateMachine;

    protected const float CrossFadeDuration = 0.1f;

    public PlayerBaseState(PlayerStateMachine _stateMachine)
    {
        stateMachine = _stateMachine;
    }

    protected void Move(float deltaTime)
    {
        Move(Vector3.zero, deltaTime);
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

    protected void ReturnToLocomotion()
    {
        if (stateMachine.Targeter.CurrentTarget == null) stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
        if (stateMachine.sneaking) stateMachine.SwitchState(new PlayerSneakState(stateMachine));
        else stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
    }

    protected void FaceMovementDirection(Vector3 movement, float deltaTime)
    {
        stateMachine.transform.rotation = Quaternion.Lerp(
            stateMachine.transform.rotation,
            Quaternion.LookRotation(movement),
            stateMachine.rotationDamping * deltaTime
        );
    }

    protected Vector3 CalculateMovementDirection()
    {
        Vector3 forward = stateMachine.MainCameraTransform.forward;
        Vector3 right = stateMachine.MainCameraTransform.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        return forward * stateMachine.InputReader.MovementValue.y + right * stateMachine.InputReader.MovementValue.x;
    }
}

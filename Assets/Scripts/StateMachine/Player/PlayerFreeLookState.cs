using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFreeLookState : PlayerBaseState
{
    private readonly int speedPercent = Animator.StringToHash("speedPercent");
    private readonly int FreeLookBlendTree = Animator.StringToHash("Free Look Blend Tree");

    public PlayerFreeLookState(PlayerStateMachine _stateMachine) : base(_stateMachine) { }

    private const float animatorDampTime = 0.1f;
    private const float gravity = 20.0f;

    public override void Enter() 
    {
        stateMachine.animator.Play(FreeLookBlendTree);
        stateMachine.InputReader.TargetEvent += OnTarget;
    }

    public override void Tick(float deltaTime)
    {
        Vector3 movement = CalculateMovementDirection();
        stateMachine.Controller.Move(movement * stateMachine.freeLookSpeed * deltaTime);
        if (stateMachine.InputReader.MovementValue == Vector2.zero)
        {
            stateMachine.animator.SetFloat(speedPercent, 0, animatorDampTime, deltaTime);
            return;
        }
        FaceMovementDirection(movement, deltaTime);
        float currentSpeed = new Vector2(stateMachine.Controller.velocity.x, stateMachine.Controller.velocity.z).magnitude;
        stateMachine.animator.SetFloat(speedPercent, currentSpeed / stateMachine.freeLookSpeed, animatorDampTime, deltaTime);
    }

    public override void Exit() 
    {
        stateMachine.InputReader.TargetEvent -= OnTarget;
    }

    private void OnTarget()
    {
        if (!stateMachine.Targeter.SelectTarget()) return;
        stateMachine.SwicthState(new PlayerTargetingState(stateMachine));
    }

    private void FaceMovementDirection(Vector3 movement, float deltaTime)
    {
        stateMachine.transform.rotation = Quaternion.Lerp(
            stateMachine.transform.rotation,
            Quaternion.LookRotation(movement),
            stateMachine.rotationDamping * deltaTime
        );
    }

    private Vector3 CalculateMovementDirection()
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

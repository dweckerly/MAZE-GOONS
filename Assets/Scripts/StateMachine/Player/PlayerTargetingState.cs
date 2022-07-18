using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargetingState : PlayerBaseState
{
    private readonly int TargetingBlendTree = Animator.StringToHash("Targeting Blend Tree");
    private readonly int TargetingForward = Animator.StringToHash("TargetingForward");
    private readonly int TargetingRight = Animator.StringToHash("TargetingRight");

    public PlayerTargetingState(PlayerStateMachine _stateMachine) : base(_stateMachine) {}

    public override void Enter()
    {
        stateMachine.animator.Play(TargetingBlendTree);
        stateMachine.InputReader.CancelEvent += OnCancel;
    }

    public override void Tick(float deltaTime)
    {
        if (stateMachine.Targeter.CurrentTarget == null)
        { 
            stateMachine.SwicthState(new PlayerFreeLookState(stateMachine));
            return;
        }
        if (stateMachine.InputReader.IsAttacking)
        {
            stateMachine.SwicthState(new PlayerAttackingState(stateMachine, 0));
            return;
        }
        Vector3 movement = CalculateMovement();
        UpdateAnimator(deltaTime);
        Move(movement * stateMachine.targetingSpeed, deltaTime);
        FaceTarget();
    }

    public override void Exit()
    {
        stateMachine.InputReader.CancelEvent -= OnCancel;
    }

    private void UpdateAnimator(float deltaTime)
    {
        if (stateMachine.InputReader.MovementValue.y == 0) stateMachine.animator.SetFloat(TargetingForward, 0, 0.1f, deltaTime);
        else 
        {
            float value = stateMachine.InputReader.MovementValue.y > 0 ? 1f : -1f;
            stateMachine.animator.SetFloat(TargetingForward, value, 0.1f, deltaTime);
        }
        if (stateMachine.InputReader.MovementValue.x == 0) stateMachine.animator.SetFloat(TargetingRight, 0, 0.1f, deltaTime);
        else
        {
            float value = stateMachine.InputReader.MovementValue.x > 0 ? 1f : -1f;
            stateMachine.animator.SetFloat(TargetingRight, value, 0.1f, deltaTime);
        }
    }

    private void OnCancel()
    {
        stateMachine.Targeter.Cancel();
        stateMachine.SwicthState(new PlayerFreeLookState(stateMachine));
    }

    private Vector3 CalculateMovement()
    {
        Vector3 movement = new Vector3();
        movement += stateMachine.transform.right * stateMachine.InputReader.MovementValue.x;
        movement += stateMachine.transform.forward * stateMachine.InputReader.MovementValue.y;
        return movement;
    }
}

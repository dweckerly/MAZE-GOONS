using System;
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
        stateMachine.animator.CrossFadeInFixedTime(FreeLookBlendTree, CrossFadeDuration);
        stateMachine.InputReader.TargetEvent += OnTarget;
        stateMachine.InputReader.InteractEvent += OnInteract;
        stateMachine.InputReader.DodgeEvent += OnDodge;
        stateMachine.InputReader.JumpEvent += OnJump;
        stateMachine.InputReader.SneakEvent += OnSneak;
    }

    public override void Tick(float deltaTime)
    {
        if (stateMachine.InputReader.IsAttacking)
        {
            stateMachine.SwitchState(new PlayerAttackingState(stateMachine, 0));
            return;
        }
        if (stateMachine.InputReader.IsBlocking)
        {
            stateMachine.SwitchState(new PlayerBlockingState(stateMachine));
            return;
        }
        Vector3 movement = CalculateMovementDirection();
        Move(movement * stateMachine.freeLookSpeed, deltaTime);
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
        stateMachine.InputReader.InteractEvent -= OnInteract;
        stateMachine.InputReader.DodgeEvent -= OnDodge;
        stateMachine.InputReader.JumpEvent -= OnJump;
        stateMachine.InputReader.SneakEvent -= OnSneak;
    }

    private void OnInteract()
    {
        if (!stateMachine.Interacter.SelectInteraction()) return;
        stateMachine.SwitchState(new PlayerInteractingState(stateMachine));
    }

    private void OnTarget()
    {
        if (!stateMachine.Targeter.SelectTarget()) return;
        stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
    }

    private void OnDodge()
    {
        if (stateMachine.InputReader.MovementValue == Vector2.zero) return;
        stateMachine.SwitchState(new PlayerDodgingState(stateMachine, new Vector2(0f, 1f)));
    }

    private void OnJump()
    {
        stateMachine.SwitchState(new PlayerJumpingState(stateMachine));
    }

    private void OnSneak()
    {
        stateMachine.SwitchState(new PlayerSneakState(stateMachine));
    }
}

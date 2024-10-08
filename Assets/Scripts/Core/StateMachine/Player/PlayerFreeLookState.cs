using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFreeLookState : PlayerLocomotiveState
{
    public PlayerFreeLookState(PlayerStateMachine _stateMachine) : base(_stateMachine) { }

    private const float animatorDampTime = 0.1f;
    private const float gravity = 20.0f;

    public override void Enter() 
    {
        base.Enter();
        stateMachine.animator.CrossFadeInFixedTime(FreeLookBlendTree, CrossFadeDuration);
        stateMachine.InputReader.TargetEvent += OnTarget;
        stateMachine.InputReader.InteractEvent += OnInteract;
        stateMachine.InputReader.SneakEvent += OnSneak;
    }

    public override void Tick(float deltaTime)
    {
        if (stateMachine.InputReader.IsAttacking && (stateMachine.Attributes.currentStamina >= stateMachine.WeaponHandler.mainHandWeapon.staminaReq))
        {
            stateMachine.SwitchState(new PlayerAttackingState(stateMachine, 0));
            return;
        }
        if (stateMachine.InputReader.IsBlocking && stateMachine.canBlock)
        {
            stateMachine.SwitchState(new PlayerBlockingState(stateMachine));
            return;
        }
        Vector3 movement = CalculateMovementDirection();
        Move(movement * stateMachine.freeLookSpeed * stateMachine.encumberanceModifier, deltaTime);
        if (stateMachine.InputReader.MovementValue == Vector2.zero)
        {
            if (stateMachine.animator.GetFloat(speedPercent) > 0.01f) stateMachine.animator.SetFloat(speedPercent, 0, animatorDampTime, deltaTime);
            else stateMachine.animator.SetFloat(speedPercent, 0);
            return;
        }
        FaceMovementDirection(movement, deltaTime);
        float currentSpeed = new Vector2(stateMachine.Controller.velocity.x, stateMachine.Controller.velocity.z).magnitude;
        stateMachine.animator.SetFloat(speedPercent, currentSpeed / stateMachine.freeLookSpeed, animatorDampTime, deltaTime);
    }

    public override void Exit() 
    {
        base.Exit();
        stateMachine.InputReader.TargetEvent -= OnTarget;
        stateMachine.InputReader.InteractEvent -= OnInteract;
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

    private void OnSneak()
    {
        stateMachine.SwitchState(new PlayerSneakState(stateMachine));
    }
}

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
        if (stateMachine.WeaponHandler.currentWeapon.maskLayer > 0)
            stateMachine.animationMask.ApplyLayerWeight(stateMachine.animator, stateMachine.WeaponHandler.currentWeapon.maskLayer, true);
        stateMachine.InputReader.TargetEvent += OnTarget;
        stateMachine.InputReader.InteractEvent += OnInteract;
        stateMachine.InputReader.DodgeEvent += OnDodge;
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
        if (stateMachine.WeaponHandler.currentWeapon.maskLayer > 0)
            stateMachine.animationMask.ApplyLayerWeight(stateMachine.animator, stateMachine.WeaponHandler.currentWeapon.maskLayer, false);
        stateMachine.InputReader.TargetEvent -= OnTarget;
        stateMachine.InputReader.InteractEvent -= OnInteract;
        stateMachine.InputReader.DodgeEvent -= OnDodge;
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

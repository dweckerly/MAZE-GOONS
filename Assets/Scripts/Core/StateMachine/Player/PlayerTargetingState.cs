using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargetingState : PlayerLocomotiveState
{
    private readonly int TargetingBlendTree = Animator.StringToHash("Targeting Blend Tree");
    private readonly int TargetingForward = Animator.StringToHash("TargetingForward");
    private readonly int TargetingRight = Animator.StringToHash("TargetingRight");

    public PlayerTargetingState(PlayerStateMachine _stateMachine) : base(_stateMachine) {}

    public override void Enter()
    {
        base.Enter();
        stateMachine.animator.CrossFadeInFixedTime(TargetingBlendTree, CrossFadeDuration);
        stateMachine.InputReader.TargetEvent += OnTarget;
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
        if (stateMachine.Targeter.CurrentTarget == null)
        {
            ReturnToLocomotion();
            return;
        }
        Vector3 movement = CalculateMovement();
        UpdateAnimator(deltaTime);
        Move(movement * stateMachine.targetingSpeed, deltaTime);
        FaceTarget();
    }

    public override void Exit()
    {
        base.Exit();
        stateMachine.InputReader.TargetEvent -= OnTarget;
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

    private Vector3 CalculateMovement()
    {
        Vector3 movement = new Vector3();
        movement += stateMachine.transform.right * stateMachine.InputReader.MovementValue.x;
        movement += stateMachine.transform.forward * stateMachine.InputReader.MovementValue.y;
        return movement;
    }

    private void OnTarget()
    {
        stateMachine.Targeter.Cancel();
        ReturnToLocomotion();
    }

    protected override void OnDodge()
    {
        if (stateMachine.InputReader.MovementValue == Vector2.zero || stateMachine.Attributes.GetStamina() < stateMachine.dodgeStaminaReq) return;
        stateMachine.SwitchState(new PlayerDodgingState(stateMachine, stateMachine.InputReader.MovementValue));
    }
}

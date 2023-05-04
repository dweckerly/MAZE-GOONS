using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpAttackState : PlayerAttackingState
{
    private readonly int OneHandedAttackHash = Animator.StringToHash("One-Handed-Jump");
    private readonly int TwoHandedAttackHash = Animator.StringToHash("Two-Handed-Jump-Attack");
    private float forceTime = 0.1f;
    private float force = 10f;

    public PlayerJumpAttackState(PlayerStateMachine _stateMachine) : base(_stateMachine) { }

    public override void Enter()
    {
        storedAnimSpeed = stateMachine.animator.speed;
        if (stateMachine.WeaponHandler.mainHandWeapon.oneHanded) stateMachine.animator.CrossFadeInFixedTime(OneHandedAttackHash, CrossFadeDuration);
        else stateMachine.animator.CrossFadeInFixedTime(TwoHandedAttackHash, CrossFadeDuration);
        stateMachine.WeaponHandler.ApplyWeaponMasks(stateMachine.animationMask, stateMachine.animator, false);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);
        float normalizedTime = GetNormalizedAnimationTime(stateMachine.animator);
        if (normalizedTime >= previousFrameTime && normalizedTime < 1)
        {
            if (normalizedTime >= forceTime) TryApplyForce(force);
        }
        if (stateMachine.Controller.isGrounded)
        {
            ReturnToLocomotion();
            return;
        }
        previousFrameTime = normalizedTime;
    }
}

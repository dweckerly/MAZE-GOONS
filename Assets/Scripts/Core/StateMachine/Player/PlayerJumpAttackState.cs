using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpAttackState : PlayerAttackingState
{
    private readonly int AttackHash = Animator.StringToHash("Two-Handed-Jump-Attack");
    private float animationTime = 0.5f;
    private float forceTime = 0.1f;

    public PlayerJumpAttackState(PlayerStateMachine _stateMachine) : base(_stateMachine) { }

    public override void Enter()
    {
        stateMachine.animator.CrossFadeInFixedTime(AttackHash, CrossFadeDuration);
        stateMachine.WeaponHandler.ApplyWeaponMasks(stateMachine.animationMask, stateMachine.animator, false);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);
        float normalizedTime = GetNormalizedAnimationTime(stateMachine.animator);
        if (normalizedTime >= previousFrameTime && normalizedTime < 1)
        {
            if (normalizedTime >= forceTime) TryApplyForce();
        }
        if (stateMachine.Controller.isGrounded)
        {
            if (stateMachine.InputReader.IsAttacking) 
            {
                TryComboAttack(normalizedTime);
            }
            else
            {
                ReturnToLocomotion();
                return;
            }
        }
        previousFrameTime = normalizedTime;
    }
}

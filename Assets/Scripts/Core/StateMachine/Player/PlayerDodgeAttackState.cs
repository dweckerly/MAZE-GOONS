using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodgeAttackState : PlayerAttackingState
{
    private readonly int OneHandedAttackHash = Animator.StringToHash("One-Handed-Dodge-Forward");
    private readonly int TwoHandedAttackHash = Animator.StringToHash("Two-Handed-Dodge-Forward");
    private float animationTime = 0.5f;
    private float forceTime = 0.1f;
    private float force = 20f;

    public PlayerDodgeAttackState(PlayerStateMachine _stateMachine) : base(_stateMachine) {}

    public override void Enter()
    {
        stateMachine.WeaponHandler.ApplyWeaponMasks(stateMachine.animationMask, stateMachine.animator, false);
        if (stateMachine.WeaponHandler.mainHandWeapon.twoHanded)
        {
            stateMachine.animator.CrossFadeInFixedTime(TwoHandedAttackHash, CrossFadeDuration);
            return;
        }
        stateMachine.animator.CrossFadeInFixedTime(OneHandedAttackHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);
        float normalizedTime = GetNormalizedAnimationTime(stateMachine.animator);
        if (normalizedTime >= previousFrameTime && normalizedTime < 1)
        {
            if (normalizedTime >= forceTime) TryApplyForce();
            if (stateMachine.InputReader.IsAttacking) TryComboAttack(normalizedTime);
        }
        else
        {
            ReturnToLocomotion();
        }
        previousFrameTime = normalizedTime;
    }
}

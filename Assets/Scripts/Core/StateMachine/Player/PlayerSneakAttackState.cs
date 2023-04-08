using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSneakAttackState : PlayerAttackingState
{
    private readonly int OneHandedAttackHash = Animator.StringToHash("One-Handed-Sneak-Attack");

    public PlayerSneakAttackState(PlayerStateMachine _stateMachine) : base(_stateMachine) {}

    public override void Enter()
    {
        stateMachine.WeaponHandler.ApplyWeaponMasks(stateMachine.animationMask, stateMachine.animator, false);
        stateMachine.animator.CrossFadeInFixedTime(OneHandedAttackHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);
        float normalizedTime = GetNormalizedAnimationTime(stateMachine.animator);
        if (normalizedTime >= previousFrameTime && normalizedTime < 1)
        {
            if (normalizedTime >= attack.ForceTime) TryApplyForce();
        }
        else
        {
            stateMachine.sneaking = false;
            ReturnToLocomotion();
        }
        previousFrameTime = normalizedTime;
    }
}

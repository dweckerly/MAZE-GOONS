using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodgeAttackState : PlayerBaseState
{
    private readonly int AttackHash = Animator.StringToHash("One-Handed-Dodge-Forward");
    private float previousFrameTime;
    private bool appliedForce = false;

    private float animationTime = 0.5f;
    private float forceTime = 0.1f;
    private float force = 20f;

    public PlayerDodgeAttackState(PlayerStateMachine _stateMachine) : base(_stateMachine) {}

    public override void Enter()
    {
        stateMachine.WeaponHandler.ApplyWeaponMasks(stateMachine.animationMask, stateMachine.animator, false);
        stateMachine.animator.CrossFadeInFixedTime(AttackHash, CrossFadeDuration);
    }

    public override void Exit()
    {
        stateMachine.WeaponHandler.ApplyWeaponMasks(stateMachine.animationMask, stateMachine.animator, true);
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

    private void TryComboAttack(float normalizedTime)
    {
        if (normalizedTime < animationTime) return;
        stateMachine.SwitchState(new PlayerAttackingState(stateMachine, 0));
    }

    private void TryApplyForce()
    {
        if (appliedForce) return;
        stateMachine.ForceReceiver.AddForce(stateMachine.transform.forward * force);
        appliedForce = true;
    }
}

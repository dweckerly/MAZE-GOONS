using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlockingState : PlayerBaseState
{
    public PlayerBlockingState(PlayerStateMachine _stateMachine) : base(_stateMachine) {}

    public override void Enter()
    {
        stateMachine.WeaponHandler.ApplyWeaponMasks(stateMachine.animationMask, stateMachine.animator, false);
        stateMachine.animator.CrossFadeInFixedTime(stateMachine.WeaponHandler.currentWeapon.blockingAnimation, CrossFadeDuration);
        stateMachine.Attributes.SetInvulnerable(true);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);
        if (!stateMachine.InputReader.IsBlocking) ReturnToLocomotion();
    }

    public override void Exit() 
    {
        stateMachine.WeaponHandler.ApplyWeaponMasks(stateMachine.animationMask, stateMachine.animator, true);
        stateMachine.Attributes.SetInvulnerable(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerImpactState : PlayerBaseState
{
    private readonly int ImpactHash = Animator.StringToHash("Impact");
    private float duration = 0.5f;

    public PlayerImpactState(PlayerStateMachine _stateMachine) : base(_stateMachine) {}

    public override void Enter()
    {
        stateMachine.animator.CrossFadeInFixedTime(ImpactHash, CrossFadeDuration);
        if (stateMachine.WeaponHandler.currentWeapon.maskLayer > 0)
            stateMachine.animationMask.ApplyLayerWeight(stateMachine.animator, stateMachine.WeaponHandler.currentWeapon.maskLayer, true);
    }

    public override void Exit() 
    {
        if (stateMachine.WeaponHandler.currentWeapon.maskLayer > 0)
            stateMachine.animationMask.ApplyLayerWeight(stateMachine.animator, stateMachine.WeaponHandler.currentWeapon.maskLayer, false);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);
        duration -= deltaTime;
        if (duration <= 0) ReturnToLocomotion();
    }
}

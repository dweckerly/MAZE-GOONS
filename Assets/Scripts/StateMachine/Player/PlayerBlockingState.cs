using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlockingState : PlayerBaseState
{
    public PlayerBlockingState(PlayerStateMachine _stateMachine) : base(_stateMachine) {}

    public override void Enter()
    {
        stateMachine.animator.CrossFadeInFixedTime(stateMachine.WeaponHandler.currentWeapon.blockingAnimation, CrossFadeDuration);
        stateMachine.Attributes.SetInvulnerable(true);
    }

    public override void Tick(float deltaTime)
    {
        base.Tick(deltaTime);
        Move(deltaTime);
        if (!stateMachine.InputReader.IsBlocking)
        {
            if (stateMachine.Targeter.CurrentTarget == null)
            {
                stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
                return;
            }
            stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
            return;
        }
    }

    public override void Exit() 
    {
        stateMachine.Attributes.SetInvulnerable(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodgingState : PlayerBaseState
{
    private readonly int DodgingBlendTree = Animator.StringToHash("Dodging Blend Tree");
    private readonly int DodgingForward = Animator.StringToHash("DodgingForward");
    private readonly int DodgingRight = Animator.StringToHash("DodgingRight");

    public PlayerDodgingState(PlayerStateMachine _stateMachine) : base(_stateMachine) {}

    public override void Enter()
    {
        stateMachine.animator.CrossFadeInFixedTime(DodgingBlendTree, CrossFadeDuration);
    }

    public override void Exit() {}

    public override void Tick(float deltaTime)
    {
        
        if (stateMachine.Targeter.CurrentTarget != null) stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
        else stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
    }
}

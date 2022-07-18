using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargetingState : PlayerBaseState
{
    private readonly int TargetingBlendTree = Animator.StringToHash("Targeting Blend Tree");
    public PlayerTargetingState(PlayerStateMachine _stateMachine) : base(_stateMachine) {}

    public override void Enter()
    {
        stateMachine.animator.Play(TargetingBlendTree);
        stateMachine.InputReader.CancelEvent += OnCancel;
    }

    public override void Tick(float deltaTime)
    {
        if (stateMachine.Targeter.CurrentTarget == null)
        { 
            stateMachine.SwicthState(new PlayerFreeLookState(stateMachine));
            return;
        }
        FaceTarget();
    }

    public override void Exit()
    {
        stateMachine.InputReader.CancelEvent -= OnCancel;
    }

    private void OnCancel()
    {
        stateMachine.Targeter.Cancel();
        stateMachine.SwicthState(new PlayerFreeLookState(stateMachine));
    }
}

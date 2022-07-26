using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadState : PlayerBaseState
{
    private readonly int DeadHash = Animator.StringToHash("Dead");
    public PlayerDeadState(PlayerStateMachine _stateMachine) : base(_stateMachine) {}

    public override void Enter() 
    {
        stateMachine.animator.SetTrigger(DeadHash);
    }

    public override void Tick(float deltaTime)
    {
        stateMachine.SwitchState(null);
    }

    public override void Exit() {}
}

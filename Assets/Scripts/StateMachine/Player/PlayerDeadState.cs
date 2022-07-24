using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadState : PlayerBaseState
{
    public PlayerDeadState(PlayerStateMachine _stateMachine) : base(_stateMachine) {}

    public override void Enter() 
    {
        //stateMachine.Ragdoll.ToggleRagdoll(true);
        stateMachine.animator.SetTrigger("die");
    }

    public override void Tick(float deltaTime)
    {
        stateMachine.SwitchState(null);
    }

    public override void Exit() {}
}

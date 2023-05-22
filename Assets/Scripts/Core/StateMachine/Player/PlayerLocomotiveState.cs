using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotiveState : PlayerBaseState
{
    public PlayerLocomotiveState(PlayerStateMachine _stateMachine) : base(_stateMachine) {}

    public override void Enter()
    {
        stateMachine.InputReader.DodgeEvent += OnDodge;
        stateMachine.InputReader.JumpEvent += OnJump;
    }

    public override void Exit()
    {
        stateMachine.InputReader.DodgeEvent -= OnDodge;
        stateMachine.InputReader.JumpEvent -= OnJump;
    }

    public override void Tick(float deltaTime) {}

    protected virtual void OnDodge()
    {
        if (stateMachine.InputReader.MovementValue == Vector2.zero 
        || stateMachine.Attributes.GetStamina() < stateMachine.dodgeStaminaReq
        || stateMachine.Inventory.encumbered) return;
        stateMachine.SwitchState(new PlayerDodgingState(stateMachine, new Vector2(0f, 1f)));
    }

    private void OnJump()
    {
        stateMachine.SwitchState(new PlayerJumpingState(stateMachine));
    }
}

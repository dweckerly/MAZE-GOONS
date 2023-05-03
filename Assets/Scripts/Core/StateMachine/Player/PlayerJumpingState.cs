using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpingState : PlayerBaseState
{
    private readonly int JumpHash = Animator.StringToHash("Jump");
    private Vector3 momentum;

    public PlayerJumpingState(PlayerStateMachine _stateMachine) : base(_stateMachine) {}

    public override void Enter()
    {
        stateMachine.sneaking = false;
        stateMachine.ForceReceiver.Jump(stateMachine.JumpForce);
        momentum = stateMachine.Controller.velocity;
        momentum.y = 0;
        stateMachine.animator.CrossFadeInFixedTime(JumpHash, CrossFadeDuration);
    }

    public override void Exit() {}

    public override void Tick(float deltaTime)
    {
        Move(momentum, deltaTime);
        if (stateMachine.InputReader.IsAttacking && (stateMachine.Attributes.currentStamina > stateMachine.WeaponHandler.mainHandWeapon.staminaReq))
        {
            stateMachine.SwitchState(new PlayerJumpAttackState(stateMachine));
            return;
        }
        if (stateMachine.Controller.velocity.y <= 0) 
        {
            stateMachine.SwitchState(new PlayerFallingState(stateMachine));
            return;
        }
        FaceTarget();
    }
}

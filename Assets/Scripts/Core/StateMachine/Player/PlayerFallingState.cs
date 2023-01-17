using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallingState : PlayerBaseState
{
    private readonly int FallHash = Animator.StringToHash("Fall");
    private Vector3 momentum;

    public PlayerFallingState(PlayerStateMachine _stateMachine) : base(_stateMachine) {}

    public override void Enter()
    {
        momentum = stateMachine.Controller.velocity;
        momentum.y = 0;
        stateMachine.animator.CrossFadeInFixedTime(FallHash, CrossFadeDuration);
    }

    public override void Exit() {}

    public override void Tick(float deltaTime)
    {
        Move(momentum, deltaTime);
        if (stateMachine.Controller.isGrounded)
        {
            ReturnToLocomotion();
            return;
        }
        FaceTarget();
    }
}

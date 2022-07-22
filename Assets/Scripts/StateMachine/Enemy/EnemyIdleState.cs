using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    private readonly int LocomotionHash = Animator.StringToHash("Locomotion");
    private readonly int SpeedHash = Animator.StringToHash("Speed");

    private const float AnimationDampTime = 0.2f;

    public EnemyIdleState(EnemyStateMachine _stateMachine) : base(_stateMachine) {}

    public override void Enter()
    {
        stateMachine.animator.CrossFadeInFixedTime(LocomotionHash, CrossFadeDuration);
        stateMachine.animator.SetFloat(SpeedHash, 0);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);
        if (IsInDetectionRange(stateMachine.Player)) return;
        stateMachine.animator.SetFloat(SpeedHash, 0f, AnimationDampTime, deltaTime);
    }

    public override void Exit()
    {
        
    }
}

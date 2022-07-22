using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChasingState : EnemyBaseState
{
    private readonly int LocomotionHash = Animator.StringToHash("Locomotion");
    private readonly int SpeedHash = Animator.StringToHash("Speed");
    private const float AnimationDampTime = 0.2f;

    public EnemyChasingState(EnemyStateMachine _stateMachine) : base(_stateMachine) {}

    public override void Enter()
    {
        stateMachine.animator.CrossFadeInFixedTime(LocomotionHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        if (!IsInDetectionRange(stateMachine.Player)) 
        {
            stateMachine.SwitchState(new EnemyIdleState(stateMachine));
            return;
        }
        MoveToTarget(deltaTime, stateMachine.Player);
        stateMachine.animator.SetFloat(SpeedHash, 1f, AnimationDampTime, deltaTime);
    }

    public override void Exit() 
    {
        stateMachine.Agent.ResetPath();
        stateMachine.Agent.velocity = Vector3.zero;
    }

    private void MoveToTarget(float deltaTime, GameObject target)
    {
        stateMachine.Agent.destination = target.transform.position;
        Move(stateMachine.Agent.desiredVelocity.normalized * stateMachine.MovementSpeed, deltaTime);
        stateMachine.Agent.velocity = stateMachine.Controller.velocity;
    }
}

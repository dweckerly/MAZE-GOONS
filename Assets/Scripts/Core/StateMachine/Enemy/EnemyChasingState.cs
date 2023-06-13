using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChasingState : EnemyBaseState
{
    private readonly int LocomotionHash = Animator.StringToHash("Free Look Blend Tree");
    private readonly int SpeedHash = Animator.StringToHash("speedPercent");
    private const float AnimationDampTime = 0.2f;

    public EnemyChasingState(EnemyStateMachine _stateMachine) : base(_stateMachine) {}

    public override void Enter()
    {
        stateMachine.animator.CrossFadeInFixedTime(LocomotionHash, CrossFadeDuration);
        stateMachine.animator.speed = stateMachine.animSpeed;
    }

    public override void Tick(float deltaTime)
    {
        if (stateMachine.Player != null && stateMachine.playerStateMachine.Attributes.alive)
        {
            if (stateMachine.playerStateMachine.sneaking && !IsInDetectionRange(stateMachine.Player))
            {
                if (stateMachine.PatrolPath != null)
                {
                    stateMachine.SwitchState(new EnemyPatrollingState(stateMachine, 0));
                    return;
                }
                stateMachine.SwitchState(new EnemyIdleState(stateMachine));
                return;
            }
            if (IsInAttackRange(stateMachine.Player))
            {
                stateMachine.SwitchState(new EnemyFightingState(stateMachine));
                return;
            }
            MoveToTarget(deltaTime, stateMachine.Player);
            FaceTarget(stateMachine.Player);
            stateMachine.animator.SetFloat(SpeedHash, 1f, AnimationDampTime, deltaTime);
            return;
        }
        stateMachine.SwitchState(new EnemyIdleState(stateMachine));
        return;
    }

    public override void Exit() 
    {
        stateMachine.Agent.ResetPath();
        stateMachine.Agent.velocity = Vector3.zero;
        stateMachine.animator.speed = 1f;
    }

    private void MoveToTarget(float deltaTime, GameObject target)
    {
        if (stateMachine.Agent.isOnNavMesh)
        {
            stateMachine.Agent.destination = target.transform.position;
            Move(stateMachine.Agent.desiredVelocity.normalized * stateMachine.MovementSpeed, deltaTime);
        }
        stateMachine.Agent.velocity = stateMachine.Controller.velocity;
    }
}

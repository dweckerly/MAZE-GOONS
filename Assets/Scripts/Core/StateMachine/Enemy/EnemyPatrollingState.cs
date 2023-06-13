using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrollingState : EnemyBaseState
{
    private readonly int LocomotionHash = Animator.StringToHash("Free Look Blend Tree");
    private readonly int SpeedHash = Animator.StringToHash("speedPercent");
    private const float AnimationDampTime = 0.2f;

    private Transform wayPoint;
    private int wayPointIndex;

    public EnemyPatrollingState(EnemyStateMachine _stateMachine, int _wayPointIndex) : base(_stateMachine) 
    {
        wayPointIndex = _wayPointIndex;
        wayPoint = _stateMachine.PatrolPath.WayPoints[wayPointIndex];
    }

    public override void Enter()
    {
        stateMachine.animator.CrossFadeInFixedTime(LocomotionHash, CrossFadeDuration);
        stateMachine.animator.speed = stateMachine.animSpeed;
    }

    public override void Tick(float deltaTime)
    {
        if (stateMachine.Player != null && stateMachine.playerStateMachine.Attributes.alive)
        {
            if (IsInDetectionRange(stateMachine.Player))
            {
                stateMachine.AlertNearbyEnemies();
                stateMachine.SwitchState(new EnemyChasingState(stateMachine));
                return;
            }
        }
        if (IsInAttackRange(wayPoint.gameObject))
        {
            if (wayPointIndex + 1 >= stateMachine.PatrolPath.WayPoints.Count)
            {
                stateMachine.SwitchState(new EnemyPatrollingState(stateMachine, 0));
                return;
            }
            stateMachine.SwitchState(new EnemyPatrollingState(stateMachine, wayPointIndex + 1));
            return;
        }
        stateMachine.animator.SetFloat(SpeedHash, 0f, AnimationDampTime, deltaTime);
        WalkToTarget(deltaTime, wayPoint.gameObject);
        FaceTarget(wayPoint.gameObject);
        stateMachine.animator.SetFloat(SpeedHash, 1f, AnimationDampTime, deltaTime);
    }

    public override void Exit()
    {
        stateMachine.Agent.ResetPath();
        stateMachine.Agent.velocity = Vector3.zero;
        stateMachine.animator.speed = 1f;
    }

    private void WalkToTarget(float deltaTime, GameObject target)
    {
        if (stateMachine.Agent.isOnNavMesh)
        {
            stateMachine.Agent.destination = target.transform.position;
            Move(stateMachine.Agent.desiredVelocity.normalized * stateMachine.MovementSpeed / 2, deltaTime);
        }
        stateMachine.Agent.velocity = stateMachine.Controller.velocity;
    }
}

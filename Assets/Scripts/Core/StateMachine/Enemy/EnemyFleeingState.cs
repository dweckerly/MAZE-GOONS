using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFleeingState : EnemyBaseState
{
    private readonly int LocomotionHash = Animator.StringToHash("Fleeing Blend Tree");
    private readonly int SpeedHash = Animator.StringToHash("speedPercent");
    private const float AnimationDampTime = 0.2f;

    public EnemyFleeingState(EnemyStateMachine _stateMachine) : base(_stateMachine) {}

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
                stateMachine.SwitchState(new EnemyIdleState(stateMachine));
                return;
            }
            FleeFromTarget(deltaTime, stateMachine.Player);
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

    float vRotation = 0f;
    private void FleeFromTarget(float deltaTime, GameObject target)
    {
        if (stateMachine.Agent.isOnNavMesh)
        {
            bool isDirectionSafe = false;
            while (!isDirectionSafe)
            {
                Vector3 directionToPlayer = stateMachine.gameObject.transform.position - target.transform.position;
                Vector3 newPosition = stateMachine.gameObject.transform.position + directionToPlayer;
                newPosition = Quaternion.Euler(0, vRotation, 0) * newPosition;
                bool isHit = Physics.Raycast(stateMachine.gameObject.transform.position, newPosition, out RaycastHit hit, 3f);
                if (isHit && hit.transform.CompareTag("Wall"))
                {
                    vRotation += 20;
                    isDirectionSafe = false;
                }
                else
                {
                    stateMachine.Agent.destination = newPosition;
                    Move(stateMachine.Agent.desiredVelocity.normalized * stateMachine.MovementSpeed, deltaTime);
                    stateMachine.gameObject.transform.rotation = Quaternion.LookRotation(newPosition);
                    isDirectionSafe = true;
                }
            }
        }
        stateMachine.Agent.velocity = stateMachine.Controller.velocity;
    }
}

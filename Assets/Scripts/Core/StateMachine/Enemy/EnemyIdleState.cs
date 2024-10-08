using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    private readonly int LocomotionHash = Animator.StringToHash("Free Look Blend Tree");
    private readonly int StareHash = Animator.StringToHash("Sun-stare");
    private readonly int SpeedHash = Animator.StringToHash("speedPercent");
    private const float AnimationDampTime = 0.2f;

    public EnemyIdleState(EnemyStateMachine _stateMachine) : base(_stateMachine) {}

    public override void Enter()
    {
        if (stateMachine.enemyType == EnemyType.Worshipper) stateMachine.animator.CrossFadeInFixedTime(StareHash, CrossFadeDuration);
        else stateMachine.animator.CrossFadeInFixedTime(LocomotionHash, CrossFadeDuration);
        stateMachine.animator.speed = stateMachine.animSpeed;
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);
        if (stateMachine.Player != null && stateMachine.playerStateMachine.Attributes.alive)
        {
            if (IsInDetectionRange(stateMachine.Player))
            {
                stateMachine.AlertNearbyEnemies();
                stateMachine.SwitchState(new EnemyChasingState(stateMachine));
            } 
        }
        stateMachine.animator.SetFloat(SpeedHash, 0f, AnimationDampTime, deltaTime);
    }

    public override void Exit() 
    {
        stateMachine.animator.speed = 1f;
    }
}

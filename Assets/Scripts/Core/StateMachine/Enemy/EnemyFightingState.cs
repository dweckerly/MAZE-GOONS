using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFightingState : EnemyBaseState
{
    private readonly int FightingIdleHash = Animator.StringToHash("Targeting Blend Tree");

    public EnemyFightingState(EnemyStateMachine _stateMachine) : base(_stateMachine) {}

    public override void Enter()
    {
        stateMachine.animator.CrossFadeInFixedTime(FightingIdleHash, CrossFadeDuration);
        stateMachine.animator.speed = stateMachine.animSpeed;
    }

    public override void Exit() 
    {
        stateMachine.animator.speed = 1f;
    }

    public override void Tick(float deltaTime)
    {
        if (stateMachine.Player != null && stateMachine.playerStateMachine.Attributes.alive)
        {
            if (stateMachine.Attributes.GetHPFraction() < 0.5f)
            {
                stateMachine.SwitchState(new EnemyFleeingState(stateMachine));
                return;
            }
            FaceTarget(stateMachine.Player);
            if (!IsInAttackRange(stateMachine.Player))
            {
                stateMachine.SwitchState(new EnemyChasingState(stateMachine));
                return;
            }
            stateMachine.SwitchState(new EnemyAttackingState(stateMachine, 0));
            return;
        }
        stateMachine.SwitchState(new EnemyIdleState(stateMachine));
        return;
    }
}

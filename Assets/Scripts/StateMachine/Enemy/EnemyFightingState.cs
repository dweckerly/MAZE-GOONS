using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFightingState : EnemyBaseState
{
    private readonly int FightingIdleHash = Animator.StringToHash("FightingIdle");

    public EnemyFightingState(EnemyStateMachine _stateMachine) : base(_stateMachine) {}

    public override void Enter()
    {
        stateMachine.animator.CrossFadeInFixedTime(FightingIdleHash, CrossFadeDuration);
    }

    public override void Exit()
    {
        
    }

    public override void Tick(float deltaTime)
    {
        if (!IsInAttackRange(stateMachine.Player))
        {
            stateMachine.SwitchState(new EnemyChasingState(stateMachine));
            return;
        }
        else 
        {
            stateMachine.SwitchState(new EnemyAttackingState(stateMachine, 0));
        }
    }
}

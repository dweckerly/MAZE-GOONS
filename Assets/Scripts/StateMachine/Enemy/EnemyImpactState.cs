using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyImpactState : EnemyBaseState
{
    private readonly int ImpactHash = Animator.StringToHash("Impact");
    private float duration = 1f;

    public EnemyImpactState(EnemyStateMachine _stateMachine) : base(_stateMachine) { }

    public override void Enter()
    {
        stateMachine.animator.CrossFadeInFixedTime(ImpactHash, CrossFadeDuration);
    }

    public override void Exit() {}

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);
        duration -= deltaTime;
        if (duration <= 0f) stateMachine.SwitchState(new EnemyFightingState(stateMachine));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCoweringState : EnemyBaseState
{
    private readonly int CowerHash = Animator.StringToHash("Cower");

    public EnemyCoweringState(EnemyStateMachine _stateMachine) : base(_stateMachine) {}

    public override void Enter()
    {
        stateMachine.animator.CrossFadeInFixedTime(CowerHash, CrossFadeDuration);
        stateMachine.animator.speed = stateMachine.animSpeed;
    }

    public override void Exit() {}

    public override void Tick(float deltaTime) 
    {
        Move(deltaTime);
    }
}

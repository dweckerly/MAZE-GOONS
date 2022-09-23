using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySunStareState : EnemyBaseState
{
    private readonly int StareHash = Animator.StringToHash("Sun-stare");

    public EnemySunStareState(EnemyStateMachine _stateMachine) : base(_stateMachine) {}

    public override void Enter()
    {
        stateMachine.animator.CrossFadeInFixedTime(StareHash, CrossFadeDuration);
        stateMachine.animator.speed = stateMachine.animSpeed;
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);
    }

    public override void Exit() {}    
}

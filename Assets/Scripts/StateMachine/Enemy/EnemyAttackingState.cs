using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackingState : EnemyBaseState
{
    private readonly int AttackHash = Animator.StringToHash("Attack-Unarmed");
    public EnemyAttackingState(EnemyStateMachine _stateMachine) : base(_stateMachine) {}

    public override void Enter()
    {
        stateMachine.animator.CrossFadeInFixedTime(AttackHash, CrossFadeDuration);
    }

    public override void Exit() {}

    public override void Tick(float deltaTime)
    {
        
    }
}

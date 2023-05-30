using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBlockingState : EnemyBaseState
{
    private const string SHIELD_BLOCK = "Block-Shield";
    private float blockMaxDuration = 3f;
    private float blockMinDuration = 1f;
    private float remainingBlockTime;
    public EnemyBlockingState(EnemyStateMachine _stateMachine) : base(_stateMachine) {}

    public override void Enter()
    {
        stateMachine.Attributes.isBlocking = true;
        stateMachine.animator.CrossFadeInFixedTime(SHIELD_BLOCK, CrossFadeDuration);
        stateMachine.WeaponHandler.ApplyWeaponMasks(stateMachine.animationMask, stateMachine.animator, false);
        remainingBlockTime = Random.Range(blockMinDuration, blockMaxDuration);
    }

    public override void Exit()
    {
        stateMachine.WeaponHandler.ApplyWeaponMasks(stateMachine.animationMask, stateMachine.animator, true);
        stateMachine.Attributes.isBlocking = false;
    }

    public override void Tick(float deltaTime)
    {
        remainingBlockTime -= deltaTime;
        if (remainingBlockTime <= 0f) stateMachine.SwitchState(new EnemyFightingState(stateMachine));
    }
}

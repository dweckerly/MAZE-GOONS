using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackingState : PlayerBaseState
{
    private float previousFrameTime;
    private Attack attack;

    public PlayerAttackingState(PlayerStateMachine _stateMachine, int attackIndex) : base(_stateMachine) 
    {
        attack = stateMachine.Attacks[attackIndex];
    }

    public override void Enter()
    {
        stateMachine.animator.CrossFadeInFixedTime(attack.AnimationName, attack.TransitionDuration);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);
        FaceTarget();
        float normalizedTime = GetNormalizedAnimationTime();
        if (normalizedTime > previousFrameTime && normalizedTime < 1)
        {
            if (stateMachine.InputReader.IsAttacking)
            {
                TryComboAttack(normalizedTime);
            }
        }
        else
        {
            // go back to locomotion
        }

        previousFrameTime = normalizedTime;
    }

    private void TryComboAttack(float normalizedTime)
    {
        if (attack.ComboStateIndex == -1) return;
        if (normalizedTime < attack.ComboAttackTime) return;
        stateMachine.SwicthState(new PlayerAttackingState(stateMachine, attack.ComboStateIndex));
    }

    public override void Exit()
    {
        
    }

    private float GetNormalizedAnimationTime()
    {
        AnimatorStateInfo currentInfo = stateMachine.animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextInfo = stateMachine.animator.GetNextAnimatorStateInfo(0);

        if (stateMachine.animator.IsInTransition(0) && nextInfo.IsTag("Attack")) return nextInfo.normalizedTime;
        else if (!stateMachine.animator.IsInTransition(0) && currentInfo.IsTag("Attack")) return currentInfo.normalizedTime;
        return 0f;
    }
}

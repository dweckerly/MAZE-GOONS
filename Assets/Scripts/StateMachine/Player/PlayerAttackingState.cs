using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackingState : PlayerBaseState
{
    private float previousFrameTime;
    private Attack attack;
    private bool appliedForce = false;

    public PlayerAttackingState(PlayerStateMachine _stateMachine, int attackIndex) : base(_stateMachine) 
    {
        attack = stateMachine.WeaponHandler.currentWeapon.Attacks[attackIndex];
        stateMachine.WeaponHandler.currentWeapon.SetAdditiveDamageModifier(stateMachine.Attributes.GetStat(Attribute.Brawn));
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
        if (normalizedTime >= previousFrameTime && normalizedTime < 1)
        {
            if (normalizedTime >= attack.ForceTime) TryApplyForce();
            if (stateMachine.InputReader.IsAttacking)
            {
                TryComboAttack(normalizedTime);
            }
        }
        else
        {
            if (stateMachine.Targeter.CurrentTarget != null)
            {
                stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
            }
            else
            {
                stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
            }

        }
        previousFrameTime = normalizedTime;
    }

    public override void Exit() {}

    private void TryComboAttack(float normalizedTime)
    {
        if (attack.ComboStateIndex == -1) return;
        if (normalizedTime < attack.ComboAttackTime) return;
        stateMachine.SwitchState(new PlayerAttackingState(stateMachine, attack.ComboStateIndex));
    }

    private void TryApplyForce()
    {
        if (appliedForce) return;
        stateMachine.ForceReceiver.AddForce(stateMachine.transform.forward * attack.Force);
        appliedForce = true;
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

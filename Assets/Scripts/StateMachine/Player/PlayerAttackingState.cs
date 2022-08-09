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
    }

    public override void Enter()
    {
        stateMachine.animationMask.ApplyLayerWeight(stateMachine.animator, stateMachine.WeaponHandler.currentWeapon.maskLayer, false);
        if (attack.RightHand)
        {
            stateMachine.WeaponHandler.mainHandDamage.SetAdditiveDamageModifier(stateMachine.Attributes.GetStat(Attribute.Brawn));
            stateMachine.WeaponHandler.mainHandDamage.knockback = attack.Knockback;
            stateMachine.WeaponHandler.mainHandDamage.ClearColliderList();
            stateMachine.WeaponHandler.EnableRightHandCollider();
        }
        else 
        {
            stateMachine.WeaponHandler.offHandDamage.SetAdditiveDamageModifier(stateMachine.Attributes.GetStat(Attribute.Brawn));
            stateMachine.WeaponHandler.offHandDamage.knockback = attack.Knockback;
            stateMachine.WeaponHandler.offHandDamage.ClearColliderList();
            stateMachine.WeaponHandler.EnableLeftHandCollider();
        }
        stateMachine.animator.CrossFadeInFixedTime(attack.AnimationName, attack.TransitionDuration);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);
        float normalizedTime = GetNormalizedAnimationTime(stateMachine.animator);
        if (normalizedTime >= previousFrameTime && normalizedTime < 1)
        {
            if (normalizedTime >= attack.ForceTime) TryApplyForce();
            if (stateMachine.InputReader.IsAttacking) TryComboAttack(normalizedTime);
        }
        else
        {
            if (stateMachine.Targeter.CurrentTarget != null) stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
            else stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
        }
        previousFrameTime = normalizedTime;
    }

    public override void Exit() 
    {
        stateMachine.animationMask.ApplyLayerWeight(stateMachine.animator, stateMachine.WeaponHandler.currentWeapon.maskLayer, true);
        if (attack.RightHand) stateMachine.WeaponHandler.DisableRightHandCollider();
        else stateMachine.WeaponHandler.DisableLeftHandCollider();
    }

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
}

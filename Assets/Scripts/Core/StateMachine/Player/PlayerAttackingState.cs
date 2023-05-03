using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackingState : PlayerBaseState
{
    protected float previousFrameTime;
    protected Attack attack;
    protected bool appliedForce = false;

    public PlayerAttackingState(PlayerStateMachine _stateMachine) : base(_stateMachine) { }

    public PlayerAttackingState(PlayerStateMachine _stateMachine, int attackIndex) : base(_stateMachine) 
    {
        attack = stateMachine.WeaponHandler.mainHandWeapon.Attacks[attackIndex];
        if (stateMachine.WeaponHandler.mainHandWeapon.weight < stateMachine.Attributes.currentBrawn) 
            stateMachine.animator.speed = 1 + (stateMachine.Attributes.currentGuile / 10);
    }

    public override void Enter()
    {
        stateMachine.sneaking = false;
        if (attack.RightHand)
        {
            stateMachine.Attributes.SpendStamina(stateMachine.WeaponHandler.mainHandWeapon.staminaReq);
            stateMachine.WeaponHandler.mainHandDamage.SetAdditiveDamageModifier(stateMachine.Attributes.GetStat(Attribute.Brawn));
            stateMachine.WeaponHandler.mainHandDamage.knockback = attack.Knockback;
            stateMachine.WeaponHandler.mainHandDamage.ClearColliderList();
        }
        else 
        {
            stateMachine.Attributes.SpendStamina(stateMachine.WeaponHandler.offHandWeapon.staminaReq);
            stateMachine.WeaponHandler.offHandDamage.SetAdditiveDamageModifier(stateMachine.Attributes.GetStat(Attribute.Brawn));
            stateMachine.WeaponHandler.offHandDamage.knockback = attack.Knockback;
            stateMachine.WeaponHandler.offHandDamage.ClearColliderList();
        }
        stateMachine.animator.CrossFadeInFixedTime(attack.AnimationName, attack.TransitionDuration);
        stateMachine.WeaponHandler.ApplyWeaponMasks(stateMachine.animationMask, stateMachine.animator, false);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);
        float normalizedTime = GetNormalizedAnimationTime(stateMachine.animator);
        if (normalizedTime >= previousFrameTime && normalizedTime < 1)
        {
            if (normalizedTime >= attack.ForceTime) TryApplyForce(attack.Force);
            if (stateMachine.InputReader.IsAttacking) TryComboAttack(normalizedTime);
        }
        else
        {
            ReturnToLocomotion();
        }
        previousFrameTime = normalizedTime;
    }

    public override void Exit() 
    {
        stateMachine.WeaponHandler.ApplyWeaponMasks(stateMachine.animationMask, stateMachine.animator, true);
    }

    protected void TryComboAttack(float normalizedTime)
    {
        if (attack.ComboStateIndex == -1) return;
        if (normalizedTime < attack.ComboAttackTime) return;
        stateMachine.SwitchState(new PlayerAttackingState(stateMachine, attack.ComboStateIndex));
    }

    protected void TryApplyForce(float force = 20f)
    {
        if (appliedForce) return;
        stateMachine.ForceReceiver.AddForce(stateMachine.transform.forward * force);
        appliedForce = true;
    }
}

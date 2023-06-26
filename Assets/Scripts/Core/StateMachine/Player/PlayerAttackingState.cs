using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackingState : PlayerBaseState
{
    protected float previousFrameTime;
    protected Attack attack;
    protected bool appliedForce = false;
    protected float storedAnimSpeed;

    public PlayerAttackingState(PlayerStateMachine _stateMachine) : base(_stateMachine) { }

    public PlayerAttackingState(PlayerStateMachine _stateMachine, int attackIndex) : base(_stateMachine) 
    {
        attack = stateMachine.WeaponHandler.mainHandWeapon.Attacks[attackIndex];
        storedAnimSpeed = stateMachine.animator.speed;
        // if (stateMachine.WeaponHandler.mainHandWeapon.weight < stateMachine.Attributes.currentBrawn) 
        //     stateMachine.animator.speed = 1 + Mathf.Clamp((stateMachine.Attributes.currentGuile - stateMachine.WeaponHandler.mainHandWeapon.weight) / 10, 0, 1);
    }

    public override void Enter()
    {
        stateMachine.sneaking = false;
        //float comboMultiplier = Mathf.Abs((attack.ComboStateIndex - 1) / 2) + 1;
        float comboMultiplier = attack.ComboStateIndex > 1 || attack.ComboStateIndex < 0 ? 1.5f : 1f;
        if (attack.RightHand)
        {
            stateMachine.Attributes.SpendStamina(stateMachine.WeaponHandler.mainHandWeapon.staminaReq);
            if (stateMachine.WeaponHandler.mainHandWeapon.twoHanded)
                stateMachine.WeaponHandler.mainHandDamage.SetAdditiveDamageModifier(Mathf.RoundToInt(stateMachine.Attributes.GetStat(Attribute.Brawn) * 1.5f) + attack.AdditiveDamage);
            else stateMachine.WeaponHandler.mainHandDamage.SetAdditiveDamageModifier(stateMachine.Attributes.GetStat(Attribute.Brawn) + attack.AdditiveDamage);
            stateMachine.WeaponHandler.mainHandDamage.SetMultiplicativeDamageModifier(comboMultiplier);
            stateMachine.WeaponHandler.mainHandDamage.knockback = attack.Knockback;
            stateMachine.WeaponHandler.mainHandDamage.ClearColliderList();
        }
        else 
        {
            stateMachine.Attributes.SpendStamina(stateMachine.WeaponHandler.offHandWeapon.staminaReq);
            stateMachine.WeaponHandler.offHandDamage.SetAdditiveDamageModifier(stateMachine.Attributes.GetStat(Attribute.Brawn) + attack.AdditiveDamage);
            stateMachine.WeaponHandler.offHandDamage.SetMultiplicativeDamageModifier(comboMultiplier);
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
        stateMachine.animator.speed = storedAnimSpeed;
        stateMachine.WeaponHandler.ApplyWeaponMasks(stateMachine.animationMask, stateMachine.animator, true);
    }

    protected void TryComboAttack(float normalizedTime)
    {
        if (attack.ComboStateIndex == -1) return;
        if (normalizedTime < attack.ComboAttackTime) return;
        if (stateMachine.Attributes.currentStamina < stateMachine.WeaponHandler.mainHandWeapon.staminaReq) return;
        stateMachine.SwitchState(new PlayerAttackingState(stateMachine, attack.ComboStateIndex));
    }

    protected void TryApplyForce(float force = 20f)
    {
        if (appliedForce) return;
        stateMachine.ForceReceiver.AddForce(stateMachine.transform.forward * force);
        appliedForce = true;
    }
}

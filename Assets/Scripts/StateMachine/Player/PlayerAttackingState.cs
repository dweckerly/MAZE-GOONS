using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackingState : PlayerBaseState
{
    private float previousFrameTime;
    private Attack attack;
    private Weapon weapon;
    private bool appliedForce = false;

    public PlayerAttackingState(PlayerStateMachine _stateMachine, int attackIndex) : base(_stateMachine) 
    {
        attack = stateMachine.WeaponHandler.currentWeapon.Attacks[attackIndex];
        weapon = stateMachine.WeaponHandler.currentWeapon;
        weapon.DisableRightHand();
        weapon.weaponPrefab.GetComponent<WeaponDamage>().SetAdditiveDamageModifier(stateMachine.Attributes.GetStat(Attribute.Brawn));
        if (weapon.offHandPrefab != null)
        {
            weapon.DisableLeftHand();
            weapon.offHandPrefab.GetComponent<WeaponDamage>().SetAdditiveDamageModifier(stateMachine.Attributes.GetStat(Attribute.Brawn));
        }
    }

    public override void Enter()
    {
        stateMachine.animator.CrossFadeInFixedTime(attack.AnimationName, attack.TransitionDuration);
    }

    public override void Tick(float deltaTime)
    {
        base.Tick(deltaTime);
        Move(deltaTime);
        FaceTarget();
        float normalizedTime = GetNormalizedAnimationTime(stateMachine.animator);
        if (normalizedTime >= previousFrameTime && normalizedTime < 1)
        {
            if (attack.RightHand)
            {
                weapon.weaponPrefab.GetComponent<WeaponDamage>().knockback = attack.Knockback;
                weapon.EnableRightHand();
            } 
            else 
            {
                weapon.offHandPrefab.GetComponent<WeaponDamage>().knockback = attack.Knockback;
                weapon.EnableLeftHand();
            }
            if (normalizedTime >= attack.ForceTime) TryApplyForce();
            if (stateMachine.InputReader.IsAttacking)
            {
                TryComboAttack(normalizedTime);
            }
        }
        else
        {
            if (attack.RightHand) weapon.DisableRightHand();
            else weapon.DisableLeftHand();
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

    public override void Exit() 
    {
        weapon.DisableRightHand();
        if (weapon.offHandPrefab != null) weapon.DisableLeftHand();
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

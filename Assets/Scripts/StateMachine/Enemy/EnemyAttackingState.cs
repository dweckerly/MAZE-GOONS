using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackingState : EnemyBaseState
{
    private Attack attack;
    private Weapon weapon;
    private bool appliedForce = false;
    private float previousFrameTime;

    public EnemyAttackingState(EnemyStateMachine _stateMachine, int attackIndex) : base(_stateMachine) 
    {
        attack = stateMachine.WeaponHandler.currentWeapon.Attacks[attackIndex];
        weapon = stateMachine.WeaponHandler.currentWeapon;
        //weapon.mainHandDamage.SetAdditiveDamageModifier(stateMachine.Attributes.GetStat(Attribute.Brawn));
        //weapon.mainHandDamage.knockback = attack.Knockback;
        if (weapon.offHandPrefab != null)
        {
            weapon.offHandDamage.SetAdditiveDamageModifier(stateMachine.Attributes.GetStat(Attribute.Brawn));
            weapon.offHandDamage.knockback = attack.Knockback;
        }
    }

    public override void Enter()
    {
        stateMachine.WeaponHandler.ClearWeaponColliderHistory(attack.RightHand);
        stateMachine.WeaponHandler.EnableRightHandCollider();
        if (stateMachine.WeaponHandler.offHandPrefab != null) stateMachine.WeaponHandler.EnableLeftHandCollider();
        stateMachine.animator.CrossFadeInFixedTime(attack.AnimationName, CrossFadeDuration);
    }

    public override void Exit() 
    {
        stateMachine.WeaponHandler.DisableRightHandCollider();
        if (stateMachine.WeaponHandler.offHandPrefab != null) stateMachine.WeaponHandler.DisableLeftHandCollider();
    }

    public override void Tick(float deltaTime)
    {
        float normalizedTime = GetNormalizedAnimationTime(stateMachine.animator);
        if (normalizedTime >= previousFrameTime && normalizedTime < 1)
        {
            if (normalizedTime >= attack.ForceTime) TryApplyForce();
        }
        previousFrameTime = normalizedTime;
        if (normalizedTime >= 1) stateMachine.SwitchState(new EnemyFightingState(stateMachine));
    }

    private void TryApplyForce()
    {
        if (appliedForce) return;
        stateMachine.ForceReceiver.AddForce(stateMachine.transform.forward * attack.Force);
        appliedForce = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileAttackingState : EnemyBaseState
{
    private Attack attack;
    private bool appliedForce = false;
    private float previousFrameTime;

    public EnemyProjectileAttackingState(EnemyStateMachine _stateMachine, int attackIndex) : base(_stateMachine)
    {
        attack = stateMachine.WeaponHandler.mainHandWeapon.Attacks[0];
    }

    public override void Enter()
    {
        stateMachine.WeaponHandler.ApplyWeaponMasks(stateMachine.animationMask, stateMachine.animator, false);
        stateMachine.WeaponHandler.mainHandDamage.SetAdditiveDamageModifier(stateMachine.Attributes.GetStat(Attribute.Guile));
        stateMachine.WeaponHandler.mainHandDamage.knockback = attack.Knockback;
        stateMachine.WeaponHandler.mainHandDamage.ClearColliderList();
        stateMachine.animator.CrossFadeInFixedTime(attack.AnimationName, CrossFadeDuration);
    }

    public override void Exit()
    {
        stateMachine.WeaponHandler.ApplyWeaponMasks(stateMachine.animationMask, stateMachine.animator, true);
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

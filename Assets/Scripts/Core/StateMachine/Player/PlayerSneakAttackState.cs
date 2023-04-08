using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSneakAttackState : PlayerBaseState
{
    private float previousFrameTime;
    private Attack attack;
    private bool appliedForce = false;

    public PlayerSneakAttackState(PlayerStateMachine _stateMachine, int attackIndex) : base(_stateMachine)
    {
        attack = stateMachine.WeaponHandler.mainHandWeapon.Attacks[attackIndex];
    }

    public override void Enter()
    {
        stateMachine.sneaking = false;
        if (attack.RightHand)
        {
            stateMachine.WeaponHandler.mainHandDamage.SetAdditiveDamageModifier(stateMachine.Attributes.GetStat(Attribute.Brawn));
            stateMachine.WeaponHandler.mainHandDamage.knockback = attack.Knockback;
            stateMachine.WeaponHandler.mainHandDamage.ClearColliderList();
        }
        else
        {
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
            if (normalizedTime >= attack.ForceTime) TryApplyForce();
        }
        else
        {
            stateMachine.sneaking = false;
            ReturnToLocomotion();
        }
        previousFrameTime = normalizedTime;
    }

    public override void Exit()
    {
        stateMachine.WeaponHandler.ApplyWeaponMasks(stateMachine.animationMask, stateMachine.animator, true);
    }

    private void TryApplyForce()
    {
        if (appliedForce) return;
        stateMachine.ForceReceiver.AddForce(stateMachine.transform.forward * attack.Force);
        appliedForce = true;
    }
}

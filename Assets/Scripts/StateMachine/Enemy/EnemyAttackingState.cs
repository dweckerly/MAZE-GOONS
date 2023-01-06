using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackingState : EnemyBaseState
{
    private Attack attack;
    private bool appliedForce = false;
    private float previousFrameTime;

    public EnemyAttackingState(EnemyStateMachine _stateMachine, int attackIndex) : base(_stateMachine) 
    {
        int atkIndex = Random.Range(0, stateMachine.WeaponHandler.currentWeapon.Attacks.Length);
        attack = stateMachine.WeaponHandler.currentWeapon.Attacks[atkIndex];
    }

    public override void Enter()
    {
        stateMachine.WeaponHandler.ApplyWeaponMasks(stateMachine.animationMask, stateMachine.animator, false);
        if (attack.RightHand)
        {
            stateMachine.WeaponHandler.mainHandDamage.SetAdditiveDamageModifier(stateMachine.Attributes.GetStat(Attribute.Brawn));
            stateMachine.WeaponHandler.mainHandDamage.knockback = attack.Knockback;
            stateMachine.WeaponHandler.mainHandDamage.ClearColliderList();
            //stateMachine.WeaponHandler.EnableRightHandCollider();
        }
        else
        {
            stateMachine.WeaponHandler.offHandDamage.SetAdditiveDamageModifier(stateMachine.Attributes.GetStat(Attribute.Brawn));
            stateMachine.WeaponHandler.offHandDamage.knockback = attack.Knockback;
            stateMachine.WeaponHandler.offHandDamage.ClearColliderList();
            //stateMachine.WeaponHandler.EnableLeftHandCollider();
        }
        stateMachine.animator.CrossFadeInFixedTime(attack.AnimationName, CrossFadeDuration);
    }

    public override void Exit() 
    {
        stateMachine.WeaponHandler.ApplyWeaponMasks(stateMachine.animationMask, stateMachine.animator, true);
        //if (attack.RightHand) stateMachine.WeaponHandler.DisableRightHandCollider();
        //else stateMachine.WeaponHandler.DisableLeftHandCollider();
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

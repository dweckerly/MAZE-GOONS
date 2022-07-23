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
        weapon.weaponPrefab.GetComponent<WeaponDamage>().SetAdditiveDamageModifier(stateMachine.Attributes.GetStat(Attribute.Brawn));
        weapon.weaponPrefab.GetComponent<WeaponDamage>().knockback = attack.Knockback;
        weapon.DisableRightHand();
        if (weapon.offHandPrefab != null) 
        {
            weapon.offHandPrefab.GetComponent<WeaponDamage>().SetAdditiveDamageModifier(stateMachine.Attributes.GetStat(Attribute.Brawn));
            weapon.offHandPrefab.GetComponent<WeaponDamage>().knockback = attack.Knockback;
            weapon.DisableLeftHand();
        }
    }

    public override void Enter()
    {
        stateMachine.animator.CrossFadeInFixedTime(attack.AnimationName, CrossFadeDuration);
    }

    public override void Exit() {}

    public override void Tick(float deltaTime)
    {
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

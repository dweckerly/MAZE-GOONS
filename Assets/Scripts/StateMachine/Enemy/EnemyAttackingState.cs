using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackingState : EnemyBaseState
{
    private Attack attack;
    private Weapon weapon;
    private bool appliedForce = false;
    public EnemyAttackingState(EnemyStateMachine _stateMachine, int attackIndex) : base(_stateMachine) 
    {
        attack = stateMachine.WeaponHandler.currentWeapon.Attacks[attackIndex];
        weapon = stateMachine.WeaponHandler.currentWeapon;
        weapon.DisableRightHand();
        if (weapon.offHandPrefab != null) 
        {
            weapon.DisableLeftHand();
            weapon.offHandPrefab?.GetComponent<WeaponDamage>().SetAdditiveDamageModifier(stateMachine.Attributes.GetStat(Attribute.Brawn));
        }
        weapon.weaponPrefab.GetComponent<WeaponDamage>().SetAdditiveDamageModifier(stateMachine.Attributes.GetStat(Attribute.Brawn));
    }

    public override void Enter()
    {
        stateMachine.animator.CrossFadeInFixedTime(attack.AnimationName, CrossFadeDuration);
    }

    public override void Exit() {}

    public override void Tick(float deltaTime)
    {
        
    }
}

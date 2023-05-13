using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlockingState : PlayerBaseState
{
    private const string UNARMED_BLOCK = "Block-Unarmed";
    private const string ONE_HANDED_BLOCK = "Block-One-Handed";
    private const string TWO_HANDED_BLOCK = "Block-Two-Handed";
    private const string SHIELD_BLOCK = "Block-Shield";

    public PlayerBlockingState(PlayerStateMachine _stateMachine) : base(_stateMachine) {}

    public override void Enter()
    {
        stateMachine.sneaking = false;
        stateMachine.Attributes.isBlocking = true;
        if (!string.IsNullOrEmpty(stateMachine.WeaponHandler.shieldEquipped)) stateMachine.animator.CrossFadeInFixedTime(SHIELD_BLOCK, CrossFadeDuration);
        else if (stateMachine.WeaponHandler.mainHandWeapon.twoHanded) stateMachine.animator.CrossFadeInFixedTime(TWO_HANDED_BLOCK, CrossFadeDuration);
        else if (stateMachine.WeaponHandler.mainHandWeapon.oneHanded) stateMachine.animator.CrossFadeInFixedTime(ONE_HANDED_BLOCK, CrossFadeDuration);
        else stateMachine.animator.CrossFadeInFixedTime(UNARMED_BLOCK, CrossFadeDuration);
        stateMachine.WeaponHandler.ApplyWeaponMasks(stateMachine.animationMask, stateMachine.animator, false);
        
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);
        if (!stateMachine.InputReader.IsBlocking) ReturnToLocomotion();
        if (stateMachine.Attributes.GetStamina() <= 0)
        {
            stateMachine.canBlock = false;
            stateMachine.StartCoroutine("BlockCooldown");
            stateMachine.HandleTakeDamage(true);
        }
    }

    public override void Exit() 
    {
        stateMachine.WeaponHandler.ApplyWeaponMasks(stateMachine.animationMask, stateMachine.animator, true);
        stateMachine.Attributes.isBlocking = false;
    }
}

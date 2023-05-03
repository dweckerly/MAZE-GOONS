using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodgingState : PlayerBaseState
{
    private readonly int DodgingBlendTree = Animator.StringToHash("Dodging BlendTree");
    private readonly int DodgingForward = Animator.StringToHash("DodgingForward");
    private readonly int DodgingRight = Animator.StringToHash("DodgingRight");

    private Vector3 dodgeDirection;
    private float remainingDodgeTime;
    private float dodgeAdjustDistance = 2f;

    public PlayerDodgingState(PlayerStateMachine _stateMachine, Vector3 direction) : base(_stateMachine) 
    {
        dodgeDirection = direction;
    }

    public override void Enter()
    {
        stateMachine.sneaking = false;
        remainingDodgeTime = stateMachine.dodgeDuration;
        stateMachine.animator.SetFloat(DodgingForward, dodgeDirection.y);
        stateMachine.animator.SetFloat(DodgingRight, dodgeDirection.x);
        stateMachine.animator.CrossFadeInFixedTime(DodgingBlendTree, CrossFadeDuration);
        stateMachine.DodgeParticles.Play();
        stateMachine.Attributes.SpendStamina(stateMachine.dodgeStaminaReq);
    }

    public override void Exit() 
    {
        stateMachine.DodgeParticles.Stop();
    }

    public override void Tick(float deltaTime)
    {
        if (stateMachine.InputReader.IsAttacking && (stateMachine.Attributes.currentStamina > stateMachine.WeaponHandler.mainHandWeapon.staminaReq))
        {
            stateMachine.SwitchState(new PlayerDodgeAttackState(stateMachine));
            return;
        }
        Vector3 movement = new Vector3();
        movement += stateMachine.transform.right * dodgeDirection.x * stateMachine.dodgeDistance / stateMachine.dodgeDuration;
        movement += stateMachine.transform.forward * dodgeDirection.y * stateMachine.dodgeDistance / stateMachine.dodgeDuration;
        
        Move(movement, deltaTime);

        if (stateMachine.Targeter.CurrentTarget != null)
        {
            float toDistanceSqr = (stateMachine.Targeter.CurrentTarget.transform.position - stateMachine.transform.position).sqrMagnitude;
            if (toDistanceSqr >= dodgeAdjustDistance * dodgeAdjustDistance) FaceTarget();
        }
        
        remainingDodgeTime -= deltaTime;
        if (remainingDodgeTime <= 0f) ReturnToLocomotion();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractingState : PlayerBaseState
{
    private readonly int FreeLookBlendTree = Animator.StringToHash("Free Look Blend Tree");
    public PlayerInteractingState(PlayerStateMachine _stateMachine) : base(_stateMachine) {}

    public override void Enter() 
    {
        stateMachine.animator.CrossFadeInFixedTime(FreeLookBlendTree, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        if (stateMachine.Interacter.Interaction != null && stateMachine.Interacter.Interaction.CanInteract)
        {
            stateMachine.Interacter.Interaction.Interact(stateMachine);
        }
        else
        {
            ReturnToLocomotion();
            return;
        }
    }

    public override void Exit() {}
}

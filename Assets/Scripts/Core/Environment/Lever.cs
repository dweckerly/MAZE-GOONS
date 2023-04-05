using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : Interactable
{
    Animator animator;
    public LeverTarget leverTarget;

    public override InteractableType type { get { return InteractableType.Lever; } }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public override void Interact(PlayerStateMachine stateMachine)
    {
        InteractWithLever();
        CanInteract = false;
    }

    public void InteractWithLever()
    {
        animator.SetBool("IsOpen", true);
    }

    // animation event used in the last frame of the opening and closing animations
    public void AnimationComplete()
    {
        // Open target
        leverTarget.Open();
    }
}

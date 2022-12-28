using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public override void Interact(PlayerStateMachine stateMachine)
    {
        InteractWithDoor();
        CanInteract = false;
    }

    public void InteractWithDoor()
    {
        bool open = animator.GetBool("IsOpen");
        animator.SetTrigger("OpenDoor");
        animator.SetBool("IsOpen", !open);
    }

    // animation event used in the last frame of the opening and closing animations
    public void AnimationComplete()
    {
        CanInteract = true;
    }
}

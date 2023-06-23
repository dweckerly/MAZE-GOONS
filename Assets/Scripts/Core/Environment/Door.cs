using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    Animator animator;
    public bool isOpen = false;
    public AudioClip clip;
    public AudioSource audioSource;

    public override InteractableType type { get { return InteractableType.Door; } } 

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
        isOpen = !isOpen;
        animator.SetBool("IsOpen", isOpen);
    }

    // animation event used in the last frame of the opening and closing animations
    public void AnimationComplete()
    {
        CanInteract = true;
    }

    // animation event
    void DoorOpening()
    {
        if (clip == null || audioSource == null) return;
        audioSource.clip = clip;
        audioSource.Play();
    }
}

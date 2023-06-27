using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    Animator animator;
    public bool isOpen = false;
    public AudioClip openingClip;
    public AudioClip closingClip;
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
        CanInteract = false;
    }

    // animation event used in the last frame of the opening and closing animations
    public void AnimationComplete()
    {
        //CanInteract = true;
    }

    // animation event
    void DoorOpening()
    {
        if (openingClip == null || audioSource == null) return;
        audioSource.clip = openingClip;
        audioSource.Play();
    }

    // animation event
    void DoorClosing()
    {
        if (closingClip == null || audioSource == null) return;
        audioSource.clip = closingClip;
        audioSource.Play();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Interactable
{
    Animator animator;
    [SerializeField] GameObject particlePrefab;

    private void Awake() 
    {
        animator = GetComponent<Animator>();
    }

    public override void Interact()
    {
        Debug.Log("Interact called!");
        OpenChest();
        CanInteract = false;
    }

    public void OpenChest()
    {
        animator.SetTrigger("OpenChest");
        animator.SetBool("IsOpen", true);
        particlePrefab.SetActive(true);
    }
}

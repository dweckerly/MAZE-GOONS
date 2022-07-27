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

    public override void Interact(PlayerStateMachine stateMachine)
    {
        OpenChest();
        CanInteract = false;
        stateMachine.Inventory.UpdateGold(Random.Range(10, 1001));
    }

    public void OpenChest()
    {
        animator.SetTrigger("OpenChest");
        animator.SetBool("IsOpen", true);
        particlePrefab.SetActive(true);
    }
}

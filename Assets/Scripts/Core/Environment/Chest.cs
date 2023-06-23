using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Interactable
{
    public override InteractableType type { get { return InteractableType.Chest; } }
    Animator animator;
    [SerializeField] GameObject particlePrefab;
    public int Gold = 0;

    private void Awake() 
    {
        animator = GetComponent<Animator>();
    }

    public override void Interact(PlayerStateMachine stateMachine)
    {
        OpenChest();
        CanInteract = false;
        if (Gold == 0) Gold = Random.Range(100, 1001);
        stateMachine.Inventory.UpdateGold(Gold);
        stateMachine.Notifier.NotifyGold(Gold);
    }

    public void OpenChest()
    {
        animator.SetTrigger("OpenChest");
        animator.SetBool("IsOpen", true);
        particlePrefab.SetActive(true);
    }
}

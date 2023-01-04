using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : Interactable
{
    public Item item;
    public override InteractableType type { get { return InteractableType.PickUp; } }

    public override void Interact(PlayerStateMachine stateMachine)
    {
        stateMachine.Inventory.AddItem(item);
        Destroy(this.gameObject);
    }
}

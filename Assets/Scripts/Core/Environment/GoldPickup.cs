using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldPickup : Interactable
{
    public int GoldAmount = 0;
    public override InteractableType type { get { return InteractableType.Gold; } }

    public override void Interact(PlayerStateMachine stateMachine)
    {
        if (GoldAmount == 0) stateMachine.Inventory.UpdateGold(Random.Range(10, 1001));
        else stateMachine.Inventory.UpdateGold(GoldAmount);
        Destroy(this.gameObject);
    }
}

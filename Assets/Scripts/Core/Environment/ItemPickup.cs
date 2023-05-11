using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : Interactable
{
    public Item item;
    public override InteractableType type { get { return InteractableType.PickUp; } }
    public AudioSource AudioSource;

    public override void Interact(PlayerStateMachine stateMachine)
    {
        stateMachine.Inventory.AddItem(item);
        AudioSource.Play();
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            gameObject.transform.GetChild(i).gameObject.SetActive(false);
        }
        CanInteract = false;
        Destroy(this.gameObject, 1f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : Interactable
{
    public List<Item> items = new List<Item>();

    public override void Interact(PlayerStateMachine stateMachine)
    {
        CanInteract = false;
    }
}

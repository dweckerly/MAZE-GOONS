using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : Interactable
{
    public override InteractableType type { get { return InteractableType.Body; } }
    public List<Item> items = new List<Item>();
    public Collider lootCollider;
    public EnemyStateMachine enemyStateMachine;

    public void Awake()
    {
        DisableLoot();
    }

    public void EnableLoot()
    {
        CanInteract = true;
        lootCollider.enabled = true;
    }

    public void DisableLoot()
    {
        CanInteract = false;
        lootCollider.enabled = false;
    }

    public override void Interact(PlayerStateMachine stateMachine)
    {
        CanInteract = false;
    }

    public void AddItem(Item item)
    {
        items.Add(item);
    }
}

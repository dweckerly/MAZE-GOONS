using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : Interactable
{
    public override InteractableType type { get { return InteractableType.Body; } }
    public List<Item> items = new List<Item>();
    public Collider lootCollider;
    public EnemyStateMachine enemyStateMachine;

    public void Enable()
    {
        CanInteract = true;
        lootCollider.enabled = true;
    }

    public override void Interact(PlayerStateMachine stateMachine)
    {
        CanInteract = false;
    }
}

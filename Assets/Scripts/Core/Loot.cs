using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LootItem
{
    public Item item;
    public GameObject prefab;

    public LootItem(Item _item, GameObject _prefab = null)
    {
        item = _item;
        if (_prefab != null) prefab = _prefab;
    }
}

public class Loot : Interactable
{
    public override InteractableType type { get { return InteractableType.Body; } }
    public List<LootItem> items = new List<LootItem>();
    public Collider lootCollider;
    public GameObject lootParticles;

    public void Awake()
    {
        DisableLoot();
    }

    public void EnableLoot()
    {
        CanInteract = true;
        lootCollider.enabled = true;
        if (lootParticles != null) lootParticles.SetActive(true);
    }

    public void DisableLoot()
    {
        CanInteract = false;
        lootCollider.enabled = false;
        if (lootParticles != null) lootParticles.SetActive(false);
    }

    public override void Interact(PlayerStateMachine stateMachine)
    {
        CanInteract = false;
    }

    public void AddItem(LootItem item)
    {
        items.Add(item);
    }
}

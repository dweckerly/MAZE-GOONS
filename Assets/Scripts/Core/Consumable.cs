using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Consumable", menuName = "Items/New Consumable", order = 4)]
public abstract class Consumable : Item
{
    public override ItemType itemType => ItemType.Consumable;

    public abstract void Consume();
}

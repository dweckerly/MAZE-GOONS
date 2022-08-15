using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Armor,
    Weapon
}

public abstract class Item : BaseScriptableObject
{
    public string itemName;
    public abstract ItemType itemType { get; }
    public float weight;
    public int value;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Items/New Shield", order = 2)]
public class Shield : Item
{
    public override ItemType itemType => ItemType.Shield;
}

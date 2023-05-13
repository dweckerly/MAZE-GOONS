using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Items/New Shield", order = 2)]
public class Shield : Item
{
    public override ItemType itemType => ItemType.Shield;

    public GameObject shieldPrefab = null;
    
    [Range(0, 180f)]
    public float blockingAngle = 60f;
    public int DamageReduction = 1;
}

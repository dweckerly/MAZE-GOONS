using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dual Weapon", menuName = "Items/Weapons/New Dual Weapon", order = 2)]
public class DualWeapon : Weapon
{
    public GameObject offHandPrefab = null;
}

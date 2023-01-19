using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Items/New Weapon", order = 2)]
public class Weapon : Item
{
    public override ItemType itemType => ItemType.Weapon;

    public GameObject weaponPrefab = null;
    public GameObject offHandPrefab = null;
    public int weaponDamage = 1;
    public float weaponLength = 1f;
    public bool oneHanded = true;
    public bool twoHanded = false;
    [field: SerializeField] public Attack[] Attacks { get; private set; }
}

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
    public int staminaReq = 1;
    public bool oneHanded = true;
    public bool twoHanded = false;
    public bool projectile = false;
    public bool dual = false;
    public bool head = false;
    public bool lootable = true;
    public bool unarmed = false;
    [field: SerializeField] public Attack[] Attacks { get; private set; }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Items/Weapons/New Weapon", order = 1)]
public class Weapon : Item
{
    public GameObject weaponPrefab = null;
    public bool rightHanded = true;
    public bool dual = false;
    [field: SerializeField] public Attack[] Attacks { get; private set; }

}

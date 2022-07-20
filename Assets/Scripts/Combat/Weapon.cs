using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Items/New Weapon", order = 1)]
public class Weapon : Item
{
    public GameObject weaponPrefab = null;
    public bool rightHanded = true;
    [field: SerializeField] public Attack[] Attacks { get; private set; }

}

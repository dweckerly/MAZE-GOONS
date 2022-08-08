using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Items/New Weapon", order = 2)]
public class Weapon : Item
{
    public override string itemType => "Weapon";

    public GameObject weaponPrefab = null;
    public GameObject offHandPrefab = null;
    public int weaponDamage = 1;
    public bool rightHanded = true;
    public bool dual = false;
    [field: SerializeField] public Attack[] Attacks { get; private set; }
    public string blockingAnimation;
    public int maskLayer;

    public override void UIClick()
    {
        Debug.Log("UI click");
    }
}

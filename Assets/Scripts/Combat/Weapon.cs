using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Items/Weapons/New Weapon", order = 1)]
public class Weapon : Item
{
    public GameObject weaponPrefab = null;
    public GameObject offHandPrefab = null;
    public int weaponDamage = 1;
    public bool rightHanded = true;
    public bool dual = false;
    [field: SerializeField] public Attack[] Attacks { get; private set; }

    protected int additiveDamageModifier = 0;
    protected float multiplicativeDamageModifier = 0f;
    protected WeaponDamage mainHandDamage;
    protected WeaponDamage offHandDamage;

    public virtual void Init() 
    {
        mainHandDamage = weaponPrefab.GetComponent<WeaponDamage>();
        mainHandDamage.baseDamage = weaponDamage;
        DisableRightHand();
        if (offHandPrefab != null)
        {
            offHandDamage = offHandPrefab.GetComponent<WeaponDamage>();
            offHandDamage.baseDamage = weaponDamage;
            DisableLeftHand();
        }
    }

    public void EnableRightHand()
    {
        weaponPrefab.SetActive(true);
    }

    public virtual void DisableRightHand()
    {
        weaponPrefab.SetActive(false);
    }

    public void EnableLeftHand()
    {
        offHandPrefab?.SetActive(true);
    }

    public void DisableLeftHand()
    {
        offHandPrefab?.SetActive(false);
    }

}

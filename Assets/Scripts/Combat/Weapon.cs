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
    public string blockingAnimation;

    protected int additiveDamageModifier = 0;
    protected float multiplicativeDamageModifier = 0f;
    public WeaponDamage mainHandDamage;
    public WeaponDamage offHandDamage;
    public Collider mainHandCollider;
    public Collider offHandCollider;

    public virtual void Init() 
    {
        mainHandDamage = weaponPrefab.GetComponent<WeaponDamage>();
        mainHandCollider = weaponPrefab.GetComponent<Collider>();
        mainHandDamage.baseDamage = weaponDamage;
        //DisableRightHand();
        if (offHandPrefab != null)
        {
            offHandDamage = offHandPrefab.GetComponent<WeaponDamage>();
            offHandCollider = offHandPrefab.GetComponent<Collider>();
            offHandDamage.baseDamage = weaponDamage;
            //DisableLeftHand();
        }
    }
}

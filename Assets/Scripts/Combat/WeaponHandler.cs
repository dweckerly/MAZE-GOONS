using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [SerializeField] private Collider sourceCollider;
    [SerializeField] GameObject RightHand;
    [SerializeField] GameObject LeftHand;
    [field: SerializeField] public Weapon defaultWeapon { get; private set; }
    public Weapon currentWeapon;

    private void Awake() 
    {
        sourceCollider = GetComponent<Collider>();
        currentWeapon = Instantiate(defaultWeapon);
        Init();
        EquipWeapon(currentWeapon);
    }

    public virtual void Init()
    {
        currentWeapon.mainHandDamage = currentWeapon.weaponPrefab.GetComponent<WeaponDamage>();
        currentWeapon.mainHandCollider = currentWeapon.weaponPrefab.GetComponent<Collider>();
        currentWeapon.mainHandDamage.baseDamage = currentWeapon.weaponDamage;
        //DisableRightHand();
        if (currentWeapon.offHandPrefab != null)
        {
            currentWeapon.offHandDamage = currentWeapon.offHandPrefab.GetComponent<WeaponDamage>();
            currentWeapon.offHandCollider = currentWeapon.offHandPrefab.GetComponent<Collider>();
            currentWeapon.offHandDamage.baseDamage = currentWeapon.weaponDamage;
            //DisableLeftHand();
        }
    }

    public void EquipWeapon(Weapon weapon)
    {
        if (currentWeapon != null)
        {
            // destroy old weapon
        }
        if (weapon.weaponPrefab != null)
        {
            if (weapon.dual)
            {
                weapon.weaponPrefab = Instantiate(weapon.weaponPrefab, RightHand.transform);
                weapon.offHandPrefab = Instantiate(weapon.offHandPrefab, LeftHand.transform);
            }
            else if (weapon.rightHanded)
                weapon.weaponPrefab = Instantiate(weapon.weaponPrefab, RightHand.transform);
            else
                weapon.weaponPrefab = Instantiate(weapon.weaponPrefab, LeftHand.transform);
        }
        currentWeapon = weapon;
        currentWeapon.mainHandDamage.IgnoreCollider(sourceCollider);
        if (currentWeapon.offHandPrefab != null) currentWeapon.offHandDamage.IgnoreCollider(sourceCollider);
    }
}

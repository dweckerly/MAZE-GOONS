using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [SerializeField] GameObject RightHand;
    [SerializeField] GameObject LeftHand;
    [field: SerializeField] public Weapon defaultWeapon { get; private set; }
    public Weapon currentWeapon;

    private void Awake() 
    {
        currentWeapon = defaultWeapon;
        EquipWeapon(currentWeapon);
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
                DualWeapon dualWeapon = (DualWeapon) weapon;
                Instantiate(weapon.weaponPrefab, weapon.weaponPrefab.transform.position, weapon.weaponPrefab.transform.rotation, RightHand.transform);
                Instantiate(dualWeapon.offHandPrefab, dualWeapon.offHandPrefab.transform.position, dualWeapon.offHandPrefab.transform.rotation, LeftHand.transform);
            }
            else if (weapon.rightHanded) 
                Instantiate(weapon.weaponPrefab, weapon.weaponPrefab.transform.position, weapon.weaponPrefab.transform.rotation, RightHand.transform);
            else
                Instantiate(weapon.weaponPrefab, weapon.weaponPrefab.transform.position, weapon.weaponPrefab.transform.rotation, LeftHand.transform);
        }
        currentWeapon = weapon;
    }
}

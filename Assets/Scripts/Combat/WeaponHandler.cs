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
        currentWeapon.Init();
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
                weapon.weaponPrefab = Instantiate(weapon.weaponPrefab, RightHand.transform);
                weapon.offHandPrefab = Instantiate(weapon.offHandPrefab, LeftHand.transform);
            }
            else if (weapon.rightHanded)
                weapon.weaponPrefab = Instantiate(weapon.weaponPrefab, RightHand.transform);
            else
                weapon.weaponPrefab = Instantiate(weapon.weaponPrefab, LeftHand.transform);
        }
        currentWeapon = weapon;
        currentWeapon.weaponPrefab.GetComponent<WeaponDamage>().IgnoreCollider(sourceCollider);
        if (currentWeapon.offHandPrefab != null) currentWeapon.offHandPrefab?.GetComponent<WeaponDamage>().IgnoreCollider(sourceCollider);
    }
}

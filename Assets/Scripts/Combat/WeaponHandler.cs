using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    public event Action OnEquip;
    [SerializeField] GameObject RightHand;
    [SerializeField] GameObject LeftHand;
    [field: SerializeField] public Weapon defaultWeapon { get; private set; }
    public Weapon currentWeapon;
    public GameObject mainHandPrefab;
    public GameObject offHandPrefab;
    public WeaponDamage mainHandDamage;
    public WeaponDamage offHandDamage;
    public Collider mainHandCollider;
    public Collider offHandCollider;

    [SerializeField] private Collider sourceCollider;

    int LayerInt;

    private void Awake() 
    {
        LayerInt = LayerMask.NameToLayer(gameObject.tag);
        sourceCollider = GetComponent<Collider>();
        EquipWeapon(defaultWeapon);
    }

    public void EquipWeapon(Weapon weapon)
    {
        if (currentWeapon != null && currentWeapon.Id == weapon.Id) weapon = defaultWeapon;
        Destroy(mainHandPrefab);
        Destroy(offHandPrefab);
        currentWeapon = weapon;
        mainHandPrefab = Instantiate(weapon.weaponPrefab, RightHand.transform);
        mainHandPrefab.layer = LayerInt;
        mainHandDamage = mainHandPrefab.GetComponent<WeaponDamage>();
        mainHandDamage.IgnoreCollider(sourceCollider);
        mainHandDamage.baseDamage = currentWeapon.weaponDamage;
        mainHandCollider = mainHandPrefab.GetComponent<Collider>();
        DisableRightHandCollider();
        if (weapon.offHandPrefab != null) 
        {
            offHandPrefab = Instantiate(weapon.offHandPrefab, LeftHand.transform);
            offHandPrefab.layer = LayerInt;
            offHandDamage = offHandPrefab.GetComponent<WeaponDamage>();
            offHandDamage.IgnoreCollider(sourceCollider);
            offHandDamage.baseDamage = currentWeapon.weaponDamage;
            offHandCollider = offHandPrefab.GetComponent<Collider>();
            DisableLeftHandCollider();
        }
        OnEquip?.Invoke();
    }

    public void UnEquipWeapon()
    {
        EquipWeapon(defaultWeapon);
    }

    public void EnableRightHandCollider()
    {
        mainHandCollider.enabled = true;
    }

    public virtual void DisableRightHandCollider()
    {
        mainHandCollider.enabled = false;
    }

    public void EnableLeftHandCollider()
    {
        offHandCollider.enabled = true;
    }

    public void DisableLeftHandCollider()
    {
        offHandCollider.enabled = false;
    }
}

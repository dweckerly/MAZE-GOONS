using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [SerializeField] GameObject RightHand;
    [SerializeField] GameObject LeftHand;
    [field: SerializeField] public Weapon defaultWeapon { get; private set; }
    public Weapon currentWeapon;
    public GameObject mainHandPrefab;
    public GameObject offHandPrefab;

    Collider mainHandCollider;

    [SerializeField] private Collider sourceCollider;

    private void Awake() 
    {
        sourceCollider = GetComponent<Collider>();
        EquipWeapon(defaultWeapon);
    }

    public void EquipWeapon(Weapon weapon)
    {
        currentWeapon = Instantiate(weapon);
        mainHandPrefab = Instantiate(weapon.weaponPrefab, RightHand.transform);
        if (weapon.offHandPrefab != null) offHandPrefab = Instantiate(weapon.offHandPrefab, LeftHand.transform);
        mainHandPrefab.GetComponent<WeaponDamage>().IgnoreCollider(sourceCollider);
        if (offHandPrefab != null) offHandPrefab.GetComponent<WeaponDamage>().IgnoreCollider(sourceCollider);
    }

    public void ClearWeaponColliderHistory(bool right)
    {
        if (right) mainHandPrefab.GetComponent<WeaponDamage>().ClearColliderList();
        else offHandPrefab.GetComponent<WeaponDamage>().ClearColliderList();
    }

    public void EnableRightHandCollider()
    {
        mainHandPrefab.GetComponent<CapsuleCollider>().enabled = true;
    }

    public virtual void DisableRightHandCollider()
    {
        mainHandPrefab.GetComponent<CapsuleCollider>().enabled = false;
    }

    public void EnableLeftHandCollider()
    {
        offHandPrefab.GetComponent<CapsuleCollider>().enabled = true;
    }

    public void DisableLeftHandCollider()
    {
        offHandPrefab.GetComponent<CapsuleCollider>().enabled = false;
    }
}

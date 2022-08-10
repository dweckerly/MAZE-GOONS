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
    public WeaponDamage mainHandDamage;
    public WeaponDamage offHandDamage;
    public Collider mainHandCollider;
    public Collider offHandCollider;

    [SerializeField] private Collider sourceCollider;

    private void Awake() 
    {
        sourceCollider = GetComponent<Collider>();
        EquipWeapon(defaultWeapon);
    }

    public void EquipWeapon(Weapon weapon)
    {
        Destroy(mainHandPrefab);
        Destroy(offHandPrefab);
        currentWeapon = weapon;
        mainHandPrefab = Instantiate(weapon.weaponPrefab, RightHand.transform);
        mainHandDamage = mainHandPrefab.GetComponent<WeaponDamage>();
        mainHandDamage.IgnoreCollider(sourceCollider);
        mainHandCollider = mainHandPrefab.GetComponent<Collider>();
        DisableRightHandCollider();
        if (weapon.offHandPrefab != null) 
        {
            offHandPrefab = Instantiate(weapon.offHandPrefab, LeftHand.transform);
            offHandDamage = offHandPrefab.GetComponent<WeaponDamage>();
            offHandDamage.IgnoreCollider(sourceCollider);
            offHandCollider = offHandPrefab.GetComponent<Collider>();
            DisableLeftHandCollider();
        }
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

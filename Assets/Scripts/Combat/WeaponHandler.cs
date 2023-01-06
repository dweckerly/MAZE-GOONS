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
        if(gameObject.CompareTag("Player")) LayerInt = LayerMask.NameToLayer(gameObject.tag);
        sourceCollider = GetComponent<Collider>();
        EquipWeapon(defaultWeapon);
    }

    public void EquipShield(Shield shield)
    {

    }

    public void EquipWeapon(Weapon weapon)
    {
        if (currentWeapon != null && currentWeapon.Id == weapon.Id) weapon = defaultWeapon;
        Destroy(mainHandPrefab);
        Destroy(offHandPrefab);
        currentWeapon = weapon;
        mainHandPrefab = Instantiate(weapon.weaponPrefab, RightHand.transform);
        SetWeaponLayer(mainHandPrefab);
        SetWeaponDamage();
        if (weapon.offHandPrefab != null)
        {
            SetWeaponLayer(offHandPrefab);
            offHandDamage = offHandPrefab.GetComponent<WeaponDamage>();
            offHandDamage.IgnoreCollider(sourceCollider);
            offHandDamage.baseDamage = currentWeapon.weaponDamage;
            offHandCollider = offHandPrefab.GetComponent<Collider>();
            DisableLeftHandCollider();
        }
        OnEquip?.Invoke();
    }

    private void SetWeaponDamage()
    {
        mainHandDamage = mainHandPrefab.GetComponent<WeaponDamage>();
        mainHandDamage.IgnoreCollider(sourceCollider);
        mainHandDamage.baseDamage = currentWeapon.weaponDamage;
        mainHandCollider = mainHandPrefab.GetComponent<Collider>();
        DisableRightHandCollider();
    }

    private void SetWeaponLayer(GameObject prefab)
    {
        prefab.layer = LayerInt;
        foreach (Transform t in prefab.GetComponentInChildren<Transform>())
        {
            t.gameObject.layer = LayerInt;
        }
    }

    public void UnEquipWeapon()
    {
        EquipWeapon(defaultWeapon);
    }

    // animation event required to enable weapon collider
    void StartHit()
    {
        if (mainHandDamage.swingParticle != null) mainHandDamage.swingParticle.Play();
        EnableRightHandCollider();
        if (offHandCollider != null) EnableLeftHandCollider();
    }

    // animation event required to disable weapon collider
    void EndHit()
    {
        DisableRightHandCollider();
        if (offHandCollider != null) DisableLeftHandCollider();

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

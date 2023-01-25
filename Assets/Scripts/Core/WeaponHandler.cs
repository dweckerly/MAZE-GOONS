using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponHand
{
    Right,
    Left
}

public class WeaponHandler : MonoBehaviour
{
    public event Action OnEquip;
    [SerializeField] GameObject RightHand;
    [SerializeField] GameObject LeftHand;
    [field: SerializeField] public Weapon defaultWeapon { get; private set; }
    public Weapon mainHandWeapon;
    public Weapon offHandWeapon;
    public GameObject mainHandPrefab;
    public GameObject offHandPrefab;
    public WeaponDamage mainHandDamage;
    public WeaponDamage offHandDamage;
    public Collider mainHandCollider;
    public Collider offHandCollider;

    [SerializeField] private Collider sourceCollider;

    int LayerInt;

    public string shieldEquipped = null;

    private const int  RIGHT_GRIP = 1;
    private const int LEFT_GRIP = 2;
    private const int ONE_HANDED_ARM_POSITION = 3;
    private const int ONE_HANDED_ARM_POSITION_LEFT = 4;
    private const int TWO_HANDED_ARM_POSITION = 5;
    private const int SHIELD_ARM_POSITION = 6;
    public List<int> maskLayers = new List<int>();

    private void Awake() 
    {
        if(gameObject.CompareTag("Player")) LayerInt = LayerMask.NameToLayer(gameObject.tag);
        sourceCollider = GetComponent<Collider>();
        EquipDefaultWeapon();
    }

    public void EquipWeapon(Weapon weapon, WeaponHand hand = WeaponHand.Right)
    {
        if (weapon.twoHanded)
        {
            if (weapon.Id == mainHandWeapon.Id)
            {
                UnEquipAllWeapons();
                EquipDefaultWeapon();
            } 
            else 
            {
                UnEquipAllWeapons();
                EquipMainHand(weapon);
            }
        }
        if (weapon.oneHanded)
        {
            if (hand == WeaponHand.Right)
            {
                if (weapon.Id == mainHandWeapon.Id)
                {
                    UnEquipMainHand();
                    EquipDefaultMainHand();
                } 
                else 
                {
                    UnEquipMainHand();
                    EquipMainHand(weapon);
                }
            }
            else
            {
                if (mainHandWeapon.twoHanded)
                {
                    UnEquipMainHand();
                    EquipDefaultMainHand();
                    EquipOffHand(weapon);
                }
                else
                {
                    if (offHandWeapon != null && weapon.Id == offHandWeapon.Id)
                    {
                        UnEquipOffHand();
                        EquipDefaultOffHand();
                    }
                    else
                    {
                        UnEquipOffHand();
                        EquipOffHand(weapon);
                    }
                }
                
            }
        }
        OnEquip?.Invoke();
    }

    private void EquipDefaultWeapon()
    {
        EquipDefaultMainHand();
        EquipDefaultOffHand();
        maskLayers.Clear();
        OnEquip?.Invoke();
    }

    private void EquipDefaultOffHand()
    {
        offHandWeapon = defaultWeapon;
        offHandPrefab = Instantiate(offHandWeapon.offHandPrefab, LeftHand.transform);
        offHandDamage = offHandPrefab.GetComponent<WeaponDamage>();
        offHandDamage.IgnoreCollider(sourceCollider);
        offHandDamage.baseDamage = offHandWeapon.weaponDamage;
        offHandCollider = offHandPrefab.GetComponent<Collider>();
        DisableLeftHandCollider();
    }

    private void EquipDefaultMainHand()
    {
        mainHandWeapon = defaultWeapon;
        mainHandPrefab = Instantiate(mainHandWeapon.weaponPrefab, RightHand.transform);
        mainHandDamage = mainHandPrefab.GetComponent<WeaponDamage>();
        mainHandDamage.IgnoreCollider(sourceCollider);
        mainHandDamage.baseDamage = mainHandWeapon.weaponDamage;
        mainHandCollider = mainHandPrefab.GetComponent<Collider>();
        DisableRightHandCollider();
    }

    private void EquipMainHand(Weapon weapon)
    {
        mainHandWeapon = weapon;
        mainHandPrefab = Instantiate(mainHandWeapon.weaponPrefab, RightHand.transform);
        mainHandDamage = mainHandPrefab.GetComponent<WeaponDamage>();
        mainHandDamage.IgnoreCollider(sourceCollider);
        mainHandDamage.baseDamage = mainHandWeapon.weaponDamage;
        mainHandCollider = mainHandPrefab.GetComponent<Collider>();
        if (mainHandWeapon.twoHanded)
        {
            maskLayers.Add(RIGHT_GRIP);
            maskLayers.Add(LEFT_GRIP);
            maskLayers.Add(TWO_HANDED_ARM_POSITION);
        }
        else
        {
            maskLayers.Add(RIGHT_GRIP);
            maskLayers.Add(ONE_HANDED_ARM_POSITION);
        }
        SetWeaponLayer(mainHandPrefab);
        DisableRightHandCollider();
    }

    private void EquipOffHand(Weapon weapon)
    {
        offHandWeapon = weapon;
        offHandPrefab = Instantiate(offHandWeapon.offHandPrefab, LeftHand.transform);
        offHandDamage = offHandPrefab.GetComponent<WeaponDamage>();
        offHandDamage.IgnoreCollider(sourceCollider);
        offHandDamage.baseDamage = offHandWeapon.weaponDamage;
        offHandCollider = offHandPrefab.GetComponent<Collider>();
        maskLayers.Add(LEFT_GRIP);
        maskLayers.Add(ONE_HANDED_ARM_POSITION_LEFT);
        SetWeaponLayer(offHandPrefab);
        DisableLeftHandCollider();
    }

    private void UnEquipMainHand()
    {
        if (mainHandPrefab != null) Destroy(mainHandPrefab);
        if (mainHandWeapon != null && mainHandWeapon.twoHanded)
        {
            maskLayers.Remove(RIGHT_GRIP);
            maskLayers.Remove(LEFT_GRIP);
            maskLayers.Remove(TWO_HANDED_ARM_POSITION);
        }
        if (mainHandWeapon != null && mainHandWeapon.oneHanded)
        {
            maskLayers.Remove(RIGHT_GRIP);
            maskLayers.Remove(ONE_HANDED_ARM_POSITION);
        }
        mainHandWeapon = null;
        mainHandDamage = null;
        mainHandCollider = null;
    }

    private void UnEquipOffHand()
    {
        if (offHandPrefab != null) Destroy(offHandPrefab);
        offHandWeapon = null;
        offHandDamage = null;
        offHandCollider = null;
        maskLayers.Remove(LEFT_GRIP);
        maskLayers.Remove(ONE_HANDED_ARM_POSITION_LEFT);
        maskLayers.Remove(SHIELD_ARM_POSITION);
        shieldEquipped = null;
    }

    private void UnEquipAllWeapons()
    {
        UnEquipMainHand();
        UnEquipOffHand();
    }

    public void EquipShield(Shield shield)
    {
        if (!string.IsNullOrEmpty(shieldEquipped) && shieldEquipped.Equals(shield.Id))
        {
            UnEquipOffHand();
            EquipDefaultOffHand();
        }
        else
        {
            if (mainHandWeapon != null && mainHandWeapon.twoHanded)
            {
                UnEquipMainHand();
                EquipDefaultMainHand();
            }
            else
            {
                UnEquipOffHand();
            }
            offHandPrefab = Instantiate(shield.shieldPrefab, LeftHand.transform);
            maskLayers.Add(LEFT_GRIP);
            maskLayers.Add(SHIELD_ARM_POSITION);
            SetWeaponLayer(offHandPrefab);
            shieldEquipped = shield.Id;
        }
        OnEquip?.Invoke();
    }

    public void ApplyWeaponMasks(AnimationMaskHandler animationMaskHandler, Animator animator, bool value)
    {
        foreach (int i in maskLayers)
        {
            animationMaskHandler.ApplyLayerWeight(animator, i, value);
        }
    }

    private void SetWeaponLayer(GameObject prefab)
    {
        prefab.layer = LayerInt;
        foreach (Transform t in prefab.GetComponentInChildren<Transform>())
        {
            t.gameObject.layer = LayerInt;
        }
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

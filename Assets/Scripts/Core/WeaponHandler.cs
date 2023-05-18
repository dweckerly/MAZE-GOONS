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
    public event Action OnEquipShield;
    public event Action OnUnEquipShield;
    [SerializeField] GameObject Head;
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

    public Shield shieldEquipped = null;

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
        maskLayers.Clear();
        if (defaultWeapon.head) EquipDefaultHead();
        else
        {
            EquipDefaultMainHand();
            if (defaultWeapon.dual) EquipDefaultOffHand();
        }
        OnEquip?.Invoke();
    }

    private void EquipDefaultOffHand()
    {
        offHandWeapon = defaultWeapon;
        offHandPrefab = Instantiate(offHandWeapon.offHandPrefab, LeftHand.transform);
        offHandDamage = offHandPrefab.GetComponent<WeaponDamage>();
        offHandDamage.sourceTransform = gameObject.transform;
        offHandDamage.IgnoreCollider(sourceCollider);
        offHandDamage.baseDamage = offHandWeapon.weaponDamage;
        offHandCollider = offHandPrefab.GetComponent<Collider>();
        if (!defaultWeapon.unarmed)
        {
            maskLayers.Add(LEFT_GRIP);
            maskLayers.Add(ONE_HANDED_ARM_POSITION_LEFT);
        }
        DisableLeftHandCollider();
    }

    private void EquipDefaultMainHand()
    {
        mainHandWeapon = defaultWeapon;
        mainHandPrefab = Instantiate(mainHandWeapon.weaponPrefab, RightHand.transform);
        mainHandPrefab.layer = gameObject.layer;
        mainHandDamage = mainHandPrefab.GetComponent<WeaponDamage>();
        mainHandDamage.sourceTransform = gameObject.transform;
        mainHandDamage.IgnoreCollider(sourceCollider);
        mainHandDamage.baseDamage = mainHandWeapon.weaponDamage;
        mainHandCollider = mainHandPrefab.GetComponent<Collider>();
        if (!defaultWeapon.unarmed)
        {
            maskLayers.Add(RIGHT_GRIP);
            maskLayers.Add(ONE_HANDED_ARM_POSITION);
        }
        DisableRightHandCollider();
    }

    private void EquipDefaultHead()
    {
        mainHandWeapon = defaultWeapon;
        mainHandPrefab = Instantiate(mainHandWeapon.weaponPrefab, Head.transform);
        mainHandPrefab.layer = gameObject.layer;
        mainHandDamage = mainHandPrefab.GetComponent<WeaponDamage>();
        mainHandDamage.sourceTransform = gameObject.transform;
        mainHandDamage.IgnoreCollider(sourceCollider);
        mainHandDamage.baseDamage = mainHandWeapon.weaponDamage;
        mainHandCollider = mainHandPrefab.GetComponent<Collider>();
        DisableRightHandCollider();
        OnEquip?.Invoke();
    }

    private void EquipMainHand(Weapon weapon)
    {
        mainHandWeapon = weapon;
        mainHandPrefab = Instantiate(mainHandWeapon.weaponPrefab, RightHand.transform);
        mainHandPrefab.layer = gameObject.layer;
        mainHandDamage = mainHandPrefab.GetComponent<WeaponDamage>();
        mainHandDamage.sourceTransform = gameObject.transform;
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
        offHandPrefab.layer = gameObject.layer;
        offHandDamage = offHandPrefab.GetComponent<WeaponDamage>();
        offHandDamage.sourceTransform = gameObject.transform;
        offHandDamage.IgnoreCollider(sourceCollider);
        offHandDamage.baseDamage = offHandWeapon.weaponDamage;
        offHandCollider = offHandPrefab.GetComponent<Collider>();
        maskLayers.Add(LEFT_GRIP);
        maskLayers.Add(ONE_HANDED_ARM_POSITION_LEFT);
        SetWeaponLayer(offHandPrefab);
        DisableLeftHandCollider();
    }

    public void UnEquipMainHand()
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

    public void UnEquipAllWeapons()
    {
        UnEquipMainHand();
        UnEquipOffHand();
    }

    public void EquipShield(Shield shield)
    {
        if (!string.IsNullOrEmpty(shieldEquipped?.Id) && shieldEquipped.Id.Equals(shield?.Id))
        {
            UnEquipOffHand();
            EquipDefaultOffHand();
            OnUnEquipShield?.Invoke();
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
            shieldEquipped = shield;
            OnEquipShield?.Invoke();
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
        EnableLeftHandCollider();
    }

    // animation event required to disable weapon collider
    void EndHit()
    {
        DisableRightHandCollider();
        DisableLeftHandCollider();
    }

    // animation event used to instantiate projectile
    void Shoot()
    {
        if (!mainHandWeapon.projectile) return;
        GameObject proj = Instantiate(mainHandWeapon.weaponPrefab, RightHand.transform.position, gameObject.transform.rotation);
        proj.GetComponent<WeaponDamage>().IgnoreCollider(sourceCollider);
        proj.layer = gameObject.layer;
    }

    public void EnableRightHandCollider()
    {
        if (mainHandCollider != null) mainHandCollider.enabled = true;
    }

    public virtual void DisableRightHandCollider()
    {
        if (mainHandCollider != null) mainHandCollider.enabled = false;
    }

    public void EnableLeftHandCollider()
    {
        if (offHandCollider != null) offHandCollider.enabled = true;
    }

    public void DisableLeftHandCollider()
    {
        if (offHandCollider != null) offHandCollider.enabled = false;
    }
}

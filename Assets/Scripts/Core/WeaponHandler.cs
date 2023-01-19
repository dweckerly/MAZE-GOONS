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

    private void EquipDefaultWeapon()
    {
        Destroy(mainHandPrefab);
        Destroy(offHandPrefab);
        if (mainHandWeapon == null)
        {
            mainHandWeapon = defaultWeapon;
            mainHandPrefab = Instantiate(defaultWeapon.weaponPrefab, RightHand.transform);
        }
        if (offHandWeapon == null)
        {
            offHandWeapon = defaultWeapon;
            offHandPrefab = Instantiate(defaultWeapon.offHandPrefab, LeftHand.transform);
        }
        SetWeaponMaskLayer();
        SetWeaponDamage();
        OnEquip?.Invoke();
    }

    public void EquipShield(Shield shield)
    {

    }

    public void EquipWeapon(Weapon weapon, WeaponHand hand = WeaponHand.Right)
    {
        if (weapon.twoHanded)
        {
            Destroy(mainHandPrefab);
            Destroy(offHandPrefab);
            if (weapon.Id == mainHandWeapon.Id)
            {
                mainHandWeapon = null;
                offHandWeapon = null;
                EquipDefaultWeapon();
            }
            else 
            {
                mainHandWeapon = weapon;
                mainHandPrefab = Instantiate(weapon.weaponPrefab, RightHand.transform);
                SetWeaponLayer(mainHandPrefab);
            }
        }
        else if (hand == WeaponHand.Right)
        {
            Destroy(mainHandPrefab);
            if (weapon.Id != mainHandWeapon.Id) 
            {
                mainHandWeapon = weapon;
                mainHandPrefab = Instantiate(weapon.weaponPrefab, RightHand.transform);
                SetWeaponLayer(mainHandPrefab);
            }
            else
            {
                mainHandWeapon = null;
                EquipDefaultWeapon();
            }
        }
        else
        {
            Destroy(offHandPrefab);
            if (weapon.Id != offHandWeapon.Id)
            {
                offHandWeapon = weapon;
                offHandPrefab = Instantiate(weapon.weaponPrefab, LeftHand.transform);
                SetWeaponLayer(offHandPrefab);
            }
            else
            {
                offHandWeapon = null;
                EquipDefaultWeapon();
            }           
        }
        //if (mainHandWeapon != null && mainHandWeapon.Id == weapon.Id) weapon = defaultWeapon;
        SetWeaponMaskLayer();
        SetWeaponDamage();
        OnEquip?.Invoke();
    }

    private void SetWeaponMaskLayer()
    {
        maskLayers.Clear();
        if (mainHandWeapon.twoHanded)
        {
            maskLayers.Add(RIGHT_GRIP);
            maskLayers.Add(LEFT_GRIP);
            maskLayers.Add(TWO_HANDED_ARM_POSITION);
            return;
        }
        if (mainHandWeapon != null && mainHandWeapon.oneHanded)
        {
            maskLayers.Add(RIGHT_GRIP);
            maskLayers.Add(ONE_HANDED_ARM_POSITION);
            return;
        }
        if (offHandWeapon != null && offHandWeapon.oneHanded)
        {
            maskLayers.Add(LEFT_GRIP);
            maskLayers.Add(ONE_HANDED_ARM_POSITION_LEFT);
        }
    }

    public void ApplyWeaponMasks(AnimationMaskHandler animationMaskHandler, Animator animator, bool value)
    {
        foreach (int i in maskLayers)
        {
            animationMaskHandler.ApplyLayerWeight(animator, i, value);
        }
    }

    private void SetWeaponDamage()
    {
        mainHandDamage = mainHandPrefab.GetComponent<WeaponDamage>();
        mainHandDamage.IgnoreCollider(sourceCollider);
        mainHandDamage.baseDamage = mainHandWeapon.weaponDamage;
        mainHandCollider = mainHandPrefab.GetComponent<Collider>();
        DisableRightHandCollider();
        if (mainHandWeapon.offHandPrefab != null)
        {
            offHandPrefab = Instantiate(mainHandWeapon.offHandPrefab, LeftHand.transform);
            SetWeaponLayer(offHandPrefab);
            offHandDamage = offHandPrefab.GetComponent<WeaponDamage>();
            offHandDamage.IgnoreCollider(sourceCollider);
            offHandDamage.baseDamage = mainHandWeapon.weaponDamage;
            offHandCollider = offHandPrefab.GetComponent<Collider>();
            DisableLeftHandCollider();
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

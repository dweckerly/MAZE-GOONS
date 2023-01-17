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


    private const int  RIGHT_GRIP = 1;
    private const int LEFT_GRIP = 2;
    private const int ONE_HANDED_ARM_POSITION = 3;
    private const int TWO_HANDED_ARM_POSITION = 4;
    private const int SHIELD_ARM_POSITION = 5;
    public List<int> maskLayers = new List<int>();

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
        SetWeaponMaskLayer();
        mainHandPrefab = Instantiate(weapon.weaponPrefab, RightHand.transform);
        SetWeaponLayer(mainHandPrefab);
        SetWeaponDamage();
        OnEquip?.Invoke();
    }

    private void SetWeaponMaskLayer()
    {
        maskLayers.Clear();
        if (currentWeapon.oneHanded) 
        {
            maskLayers.Add(RIGHT_GRIP);
            maskLayers.Add(ONE_HANDED_ARM_POSITION);
            return;
        }
        if (currentWeapon.dual)
        {
            maskLayers.Add(RIGHT_GRIP);
            maskLayers.Add(LEFT_GRIP);
            maskLayers.Add(ONE_HANDED_ARM_POSITION);
            // need to add dual arm positions
            return;
        }
        if (currentWeapon.twoHanded)
        {
            maskLayers.Add(RIGHT_GRIP);
            maskLayers.Add(LEFT_GRIP);
            maskLayers.Add(TWO_HANDED_ARM_POSITION);
            return;
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
        mainHandDamage.baseDamage = currentWeapon.weaponDamage;
        mainHandCollider = mainHandPrefab.GetComponent<Collider>();
        DisableRightHandCollider();
        if (currentWeapon.offHandPrefab != null)
        {
            offHandPrefab = Instantiate(currentWeapon.offHandPrefab, LeftHand.transform);
            SetWeaponLayer(offHandPrefab);
            offHandDamage = offHandPrefab.GetComponent<WeaponDamage>();
            offHandDamage.IgnoreCollider(sourceCollider);
            offHandDamage.baseDamage = currentWeapon.weaponDamage;
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

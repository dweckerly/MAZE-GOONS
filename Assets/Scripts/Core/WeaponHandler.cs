using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Reflection;

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

    private void Start() 
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
        weapon.equipped = true;
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
        weapon.equipped = true;
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
        if (mainHandWeapon != null) mainHandWeapon.equipped = false;
        mainHandWeapon = null;
        mainHandDamage = null;
        mainHandCollider = null;
    }

    private void UnEquipOffHand()
    {
        if (offHandPrefab != null) Destroy(offHandPrefab);
        if (offHandWeapon != null) offHandWeapon.equipped = false;
        offHandWeapon = null;
        offHandDamage = null;
        offHandCollider = null;
        maskLayers.Remove(LEFT_GRIP);
        maskLayers.Remove(ONE_HANDED_ARM_POSITION_LEFT);
        maskLayers.Remove(SHIELD_ARM_POSITION);
        if (shieldEquipped != null) shieldEquipped.equipped = false;
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
            shield.equipped = false;
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
            shield.equipped = true;
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
        GameObject proj = Instantiate(mainHandWeapon.weaponPrefab, Head.transform.position, gameObject.transform.rotation);
        proj.layer = gameObject.layer;
        WeaponDamage projWD = proj.GetComponent<WeaponDamage>();
        projWD.IgnoreCollider(sourceCollider);
        projWD.sourceTransform = gameObject.transform;
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

    [InspectorButton("OnButtonClicked")]
    public bool clickMe;

    private void OnButtonClicked()
    {
        Debug.Log("Clicked!");
        EquipDefaultWeapon();
    }
}



/// <summary>
/// This attribute can only be applied to fields because its
/// associated PropertyDrawer only operates on fields (either
/// public or tagged with the [SerializeField] attribute) in
/// the target MonoBehaviour.
/// </summary>
[System.AttributeUsage(System.AttributeTargets.Field)]
public class InspectorButtonAttribute : PropertyAttribute
{
  public static float kDefaultButtonWidth = 80;

  public readonly string MethodName;

  private float _buttonWidth = kDefaultButtonWidth;
  public float ButtonWidth
  {
    get { return _buttonWidth; }
    set { _buttonWidth = value; }
  }

  public InspectorButtonAttribute(string MethodName)
  {
    this.MethodName = MethodName;
  }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(InspectorButtonAttribute))]
public class InspectorButtonPropertyDrawer : PropertyDrawer
{
  private MethodInfo _eventMethodInfo = null;

  public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
  {
    InspectorButtonAttribute inspectorButtonAttribute = (InspectorButtonAttribute)attribute;
    Rect buttonRect = new Rect(position.x + (position.width - inspectorButtonAttribute.ButtonWidth) * 0.5f, position.y, inspectorButtonAttribute.ButtonWidth, position.height);
    if (GUI.Button(buttonRect, label.text))
    {
      System.Type eventOwnerType = prop.serializedObject.targetObject.GetType();
      string eventName = inspectorButtonAttribute.MethodName;

      if (_eventMethodInfo == null)
        _eventMethodInfo = eventOwnerType.GetMethod(eventName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

      if (_eventMethodInfo != null)
        _eventMethodInfo.Invoke(prop.serializedObject.targetObject, null);
      else
        Debug.LogWarning(string.Format("InspectorButton: Unable to find method {0} in {1}", eventName, eventOwnerType));
    }
  }
}
#endif
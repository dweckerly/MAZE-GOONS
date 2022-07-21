using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Items/Weapons/New Weapon", order = 1)]
public class Weapon : Item
{
    public GameObject weaponPrefab = null;
    public GameObject offHandPrefab = null;
    public int weaponDamage = 1;
    public bool rightHanded = true;
    public bool dual = false;
    [field: SerializeField] public Attack[] Attacks { get; private set; }

    protected Collider sourceCollider;
    protected int additiveDamageModifier = 0;
    protected float multiplicativeDamageModifier = 0f;

    public virtual void Init() 
    {
        DisableRightHand();
        DisableLeftHand();
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other == sourceCollider) return;
        if (other.TryGetComponent<Attributes>(out Attributes attributes)) 
        {
            int damage = Mathf.RoundToInt((weaponDamage + additiveDamageModifier) * multiplicativeDamageModifier) * -1;
            attributes.ChangeHP(damage);
        }
    }

    public void IgnoreCollider(Collider collider)
    {
        sourceCollider = collider;
    }

    public void SetAdditiveDamageModifier(int mod)
    {
        additiveDamageModifier = mod;
    }

    public void SetMultiplicativeDamageModifier(float mod)
    {
        multiplicativeDamageModifier = mod;
    }

    public void EnableRightHand()
    {
        weaponPrefab.SetActive(true);
    }

    public virtual void DisableRightHand()
    {
        weaponPrefab.SetActive(false);
    }

    public void EnableLeftHand()
    {
        offHandPrefab.SetActive(true);
    }

    public void DisableLeftHand()
    {
        offHandPrefab.SetActive(false);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Items/Weapons/New Weapon", order = 1)]
public class Weapon : Item
{
    public GameObject weaponPrefab = null;
    public int weaponDamage = 1;
    public bool rightHanded = true;
    public bool dual = false;
    [field: SerializeField] public Attack[] Attacks { get; private set; }

    private Collider sourceCollider;
    private int additiveDamageModifier = 0;
    private float multiplicativeDamageModifier = 0f;

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

}

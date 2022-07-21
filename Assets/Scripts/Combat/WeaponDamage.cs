using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    public int baseDamage;
    protected Collider sourceCollider;
    protected List<Collider> alreadyCollidedWith = new List<Collider>();
    protected int additiveDamageModifier = 0;
    protected float multiplicativeDamageModifier = 1f;

    private void OnEnable()
    {
        alreadyCollidedWith.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == sourceCollider) return;
        if (alreadyCollidedWith.Contains(other)) return;
        alreadyCollidedWith.Add(other);
        if (other.TryGetComponent<Attributes>(out Attributes attributes))
        {
            int damage = Mathf.RoundToInt((baseDamage + additiveDamageModifier) * multiplicativeDamageModifier) * -1;
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

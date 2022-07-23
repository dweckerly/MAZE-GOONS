using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    public int baseDamage;
    public float knockback;
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
            int damage = Mathf.RoundToInt((baseDamage + additiveDamageModifier) * multiplicativeDamageModifier);
            Debug.Log("Hitting Player for " + damage + "!");
            attributes.TakeDamage(damage);
        }
        if (other.TryGetComponent<ForceReceiver>(out ForceReceiver forceReceiver))
        {
            Vector3 direction = (other.transform.position - sourceCollider.transform.position).normalized;
            forceReceiver.AddForce(direction * knockback);
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

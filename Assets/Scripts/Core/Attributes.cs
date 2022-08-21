using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Attribute
{
    Brains,
    Brawn,
    Guile,
    Guts
}

public class Attributes : MonoBehaviour
{
    public event Action OnTakeDamage;
    public event Action OnDie;

    public bool alive = true;
    [SerializeField] int brains;
    [SerializeField] int brawn;
    [SerializeField] int guile;
    [SerializeField] int guts;

    [SerializeField] int baseHP = 10;

    public int DamageReduction = 0;

    private int currentBrains;
    private int currentBrawn;
    private int currentGuile;
    private int currentGuts;

    private Dictionary<Attribute, int> attrLookup;

    private int maxHP;
    private int currentHP;

    Animator animator;

    private bool isInvulnerable = false;

    [field: SerializeField] public RectTransform HealthRect { get; private set; }

    private void Awake() 
    {
        currentBrains = brains;
        currentBrawn = brawn;
        currentGuile = guile;
        currentGuts = guts;
        maxHP = CalculateMaxHP();
        currentHP = maxHP;

        attrLookup = new Dictionary<Attribute, int> ()
        {
            { Attribute.Brains, currentBrains },
            { Attribute.Brawn, currentBrawn },
            { Attribute.Guile, currentGuile },
            { Attribute.Guts, currentGuts },
        };
        animator = GetComponent<Animator>();
    }

    private int CalculateMaxHP()
    {
        return baseHP + guts;
    }

    public float CalculateCarryWeight()
    {
        return 50 + 10 * currentBrawn;
    }

    public int GetStat (Attribute attribute)
    {
        return attrLookup[attribute];
    }

    public int GetHP()
    {
        return currentHP;
    }

    public float GetHPFraction()
    {
        return Mathf.Clamp((float)currentHP / (float)maxHP, 0f, 1f);
    }

    public void ChangeStat (Attribute attribute, int amount)
    {
        attrLookup[attribute] += amount;
    }

    public void ChangeHP(int amount)
    {
        currentHP += amount;
        if (currentHP > maxHP) currentHP = maxHP;
        if (HealthRect != null) HealthRect.localScale = new Vector3(GetHPFraction(), 1f, 1f);
        if (currentHP <= 0) Die();
    }

    public void TakeDamage(int amount)
    {
        if (isInvulnerable || !alive) return;
        int damage = Mathf.Clamp(amount - DamageReduction, 0, currentHP);
        if (damage > 0)
        {
            OnTakeDamage?.Invoke();
            ChangeHP(amount * -1);
        }
    }

    public void SetInvulnerable(bool invulnerable)
    {
        isInvulnerable = invulnerable;
    }

    public void Die()
    {
        OnDie?.Invoke();
        alive = false;
    }
}

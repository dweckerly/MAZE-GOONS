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

    private int currentBrains;
    private int currentBrawn;
    private int currentGuile;
    private int currentGuts;

    private Dictionary<Attribute, int> attrLookup;

    private int maxHP;
    private int currentHP;

    Animator animator;

    private bool isInvulnerable = false;

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

    public int GetStat (Attribute attribute)
    {
        return attrLookup[attribute];
    }

    public int GetHP()
    {
        return currentHP;
    }

    public void ChangeStat (Attribute attribute, int amount)
    {
        attrLookup[attribute] += amount;
    }

    public void ChangeHP(int amount)
    {
        currentHP += amount;
        if (currentHP > maxHP) currentHP = maxHP;
        else if (currentHP <= 0) Die();
    }

    public void TakeDamage(int amount)
    {
        if (isInvulnerable) return;
        OnTakeDamage?.Invoke();
        ChangeHP(amount * -1);
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

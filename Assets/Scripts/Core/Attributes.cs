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
    public event Action<bool> OnTakeDamage;
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
    private int maxStamina = 10;
    private float currentStamina;
    private float staminaRecoveryRate = 0.01f;

    Animator animator;

    private bool isInvulnerable = false;

    [field: SerializeField] public RectTransform HealthRect { get; private set; }
    [field: SerializeField] public RectTransform StaminaRect { get; private set; }

    private void Awake() 
    {
        currentBrains = brains;
        currentBrawn = brawn;
        currentGuile = guile;
        currentGuts = guts;
        maxHP = CalculateMaxHP();
        currentHP = maxHP;
        currentStamina = maxStamina;

        attrLookup = new Dictionary<Attribute, int> ()
        {
            { Attribute.Brains, currentBrains },
            { Attribute.Brawn, currentBrawn },
            { Attribute.Guile, currentGuile },
            { Attribute.Guts, currentGuts },
        };
        animator = GetComponent<Animator>();
    }

    private void Start() 
    {
        StartCoroutine("RecoverStamina");
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

    public float GetStamina()
    {
        return currentStamina;
    }

    public float GetHPFraction()
    {
        return Mathf.Clamp((float)currentHP / (float)maxHP, 0f, 1f);
    }

    public float GetStaminaFraction()
    {
        return Mathf.Clamp((float)currentStamina / (float)maxStamina, 0f, 1f);
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
        if (!alive) return;
        int damage = Mathf.Clamp(amount - DamageReduction, 1, currentHP);
        if (damage > 0)
        {
            if (isInvulnerable) SpendStamina(damage);
            else
            {
                if (damage > 1)
                    OnTakeDamage?.Invoke(true);
                else
                    OnTakeDamage?.Invoke(false);
                ChangeHP(amount * -1);
            }
        }
    }

    public void SpendStamina(int amount)
    {
        currentStamina -= Mathf.Clamp(amount, 0, maxStamina);
        if (StaminaRect != null) StaminaRect.localScale = new Vector3(GetStaminaFraction(), 1f, 1f);
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

    private IEnumerator RecoverStamina()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.1f);
            currentStamina = Mathf.Clamp(currentStamina + (maxStamina * staminaRecoveryRate), 0, maxStamina);
            if (StaminaRect != null) StaminaRect.localScale = new Vector3(GetStaminaFraction(), 1f, 1f);
        }
    }

}

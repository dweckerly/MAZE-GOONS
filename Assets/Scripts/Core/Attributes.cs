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

    [SerializeField] int baseHP = 100;

    public int DamageReduction = 0;

    private int currentBrains;
    public int currentBrawn;
    public int currentGuile;
    private int currentGuts;

    private Dictionary<Attribute, int> attrLookup;

    private int maxHP;
    private int currentHP;
    private int maxStamina = 10;
    public float currentStamina;
    private float staminaRecoveryRate;
    private float baseStaminaRecoveryRate = 0.001f;

    Animator animator;

    public bool isBlocking = false;
    private float blockAngle = 70f;
    private float flankAngle = 140f;
    private float flankMod = 1.5f;

    [field: SerializeField] public RectTransform HealthRect { get; private set; }
    [field: SerializeField] public RectTransform StaminaRect { get; private set; }
    [field: SerializeField] public RectTransform HealthBGRect { get; private set; }
    [field: SerializeField] public RectTransform StaminaBGRect { get; private set; }

    float rectScaleFactor = 2f;

    private void Awake() 
    {
        InstantiateStats();
        animator = GetComponent<Animator>();
    }

    private void InstantiateStats()
    {
        currentBrains = brains;
        currentBrawn = brawn;
        currentGuile = guile;
        currentGuts = guts;
        maxHP = CalculateMaxHP();
        currentHP = maxHP;
        maxStamina = (brains + guts) * 10;
        staminaRecoveryRate = baseStaminaRecoveryRate + (0.001f * brains);
        currentStamina = maxStamina;

        attrLookup = new Dictionary<Attribute, int>()
        {
            { Attribute.Brains, currentBrains },
            { Attribute.Brawn, currentBrawn },
            { Attribute.Guile, currentGuile },
            { Attribute.Guts, currentGuts },
        };

        if (HealthBGRect != null)
        {
            HealthBGRect.sizeDelta = new Vector2(maxHP * rectScaleFactor, 30);
            HealthRect.sizeDelta = new Vector2(maxHP * rectScaleFactor, 30);
            StaminaBGRect.sizeDelta = new Vector2(maxStamina * rectScaleFactor, 30);
            StaminaRect.sizeDelta = new Vector2(maxStamina * rectScaleFactor, 30);
        }
    }

    private void Start() 
    {
        StartCoroutine("RecoverStamina");
    }

    private int CalculateMaxHP()
    {
        return baseHP + (10 * guts + 2 * brawn);
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

    public void TakeDamage(int amount, Transform trans)
    {
        if (!alive) return;
        int damage = Mathf.Clamp(amount - DamageReduction, 1, currentHP);

        Vector3 targetDir = trans.position - gameObject.transform.position;
        float attackAngle = Vector3.Angle(targetDir, gameObject.transform.forward);
        if (isBlocking && (attackAngle <= blockAngle))
        {
            SpendStamina(damage);
        } 
        else
        {
            if (attackAngle > flankAngle) damage = Mathf.FloorToInt(damage * flankMod);
            OnTakeDamage?.Invoke(true);
            ChangeHP(amount * -1);
        }
    }

    public void HealStamina(int amount)
    {
        currentStamina += Mathf.Clamp(amount, 0, maxStamina);
        if (StaminaRect != null) StaminaRect.localScale = new Vector3(GetStaminaFraction(), 1f, 1f);
    }

    public void SpendStamina(int amount)
    {
        currentStamina -= Mathf.Clamp(amount, 0, maxStamina);
        if (StaminaRect != null) StaminaRect.localScale = new Vector3(GetStaminaFraction(), 1f, 1f);
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

    public void SetStats(int _brawn, int _brains, int _guts, int _guile)
    {
        brawn = _brawn;
        brains = _brains;
        guts = _guts;
        guile = _guile;
        InstantiateStats();
    }

    public void SetBlockAngle(float angle)
    {
        blockAngle = angle;
    }
}

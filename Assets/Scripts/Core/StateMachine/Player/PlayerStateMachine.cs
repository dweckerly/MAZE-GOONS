using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    [field: SerializeField] public Animator animator { get; private set; }
    [field: SerializeField] public AnimationMaskHandler animationMask { get; private set; }
    [field: SerializeField] public ArmorHandler ArmorHandler { get; private set; }
    [field: SerializeField] public CharacterController Controller { get; private set; }
    [field: SerializeField] public InputReader InputReader { get; private set; }
    [field: SerializeField] public Targeter Targeter { get; private set; }
    [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }
    [field: SerializeField] public Attributes Attributes { get; private set; }
    [field: SerializeField] public WeaponHandler WeaponHandler { get; private set; }
    //[field: SerializeField] public Ragdoll Ragdoll { get; private set; }
    [field: SerializeField] public Interacter Interacter { get; private set; }
    [field: SerializeField] public Notifier Notifier { get; private set; }
    [field: SerializeField] public Inventory Inventory { get; private set; }
    [field: SerializeField] public ParticleSystem DodgeParticles { get; private set; }
    [field: SerializeField] public ParticleSystem BloodParticles { get; private set; }
    [field:SerializeField] public float JumpForce { get; private set; }

    public Transform MainCameraTransform { get; private set; }

    public float freeLookSpeed = 5f;
    public float sneakSpeed = 2.5f;
    public float targetingSpeed = 5f;
    public float rotationDamping = 10f;
    public float dodgeDistance = 2.5f;
    public float dodgeDuration = 0.2f;
    public int dodgeStaminaReq = 10;
    public float blockCooldown = 1f;
    public bool canBlock = true;
    private float defaultBlockAngle = 60f;

    public bool sneaking = false;

    private void Awake() 
    {
        MainCameraTransform = Camera.main.transform;
        Inventory.carryCapacity = Attributes.CalculateCarryWeight();    
    }
    private void Start() 
    {
        DodgeParticles.Stop();
        Inventory.carryCapacity = Attributes.CalculateCarryWeight();
        SwitchState(new PlayerFreeLookState(this));    
    }

    private void OnEnable()
    {
        Attributes.OnTakeDamage += HandleTakeDamage;
        Attributes.OnDie += HandleDie;
        WeaponHandler.OnEquip += HandleEquip;
        WeaponHandler.OnEquipShield += HandleShieldEquip;
        WeaponHandler.OnUnEquipShield += HandleShieldUnEquip;
        ArmorHandler.EquipArmorEvent += HandleArmorEquip;
        ArmorHandler.UnEquipArmorEvent += HandleArmorUnEquip;
    }

    private void OnDisable()
    {
        Attributes.OnTakeDamage -= HandleTakeDamage;
        Attributes.OnDie -= HandleDie;
        WeaponHandler.OnEquip -= HandleEquip;
        WeaponHandler.OnEquipShield -= HandleShieldEquip;
        WeaponHandler.OnUnEquipShield -= HandleShieldUnEquip;
        ArmorHandler.EquipArmorEvent -= HandleArmorEquip;
        ArmorHandler.UnEquipArmorEvent -= HandleArmorUnEquip;
    }

    public void HandleTakeDamage(bool impact)
    {
        BloodParticles.Play();
        if (impact)
            SwitchState(new PlayerImpactState(this));
    }

    private void HandleDie()
    {
        BloodParticles.Play();
        SwitchState(new PlayerDeadState(this));
    }

    private void HandleEquip()
    {
        for(int i = 0; i < animator.layerCount; i++)
        {
            animationMask.ApplyLayerWeight(animator, i, false);
        }
        foreach(int j in WeaponHandler.maskLayers)
        {
            animationMask.ApplyLayerWeight(animator, j, true);
        }
    }

    private void HandleShieldEquip()
    {
        Attributes.DamageReduction += WeaponHandler.shieldEquipped.DamageReduction;
        Attributes.SetBlockAngle(WeaponHandler.shieldEquipped.blockingAngle);
    }

    private void HandleShieldUnEquip()
    {
        Attributes.DamageReduction -= WeaponHandler.shieldEquipped.DamageReduction;
        Attributes.SetBlockAngle(defaultBlockAngle);
    }

    private void HandleArmorEquip(Armor armor)
    {
        Attributes.DamageReduction = ArmorHandler.CalculateArmorValue();
    }

    private void HandleArmorUnEquip(Armor armor)
    {
        Attributes.DamageReduction = ArmorHandler.CalculateArmorValue();
    }

    public IEnumerator BlockCooldown()
    {
        yield return new WaitForSeconds(blockCooldown);
        canBlock = true;
    }
}

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
    [field: SerializeField] public Inventory Inventory { get; private set; }
    [field: SerializeField] public ParticleSystem DodgeParticles { get; private set; }

    public Transform MainCameraTransform { get; private set; }

    public float freeLookSpeed = 5f;
    public float targetingSpeed = 5f;
    public float rotationDamping = 10f;
    public float dodgeDistance = 2.5f;
    public float dodgeDuration = 0.2f;

    private void Awake() 
    {
        MainCameraTransform = Camera.main.transform;    
    }
    private void Start() 
    {
        DodgeParticles.Stop();
        SwitchState(new PlayerFreeLookState(this));    
    }

    private void OnEnable()
    {
        Attributes.OnTakeDamage += HandleTakeDamage;
        Attributes.OnDie += HandleDie;
        WeaponHandler.OnEquip += HandleEquip;
        ArmorHandler.EquipArmorEvent += HandleArmorEquip;
        ArmorHandler.UnEquipArmorEvent += HandleArmorUnEquip;
    }

    private void OnDisable()
    {
        Attributes.OnTakeDamage -= HandleTakeDamage;
        Attributes.OnDie -= HandleDie;
        WeaponHandler.OnEquip -= HandleEquip;
        ArmorHandler.EquipArmorEvent -= HandleArmorEquip;
        ArmorHandler.UnEquipArmorEvent -= HandleArmorUnEquip;
    }

    private void HandleTakeDamage()
    {
        SwitchState(new PlayerImpactState(this));
    }

    private void HandleDie()
    {
        SwitchState(new PlayerDeadState(this));
    }

    private void HandleEquip()
    {
        if (WeaponHandler.currentWeapon.maskLayer == 0)
        {
            animationMask.ApplyLayerWeight(animator, 1, false);
            animationMask.ApplyLayerWeight(animator, 2, false);
        }
        else 
        {
            animationMask.ApplyLayerWeight(animator, WeaponHandler.currentWeapon.maskLayer, true);
        }
        if (WeaponHandler.currentWeapon.rightHanded) animationMask.ApplyLayerWeight(animator, 2, true);
    }

    private void HandleArmorEquip(Armor armor)
    {
        Attributes.DamageReduction = ArmorHandler.CalculateArmorValue();
    }

    private void HandleArmorUnEquip(Armor armor)
    {
        Attributes.DamageReduction = ArmorHandler.CalculateArmorValue();
    }
}

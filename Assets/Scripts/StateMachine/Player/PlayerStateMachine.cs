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
    }

    private void OnDisable()
    {
        Attributes.OnTakeDamage -= HandleTakeDamage;
        Attributes.OnDie -= HandleDie;
        WeaponHandler.OnEquip -= HandleEquip;
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
        if (WeaponHandler.currentWeapon.rightHanded && WeaponHandler.currentWeapon.maskLayer > 0)
        {
            animationMask.ApplyLayerWeight(animator, 2, true);
            animationMask.ApplyLayerWeight(animator, WeaponHandler.currentWeapon.maskLayer, true);
        }
        else 
        {
            animationMask.ApplyLayerWeight(animator, 2, false);
            animationMask.ApplyLayerWeight(animator, WeaponHandler.currentWeapon.maskLayer, false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMachine : StateMachine
{
    [field: SerializeField] public Animator animator { get; private set; }
    [field: SerializeField] public AnimationMaskHandler animationMask { get; private set; }
    [field: SerializeField] public ArmorHandler ArmorHandler { get; private set; }
    [field: SerializeField] public Attributes Attributes { get; private set; }
    [field: SerializeField] public CharacterController Controller { get; private set; }
    [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }
    [field: SerializeField] public NavMeshAgent Agent { get; private set; }
    [field: SerializeField] public WeaponHandler WeaponHandler { get; private set; }
    [field: SerializeField] public Target Target { get; private set; }
    [field: SerializeField] public Loot Loot { get; private set; }
    //[field: SerializeField] public Ragdoll Ragdoll { get; private set; }

    [field: SerializeField] public float MovementSpeed { get; private set; }
    [field: SerializeField] public float DetectionRange { get; private set; }
    [field: SerializeField] public float AttackRange { get; private set; } = 1f;
    [field: SerializeField] public ParticleSystem BloodParticles { get; private set; }

    public float scaleFactor;
    public float animSpeed;

    public GameObject Player { get; private set; }

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        Agent.updatePosition = false;
        Agent.updateRotation = false;
        AttackRange *= scaleFactor;
        animSpeed = 1 / scaleFactor;
        Loot.enabled = false;
        SwitchState(new EnemyIdleState(this));
    }

    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, DetectionRange);    
    }

    private void OnEnable() 
    {
        Attributes.OnTakeDamage += HandleTakeDamage;
        Attributes.OnDie += HandleDie;
        WeaponHandler.OnEquip += HandleWeaponEquip;
        ArmorHandler.EquipArmorEvent += HandleArmorEquip;
    }

    private void OnDisable() 
    {
        Attributes.OnTakeDamage -= HandleTakeDamage;
        Attributes.OnDie -= HandleDie;
        WeaponHandler.OnEquip -= HandleWeaponEquip;
        ArmorHandler.EquipArmorEvent -= HandleArmorEquip;
    }

    private void HandleTakeDamage()
    {
        BloodParticles.Play();
        SwitchState(new EnemyImpactState(this));
    }

    private void HandleDie()
    {
        SwitchState(new EnemyDeadState(this));
    }

    private void HandleWeaponEquip()
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
        if (WeaponHandler.currentWeapon != WeaponHandler.defaultWeapon) Loot.items.Add(WeaponHandler.currentWeapon);
    }

    private void HandleArmorEquip(Armor armor)
    {
        Attributes.DamageReduction = ArmorHandler.CalculateArmorValue();
        Loot.items.Add(armor);
    }
}

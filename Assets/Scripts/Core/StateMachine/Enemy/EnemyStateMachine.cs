using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyType
{
    Default,
    SunWorshipper,
    Trog
}

public class EnemyStateMachine : StateMachine
{
    public EnemyType enemyType;
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
    [field: SerializeField] public List<Renderer> Renderers = new List<Renderer>();

    public float scaleFactor = 1;
    public float animSpeed = 1;

    public GameObject Player { get; private set; }
    public PlayerStateMachine playerStateMachine;

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        playerStateMachine = Player.GetComponent<PlayerStateMachine>();
        Agent.updatePosition = false;
        Agent.updateRotation = false;
        AttackRange *= scaleFactor;
        animSpeed = 1 / scaleFactor;
        switch (enemyType)
        {
            case EnemyType.SunWorshipper:
                SwitchState(new EnemySunStareState(this));
                break;
            default:
                SwitchState(new EnemyIdleState(this));
                break;
        }
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

    private void HandleTakeDamage(bool impact)
    {
        BloodParticles.Play();
        if (impact)
            SwitchState(new EnemyImpactState(this));
    }

    private void HandleDie()
    {
        BloodParticles.Play();
        SwitchState(new EnemyDeadState(this));
    }

    private void HandleWeaponEquip()
    {
        for (int i = 0; i < animator.layerCount; i++)
        {
            animationMask.ApplyLayerWeight(animator, i, false);
        }
        foreach (int j in WeaponHandler.maskLayers)
        {
            animationMask.ApplyLayerWeight(animator, j, true);
        }
        if (WeaponHandler.mainHandWeapon.lootable) Loot.items.Add(WeaponHandler.mainHandWeapon);
    }

    private void HandleArmorEquip(Armor armor)
    {
        Attributes.DamageReduction = ArmorHandler.CalculateArmorValue();
        Loot.items.Add(armor);
    }
}

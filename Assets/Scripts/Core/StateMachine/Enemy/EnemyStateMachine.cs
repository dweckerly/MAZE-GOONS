using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyType
{
    Coward,
    Default,
    Worshipper,
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
    [field: SerializeField] public float DetectionAngle { get; private set; } = 85f;
    [field: SerializeField] public float AttackRange { get; private set; } = 1f;
    [field: SerializeField] public ParticleSystem BloodParticles { get; private set; }
    [field: SerializeField] public List<Renderer> Renderers = new List<Renderer>();
    [field: SerializeField] public PatrolPath PatrolPath { get; private set; }

    public float scaleFactor = 1;
    public float animSpeed = 1;
    private float shoutDistance = 10f;

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
        if (PatrolPath != null) SwitchState(new EnemyPatrollingState(this, 0));
        else SwitchState(new EnemyIdleState(this));
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
        WeaponHandler.OnEquipShield += HandleShieldEquip;
        ArmorHandler.EquipArmorEvent += HandleArmorEquip;
        if (!Attributes.alive) SwitchState(new EnemyDeadState(this));
    }

    private void OnDisable() 
    {
        Attributes.OnTakeDamage -= HandleTakeDamage;
        Attributes.OnDie -= HandleDie;
        WeaponHandler.OnEquip -= HandleWeaponEquip;
        WeaponHandler.OnEquipShield -= HandleShieldEquip;
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
        if (WeaponHandler.mainHandWeapon.lootable) 
        {
            List<GameObject> prefabs = new List<GameObject>();
            prefabs.Add(WeaponHandler.mainHandPrefab);
            Loot.AddItem(new LootItem(WeaponHandler.mainHandWeapon, prefabs));
        }
    }

    private void HandleArmorEquip(Armor armor)
    {
        Attributes.DamageReduction = ArmorHandler.CalculateArmorValue();
        //Loot.AddItem(armor);
    }

    private void HandleShieldEquip()
    {
        Attributes.DamageReduction = ArmorHandler.CalculateArmorValue();
        List<GameObject> prefabs = new List<GameObject>();
        prefabs.Add(WeaponHandler.offHandPrefab);
        Loot.AddItem(new LootItem(WeaponHandler.shieldEquipped, prefabs));
    }

    public void AlertNearbyEnemies()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, shoutDistance, Vector3.up);
        foreach(RaycastHit hit in hits)
        {
            EnemyStateMachine esm = hit.collider.GetComponent<EnemyStateMachine>();
            if(esm != null && esm.Attributes.alive) Alert(esm);
        }
    }

    private void Alert(EnemyStateMachine esm)
    {
        esm.SwitchState(new EnemyChasingState(esm));
    }
}

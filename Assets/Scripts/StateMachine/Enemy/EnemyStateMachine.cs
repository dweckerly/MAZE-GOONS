using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMachine : StateMachine
{
    [field: SerializeField] public Animator animator { get; private set; }
    [field: SerializeField] public Attributes Attributes { get; private set; }
    [field: SerializeField] public CharacterController Controller { get; private set; }
    [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }
    [field: SerializeField] public NavMeshAgent Agent { get; private set; }
    [field: SerializeField] public WeaponHandler WeaponHandler { get; private set; }
    [field: SerializeField] public Target Target { get; private set; }
    //[field: SerializeField] public Ragdoll Ragdoll { get; private set; }

    [field: SerializeField] public float MovementSpeed { get; private set; }
    [field: SerializeField] public float DetectionRange { get; private set; }
    [field: SerializeField] public float AttackRange { get; private set; }

    public GameObject Player { get; private set; }

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        Agent.updatePosition = false;
        Agent.updateRotation = false;
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
    }

    private void OnDisable() 
    {
        Attributes.OnTakeDamage -= HandleTakeDamage;
        Attributes.OnDie -= HandleDie;
    }

    private void HandleTakeDamage()
    {
        SwitchState(new EnemyImpactState(this));
    }

    private void HandleDie()
    {
        SwitchState(new EnemyDeadState(this));
    }
}

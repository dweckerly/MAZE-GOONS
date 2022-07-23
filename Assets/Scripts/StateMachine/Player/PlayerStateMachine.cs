using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    [field: SerializeField] public Animator animator { get; private set; }
    [field: SerializeField] public CharacterController Controller { get; private set; }
    [field: SerializeField] public InputReader InputReader { get; private set; }
    [field: SerializeField] public Targeter Targeter { get; private set; }
    [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }
    [field: SerializeField] public Attributes Attributes { get; private set; }
    [field: SerializeField] public WeaponHandler WeaponHandler { get; private set; }
    [field: SerializeField] public Interacter Interacter { get; private set; }

    public Transform MainCameraTransform { get; private set; }

    public float freeLookSpeed = 5f;
    public float targetingSpeed = 5f;
    public float rotationDamping = 10f;

    private void Awake() 
    {
        MainCameraTransform = Camera.main.transform;    
    }
    private void Start() 
    {
        SwitchState(new PlayerFreeLookState(this));    
    }

    private void OnEnable()
    {
        Attributes.OnTakeDamage += HandleTakeDamage;
    }

    private void OnDisable()
    {
        Attributes.OnTakeDamage -= HandleTakeDamage;
    }

    private void HandleTakeDamage()
    {
        SwitchState(new PlayerImpactState(this));
    }
}

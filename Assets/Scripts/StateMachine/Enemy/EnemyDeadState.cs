using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadState : EnemyBaseState
{
    private readonly int DeadHash = Animator.StringToHash("Dead");
    public EnemyDeadState(EnemyStateMachine _stateMachine) : base(_stateMachine) {}

    public override void Enter()
    {
        stateMachine.WeaponHandler.DisableRightHandCollider();
        if (stateMachine.WeaponHandler.offHandPrefab != null) stateMachine.WeaponHandler.DisableLeftHandCollider();
        
        GameObject.Destroy(stateMachine.Target);
        
        stateMachine.Controller.enabled = false;
        stateMachine.Agent.enabled = false;
        
        stateMachine.animator.SetTrigger(DeadHash);
        stateMachine.Loot.Enable();
    }

    public override void Exit() {}

    public override void Tick(float deltaTime)
    {
        stateMachine.SwitchState(null);
    }
}

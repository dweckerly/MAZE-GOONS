using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadState : EnemyBaseState
{
    private readonly int DeadHash = Animator.StringToHash("Dead");
    public EnemyDeadState(EnemyStateMachine _stateMachine) : base(_stateMachine) {}

    public override void Enter()
    {
        GameObject.Destroy(stateMachine.Target);
        stateMachine.Controller.enabled = false;
        stateMachine.Agent.enabled = false;
        stateMachine.animator.SetTrigger(DeadHash);
        stateMachine.WeaponHandler.DisableRightHandCollider();
        if (stateMachine.WeaponHandler.offHandPrefab != null) stateMachine.WeaponHandler.DisableLeftHandCollider();
        if (stateMachine.Loot != null && stateMachine.Loot.items.Count > 0) stateMachine.Loot.EnableLoot();
    }

    public override void Exit() {}

    public override void Tick(float deltaTime)
    {
        stateMachine.SwitchState(null);
    }
}

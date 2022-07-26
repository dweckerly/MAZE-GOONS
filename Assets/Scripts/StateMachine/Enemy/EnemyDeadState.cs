using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadState : EnemyBaseState
{
    private readonly int DeadHash = Animator.StringToHash("Dead");
    public EnemyDeadState(EnemyStateMachine _stateMachine) : base(_stateMachine) {}

    public override void Enter()
    {
        //stateMachine.Ragdoll.ToggleRagdoll(true);
        stateMachine.WeaponHandler.currentWeapon.weaponPrefab.SetActive(false);
        GameObject.Destroy(stateMachine.Target);
        stateMachine.Controller.enabled = false;
        stateMachine.Agent.enabled = false;
        stateMachine.animator.SetTrigger(DeadHash);
    }

    public override void Exit() {}

    public override void Tick(float deltaTime)
    {
        stateMachine.SwitchState(null);
    }
}

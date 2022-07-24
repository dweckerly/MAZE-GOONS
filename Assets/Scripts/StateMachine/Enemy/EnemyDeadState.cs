using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadState : EnemyBaseState
{
    public EnemyDeadState(EnemyStateMachine _stateMachine) : base(_stateMachine) {}

    public override void Enter()
    {
        //stateMachine.Ragdoll.ToggleRagdoll(true);
        stateMachine.WeaponHandler.currentWeapon.weaponPrefab.SetActive(false);
        GameObject.Destroy(stateMachine.Target);
        stateMachine.animator.SetTrigger("die");
    }

    public override void Exit() {}

    public override void Tick(float deltaTime)
    {
        stateMachine.SwitchState(null);
    }
}

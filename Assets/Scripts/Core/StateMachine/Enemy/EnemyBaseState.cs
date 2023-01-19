using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBaseState : State
{
    protected EnemyStateMachine stateMachine;

    protected const float CrossFadeDuration = 0.1f;

    public EnemyBaseState(EnemyStateMachine _stateMachine)
    {
        stateMachine = _stateMachine;
    }

    protected bool IsInRange(GameObject go, float range)
    {
        float toDistanceSqr = (go.transform.position - stateMachine.transform.position).sqrMagnitude;
        return toDistanceSqr <= range * range;
    }

    protected bool IsInDetectionRange(GameObject go)
    {
        return IsInRange(go, stateMachine.DetectionRange);
    }

    protected bool IsInAttackRange(GameObject go)
    {
        return IsInRange(go, stateMachine.AttackRange * stateMachine.WeaponHandler.mainHandWeapon.weaponLength);
    }

    protected void Move(float deltaTime)
    {
        Move(Vector3.zero, deltaTime);
    }

    protected void Move(Vector3 movement, float deltaTime)
    {
        stateMachine.Controller.Move((movement + stateMachine.ForceReceiver.Movement) * deltaTime);
    }

    protected void FaceTarget(GameObject go)
    {
        if (go == null) return;
        Vector3 lookPosition = go.transform.position - stateMachine.transform.position;
        lookPosition.y = 0f;
        stateMachine.transform.rotation = Quaternion.LookRotation(lookPosition);
    }
}

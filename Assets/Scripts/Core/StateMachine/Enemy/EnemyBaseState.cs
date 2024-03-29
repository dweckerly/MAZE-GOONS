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

    protected bool IsInRange(GameObject go, float range, bool checkAngle = false)
    {
        if (checkAngle)
        {
            Vector3 targetDir = go.transform.position - stateMachine.gameObject.transform.position;
            float angle = Vector3.Angle(targetDir, stateMachine.gameObject.transform.forward);
            if (angle > stateMachine.DetectionAngle) return false;
        }
        float toDistanceSqr = (go.transform.position - stateMachine.transform.position).sqrMagnitude;
        return toDistanceSqr <= range * range;
    }

    protected bool IsInDetectionRange(GameObject go)
    {
        if (stateMachine.playerStateMachine.sneaking)
            return IsInRange(go, Mathf.Clamp(stateMachine.DetectionRange - stateMachine.playerStateMachine.Attributes.currentGuile, 0, stateMachine.DetectionRange), true);
        else 
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

    float rotateSpeed = 700f;
    protected void FaceTarget(GameObject go)
    {
        if (go == null) return;
        Vector3 lookPosition = go.transform.position - stateMachine.transform.position;
        lookPosition.y = 0f;
        stateMachine.transform.rotation = Quaternion.RotateTowards(stateMachine.transform.rotation, Quaternion.LookRotation(lookPosition), Time.deltaTime * rotateSpeed);
    }

    protected void FaceAwayFromTarget(GameObject go)
    {
        if (go == null) return;
        Vector3 lookPosition = stateMachine.transform.position - go.transform.position;
        lookPosition.y = 0f;
        stateMachine.transform.rotation = Quaternion.LookRotation(lookPosition);
    }
}

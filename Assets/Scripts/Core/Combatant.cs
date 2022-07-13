using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(AnimationHandler))]
public class Combatant : MonoBehaviour
{
    Health health;
    AnimationHandler animator;
    CombatTarget target;

    float timeSinceLastAttack = Mathf.Infinity;
    [SerializeField] float timeBetweenAttakcs = 1.5f;

    private void Awake() 
    {
        health = GetComponent<Health>();
        animator = GetComponent<AnimationHandler>();
    }

    private void Update() 
    {
        timeSinceLastAttack += Time.deltaTime;
        if (target == null) 
        {
            StopAttack();
            animator.SetAnimationBool("inCombat", false);
            return;
        }
        LookAtTarget(target.transform.position);
        // Vector3 lookAtTarget = target.transform.position;
        // lookAtTarget.y = transform.position.y;
        // transform.LookAt(lookAtTarget);
    }

    public void LookAtTarget(Vector3 target)
    {
        Quaternion targetRotation = Quaternion.LookRotation(target - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5f * Time.deltaTime);
    }

    public void SetTarget(CombatTarget _target)
    {
        target = _target;
        animator.SetAnimationBool("inCombat", true);
    }

    public void TriggerAttack()
    {
        if (timeSinceLastAttack > timeBetweenAttakcs)
        {
            animator.SetAnimationTrigger("attack");
            timeSinceLastAttack = 0;
        }
    }

    public void StopAttack()
    {
        animator.SetAnimationTrigger("stopAttack");
    }
}

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
        if (target == null) return;
        transform.LookAt(target.transform);
    }

    private void TriggerAttack()
    {
        if (timeSinceLastAttack > timeBetweenAttakcs)
        {
            animator.SetAnimationTrigger("attack");
            timeSinceLastAttack = 0;
        }
    }
}

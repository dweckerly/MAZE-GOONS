using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(AnimationHandler))]
public class Combatant : MonoBehaviour
{
    Health health;
    AnimationHandler animator;

    [SerializeField] float timeBetweenAttakcs = 1.5f;
    float timeSinceLastAttack = Mathf.Infinity;

    private void Awake() 
    {
        health = GetComponent<Health>();
        animator = GetComponent<AnimationHandler>();
    }

    private void Update() 
    {
        timeSinceLastAttack += Time.deltaTime;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationHandler : MonoBehaviour
{
    Animator animator;

    private void Awake() 
    {
        animator = GetComponent<Animator>();    
    }

    public void SetAnimationFloat(string name, float amount)
    {
        animator.SetFloat(name, amount);
    }

    public void SetAnimationTrigger(string trigger)
    {
        animator.SetTrigger(trigger);
    }
}

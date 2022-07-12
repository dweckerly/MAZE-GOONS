using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(AnimationHandler))]
public class Combatant : MonoBehaviour
{
    Health health;
    AnimationHandler animator;

    private void Awake() 
    {
        health = GetComponent<Health>();
        animator = GetComponent<AnimationHandler>();
    }


}

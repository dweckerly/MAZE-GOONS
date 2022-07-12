using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(AnimationHandler))]
public class Combatant : MonoBehaviour
{
    Health health;
    AnimationHandler animation;

    private void Awake() 
    {
        health = GetComponent<Health>();
        animation = GetComponent<AnimationHandler>();
    }


}

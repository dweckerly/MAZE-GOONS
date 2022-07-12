using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AnimationHandler))]
public class MovementHandler : MonoBehaviour
{
    [SerializeField] float maxSpeed = 7f;
    AnimationHandler animation;
    NavMeshAgent agent;

    void Awake()
    {
        animation = GetComponent<AnimationHandler>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        UpdateAnimator();
    }

    public void MoveTo(Vector3 dest, float speedFraction = 1f)
    {
        agent.destination = dest;
        agent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
        agent.isStopped = false;
    }

    public void CancelMovement()
    {
        agent.isStopped = true;
    }

    void UpdateAnimator()
    {
        Vector3 localVelocity = transform.InverseTransformDirection(agent.velocity);
        float speedPercent = localVelocity.z / maxSpeed;
        animation.SetAnimationFloat("forwardSpeed", speedPercent);
    }

    public void SetAnimationTrigger(string trigger)
    {
        animation.SetAnimationTrigger(trigger);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovementHandler : MonoBehaviour
{
    [SerializeField] float maxSpeed = 7f;
    Animator animator;
    NavMeshAgent agent;

    void Awake()
    {
        animator = GetComponent<Animator>();
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

    public void StopMoving()
    {
        agent.isStopped = true;
    }

    void UpdateAnimator()
    {
        Vector3 localVelocity = transform.InverseTransformDirection(agent.velocity);
        float speedPercent = localVelocity.z / maxSpeed;
        animator.SetFloat("forwardSpeed", speedPercent);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GenericBehavior : MonoBehaviour
{
    protected NavMeshAgent agent;
    protected MovementHandler mover;

    protected Node root;
    protected GameObject target = null;

    public float viewDistance = 20f;
    public float viewAngle = 90f;
    public float interactionDistance = 5f;

    Node.Status behaviorStatus = Node.Status.RUNNING;

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        mover = GetComponent<MovementHandler>();
    }

    protected Node.Status MoveToTarget(float speedPercent = 1f)
    {
        if (target == null) return Node.Status.FAILURE;
        float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
        if (distanceToTarget < interactionDistance)
        {
            mover.StopMoving();
            return Node.Status.SUCCESS;
        }
        mover.MoveTo(target.transform.position, speedPercent);
        return Node.Status.RUNNING;
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (behaviorStatus != Node.Status.SUCCESS) behaviorStatus = root.Process();
    }
}

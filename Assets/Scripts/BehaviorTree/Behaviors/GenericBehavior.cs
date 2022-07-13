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

    WaitForSeconds behaveCycleTime;

    public float viewDistance = 20f;
    public float viewAngle = 90f;
    public float interactionDistance = 5f;

    Node.Status behaviorStatus = Node.Status.RUNNING;

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        mover = GetComponent<MovementHandler>();
    }

    protected virtual void Start() 
    {
        root = new Node();
        behaveCycleTime = new WaitForSeconds(Random.Range(0.1f, 0.5f));
        StartCoroutine("Behave");
    }

    public float GetDistanceToTarget()
    {
        return Vector3.Distance(transform.position, target.transform.position);
    }

    public bool isInFieldOfView(Transform target)
    {
        Vector3 targetDirection = target.position - transform.position;
        float angle = Vector3.Angle(targetDirection, transform.forward);
        return angle < (viewAngle / 2);
    }

    public bool IsInInteractionRange()
    {
        if (target == null) return false;
        return GetDistanceToTarget() <= interactionDistance;
    }

    public Node.Status InInteractionRange()
    {
        if (IsInInteractionRange()) return Node.Status.SUCCESS;
        return Node.Status.FAILURE;
    }

    // need to update this to take in speedPercent param
    protected Node.Status MoveToTarget()
    {
        if (target == null) return Node.Status.FAILURE;
        if (IsInInteractionRange())
        {
            mover.StopMoving();
            return Node.Status.SUCCESS;
        }
        mover.MoveTo(target.transform.position);
        return Node.Status.RUNNING;
    }

    IEnumerator Behave()
    {
        while (true)
        {
            behaviorStatus = root.Process();
            yield return behaveCycleTime;
        }
    }
}

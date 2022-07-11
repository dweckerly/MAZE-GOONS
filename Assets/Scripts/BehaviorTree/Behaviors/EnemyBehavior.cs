using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    Node root;
    NavMeshAgent agent;
    GameObject target = null;
    Mover mover;

    [SerializeField] float viewDistance = 30f;
    [SerializeField] float viewAngle = 90f;
    [SerializeField] float interactionDistance = 5f;

    public enum ActionState { IDLE, WORKING }
    ActionState actionState = ActionState.IDLE;

    Node.Status behaviorStatus = Node.Status.RUNNING;

    private void Awake() 
    {
        agent = GetComponent<NavMeshAgent>();
        mover = GetComponent<Mover>();
    }

    void Start()
    {
        root = new Node("Enemy Behavior");
        Leaf lookForPlayer = new Leaf("Look For Player", LookForPlayer);
        Leaf moveToPlayer = new Leaf("Move To Player", MoveToPlayer);
        Leaf attackPlayer = new Leaf("Attack Player", AttackPlayer);

        Sequence detectPlayerSequence = new Sequence("Detect and Attack Player!");

        detectPlayerSequence.AddChild(lookForPlayer);
        detectPlayerSequence.AddChild(moveToPlayer);
        detectPlayerSequence.AddChild(attackPlayer);
        root.AddChild(detectPlayerSequence);
        root.PrintTree();
        Debug.Log("Starting Look for Player...");
        root.Process();
    }

    public Node.Status LookForPlayer()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, viewDistance);
        foreach (Collider hit in hits)
        {
            if (hit.gameObject.CompareTag("Player") && isInFieldOfView(hit.transform))
            {
                Debug.Log("Look for Player SUCCESS!");
                Debug.Log("Starting Move to Player...");
                target = hit.gameObject;
                return Node.Status.SUCCESS;
            }
        }
        return Node.Status.RUNNING;
    }

    public Node.Status MoveToPlayer()
    {
        Debug.Log("Moving to player!");
        if (target == null) return Node.Status.FAILURE;
        
        float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
        if (distanceToTarget < interactionDistance)
        {
            Debug.Log("Move to Player SUCCESS!");
            Debug.Log("Starting Attack Player...");
            return Node.Status.SUCCESS;
        } 
        mover.MoveTo(target.transform.position);
        return Node.Status.RUNNING;
    }

    public Node.Status AttackPlayer()
    {
        Debug.Log("Attacking player!");
        mover.SetAnimationTrigger("attack");
        float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
        if (distanceToTarget > interactionDistance)
        {
            mover.SetAnimationTrigger("stopAttack");
            return Node.Status.FAILURE;
        }
        return Node.Status.RUNNING;
    }

    private bool isInFieldOfView(Transform target)
    {
        Vector3 targetDirection = target.position - transform.position;
        float angle = Vector3.Angle(targetDirection, transform.forward);
        return angle < (viewAngle / 2);
    }

    void Update()
    {
        if(behaviorStatus == Node.Status.RUNNING) behaviorStatus = root.Process();
    }
}

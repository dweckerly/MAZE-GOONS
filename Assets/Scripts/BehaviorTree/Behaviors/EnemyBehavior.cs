using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : GenericBehavior
{
    Combatant combatant;

    protected override void Awake() 
    {
        base.Awake();
        combatant = GetComponent<Combatant>();
    }

    protected override void Start()
    {
        base.Start();

        Leaf lookForEnemy = new Leaf("Look For Enemy", LookForTarget);
        Leaf moveToEnemy = new Leaf("Move To Player", MoveToTarget);
        Sequence detectEnemySequence = new Sequence("Detect Enemy");
        detectEnemySequence.AddChild(lookForEnemy);
        detectEnemySequence.AddChild(moveToEnemy);

        Leaf attackTarget = new Leaf("Attack Target", AttackTarget);

        Selector detectAndAttackSelector = new Selector("Detect and Attack");
        detectAndAttackSelector.AddChild(attackTarget);
        detectAndAttackSelector.AddChild(detectEnemySequence);
        

        root.AddChild(detectAndAttackSelector);
        root.PrintTree();
        root.Process();
    }

    public Node.Status LookForTarget()
    {
        if (target != null) return Node.Status.SUCCESS;
        Collider[] hits = Physics.OverlapSphere(transform.position, viewDistance);
        foreach (Collider hit in hits)
        {
            if (hit.gameObject.CompareTag("Player") && isInFieldOfView(hit.transform))
            {
                target = hit.gameObject;
                combatant.SetTarget(target.GetComponent<Attributes>());
                return Node.Status.SUCCESS;
            }
        }
        return Node.Status.FAILURE;
    }

    public Node.Status AttackTarget()
    {
        if (target == null || !IsInInteractionRange())
        {
            combatant.StopAttack();
            return Node.Status.FAILURE;
        }
        // if target is dead then return SUCCESS
        combatant.TriggerAttack();
        return Node.Status.RUNNING;
    }
}

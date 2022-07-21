using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : GenericBehavior
{
    Combatant combatant;
    Attributes attributes;

    protected override void Awake() 
    {
        base.Awake();
        combatant = GetComponent<Combatant>();
        attributes = GetComponent<Attributes>();
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

        Leaf alive = new Leaf("Am I alive?", IsAlive);
        DependencySequence aliveSeq = new DependencySequence("Alive and can act", alive);
        aliveSeq.AddChild(detectAndAttackSelector);

        root.AddChild(aliveSeq);
        root.PrintTree();
        root.Process();
    }

    public Node.Status IsAlive()
    {
        if (attributes.alive) return Node.Status.SUCCESS;
        combatant.RemoveTarget();
        StopBehavior();
        return Node.Status.FAILURE;
    }

    public Node.Status LookForTarget()
    {
        if (target != null) return Node.Status.SUCCESS;
        Collider[] hits = Physics.OverlapSphere(transform.position, viewDistance);
        foreach (Collider hit in hits)
        {
            if (hit.gameObject.CompareTag("Player") && isInFieldOfView(hit.transform))
            {
                target = hit.GetComponent<Attributes>();
                if (!target.alive) 
                {
                    target = null;
                    return Node.Status.FAILURE;
                }
                combatant.SetTarget(target);
                return Node.Status.SUCCESS;
            }
        }
        return Node.Status.FAILURE;
    }

    public Node.Status AttackTarget()
    {
        if (target == null || !target.alive || !IsInInteractionRange())
        {
            combatant.StopAttack();
            return Node.Status.FAILURE;
        }
        combatant.TriggerAttack();
        return Node.Status.RUNNING;
    }
}

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


        Leaf inRangeForAttack = new Leaf("In Range For Attack", InInteractionRange);
        Leaf attackTarget = new Leaf("Attack Target", AttackTarget);
        Sequence attackTargetSequence = new Sequence("Attack Target Sequence");
        attackTargetSequence.AddChild(inRangeForAttack);
        attackTargetSequence.AddChild(attackTarget);

        Selector detectAndAttackSelector = new Selector("Detect and Attack");
        detectAndAttackSelector.AddChild(detectEnemySequence);
        detectAndAttackSelector.AddChild(attackTargetSequence);

        root.AddChild(detectAndAttackSelector);
        root.PrintTree();
        root.Process();
    }

    public Node.Status LookForTarget()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, viewDistance);
        foreach (Collider hit in hits)
        {
            if (hit.gameObject.CompareTag("Player") && isInFieldOfView(hit.transform))
            {
                target = hit.gameObject;
                combatant.SetTarget(target.GetComponent<CombatTarget>());
                return Node.Status.SUCCESS;
            }
        }
        target = null;
        return Node.Status.FAILURE;
    }

    public Node.Status AttackTarget()
    {
        if (target == null) return Node.Status.FAILURE;
        combatant.TriggerAttack();
        return Node.Status.SUCCESS;
    }

}

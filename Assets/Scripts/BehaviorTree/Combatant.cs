using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combatant : MonoBehaviour
{
    Animator animator;

    Attributes attributes;
    Attributes target;

    float timeSinceLastAttack = Mathf.Infinity;
    [SerializeField] float timeBetweenAttakcs = 1.5f;

    private void Awake() 
    {
        animator = GetComponent<Animator>();
        attributes = GetComponent<Attributes>();
    }

    private void Update() 
    {
        timeSinceLastAttack += Time.deltaTime;
        if (target == null) 
        {
            StopAttack();
            animator.SetBool("inCombat", false);
            return;
        }
        LookAtTarget(target.transform.position);
    }

    public void LookAtTarget(Vector3 target)
    {
        Quaternion targetRotation = Quaternion.LookRotation(target - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5f * Time.deltaTime);
    }

    public void SetTarget(Attributes _target)
    {
        target = _target;
        animator.SetBool("inCombat", true);
    }

    public void RemoveTarget()
    {
        target = null;
    }

    public void TriggerAttack()
    {
        if (timeSinceLastAttack > timeBetweenAttakcs)
        {
            animator.SetTrigger("attack");
            //target.TakeDamage(attributes.GetStat(Attribute.Brawn));
            timeSinceLastAttack = 0;
        }
    }

    public void StopAttack()
    {
        animator.SetTrigger("stopAttack");
    }
}

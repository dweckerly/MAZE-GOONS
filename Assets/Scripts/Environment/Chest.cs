using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    Animator animator;
    

    private void Awake() 
    {
        animator = GetComponent<Animator>();
    }

    public void OpenChest()
    {
        animator.SetTrigger("OpenChest");
        animator.SetBool("IsOpen", true);
    }
}

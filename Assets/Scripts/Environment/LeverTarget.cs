using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverTarget : MonoBehaviour
{
    Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Open()
    {
         animator.SetBool("IsOpen", true);
    }
}

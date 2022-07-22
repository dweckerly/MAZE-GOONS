using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceReceiver : MonoBehaviour
{
    [SerializeField] private float drag = 0.1f;
    private CharacterController controller;
    private Vector3 impact;
    private Vector3 dampingVelocity;
    private float verticalVelocity;
    public Vector3 Movement => impact + Vector3.up * verticalVelocity; 

    private void Awake() 
    {
        controller = GetComponent<CharacterController>();    
    }

    private void Update() 
    {
        if (controller.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }
        impact = Vector3.SmoothDamp(impact, Vector3.zero, ref dampingVelocity, drag);
    }

    public void AddForce(Vector3 force)
    {
        impact += force;
    }
}

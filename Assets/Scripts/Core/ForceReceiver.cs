using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ForceReceiver : MonoBehaviour
{
    [SerializeField] private float drag = 0.1f;
    [SerializeField] private NavMeshAgent agent;
    private CharacterController controller;
    private Vector3 impact;
    private Vector3 dampingVelocity;
    private float verticalVelocity;
    public Vector3 Movement => impact + Vector3.up * verticalVelocity; 

    private void Awake() 
    {
        agent = GetComponent<NavMeshAgent>();
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
        if (agent != null && impact.sqrMagnitude < 0.2f * 0.2f) 
        {
            impact = Vector3.zero;
            agent.enabled = true;
        }
    }

    public void AddForce(Vector3 force)
    {
        impact += force;
        if (agent != null) agent.enabled = false;
    }

    public void Jump(float jumpForce)
    {
        verticalVelocity += jumpForce;
    }
}

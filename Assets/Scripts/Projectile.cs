using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    public float Speed = 10f;
    public float Lifespan = 3f;

    private Rigidbody rigidBody;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        rigidBody.AddForce(rigidBody.transform.forward * Speed);
        Destroy(gameObject, Lifespan);
    }
}

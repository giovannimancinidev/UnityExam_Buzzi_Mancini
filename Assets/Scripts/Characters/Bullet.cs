using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    public Rigidbody Rb { get; }
    private float launchVelocity = 10.0f;
    private Rigidbody rb;
    public float LaunchVelocity { get{ return launchVelocity; } }

    private void Awake()
    {
    }

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!GetComponent<Renderer>().isVisible)
        {
            rb.velocity = Vector3.zero;
            gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        //rb.AddRelativeForce(new Vector3(0, launchVelocity, 0), ForceMode.Acceleration);
    }
}

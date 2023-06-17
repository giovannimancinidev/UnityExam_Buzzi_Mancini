using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    public Rigidbody Rb { get; }
    private float launchVelocity = 30.0f;
    private Rigidbody rb;
    public float LaunchVelocity { get{ return launchVelocity; } }

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

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Surface") || collision.gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false);

            if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Player"))
            {
                collision.GetComponent<Actor>().AddDamage(25f);
            }
        }
    }
}

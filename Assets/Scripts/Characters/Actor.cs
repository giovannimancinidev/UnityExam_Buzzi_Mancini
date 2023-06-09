using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Actor : MonoBehaviour
{
    protected float energy;
    protected BulletsManager bulletsManager;

    public float Energy { get { return energy; } set { energy = value; } }

    protected virtual void Awake()
    {
        bulletsManager = FindAnyObjectByType<BulletsManager>();
    }

    protected virtual void Shoot(Transform spawnBulletPoint)
    {
        GameObject b;
        b = bulletsManager.GetBullet();

        if (b != null)
        {
            b.transform.position = spawnBulletPoint.position;

            b.SetActive(true);

            b.GetComponent<Rigidbody>().AddForce(spawnBulletPoint.TransformDirection(Vector3.forward * b.GetComponent<Bullet>().LaunchVelocity), ForceMode.Impulse);
        }
    }

    public void AddDamage(float damage)
    {
        energy -= damage;

        if (energy == 0)
        {
            Death();
        }
    }

    protected virtual void Death()
    {
    }
}

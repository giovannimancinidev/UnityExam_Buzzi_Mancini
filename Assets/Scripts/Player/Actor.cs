using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Actor : MonoBehaviour
{
    protected float energy;
    protected BulletsManager bulletsManager;

    protected virtual void Awake()
    {
        bulletsManager = FindAnyObjectByType<BulletsManager>();
    }
}

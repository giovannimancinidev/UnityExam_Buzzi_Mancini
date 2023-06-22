using UnityEngine;
using System;

public abstract class LeverBase : MonoBehaviour
{
    protected bool isActive = false;
    
    void OnEnable()
    {
        ButtonScript.OnButtonHit += Activate;
    }

    void OnDisable()
    {
        ButtonScript.OnButtonHit -= Activate;
    }
    
    public abstract void Activate();
}
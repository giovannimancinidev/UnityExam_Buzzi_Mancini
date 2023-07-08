using UnityEngine;
using System;

public abstract class LeverBase : MonoBehaviour
{
    public ButtonScript ButtonRef;
    protected bool isActive = false;

    void OnEnable()
    {
        ButtonRef.OnButtonHit += Activate;
    }

    void OnDisable()
    {
        ButtonRef.OnButtonHit -= Activate;
    }
    
    public abstract void Activate();
}
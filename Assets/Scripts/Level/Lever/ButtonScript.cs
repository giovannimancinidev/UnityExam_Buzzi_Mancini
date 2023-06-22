using UnityEngine;
using System;

public class ButtonScript : MonoBehaviour
{
    public static event Action OnButtonHit;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            OnButtonHit?.Invoke();
        }
    }
}
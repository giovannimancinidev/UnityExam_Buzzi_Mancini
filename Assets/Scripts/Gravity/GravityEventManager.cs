using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityEventManager : MonoBehaviour
{
    public GravityInvertEvent onGravityInvert;
    public GameObject CanvasRef;
    public float minTime = 5f;
    public float maxTime = 10f;

    public static bool InputForEvent;

    void Start()
    {
        //StartCoroutine(InvokeGravityInvertRandomly());
    }

    private void Update()
    {
        if (InputForEvent)
        {
            InputForEvent = false;
            DebugGravityInvert();
        }
    }

    IEnumerator InvokeGravityInvertRandomly()
    {
        //float rndm = Random.Range(minTime, maxTime);
        //float noticeTime = rndm - 4;
        yield return new WaitForSeconds(4);
        bool isGravityInverted = Physics.gravity.y < 0;
        Physics.gravity = -Physics.gravity;
        onGravityInvert.Invoke(isGravityInverted);
        StartCoroutine(InvokeGravityInvertRandomly());
    }

    public void DebugGravityInvert()
    {
        bool isGravityInverted = Physics.gravity.y < 0;
        Physics.gravity = -Physics.gravity;
        onGravityInvert.Invoke(isGravityInverted);
    }
}


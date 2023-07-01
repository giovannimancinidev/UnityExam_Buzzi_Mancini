using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityEventManager : MonoBehaviour
{
    public GravityInvertEvent onGravityInvert;
    public GameObject CanvasRef;
    public float minTime = 5f;
    public float maxTime = 10f;

    void Start()
    {
        StartCoroutine(InvokeGravityInvertRandomly());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            DebugGravityInvert();
        }
    }

    IEnumerator InvokeGravityInvertRandomly()
    {
        float rndm = Random.Range(minTime, maxTime);
        float noticeTime = rndm - 4;
        yield return new WaitForSeconds(noticeTime);
        CanvasRef.SetActive(true);
        yield return new WaitForSeconds(4);
        CanvasRef.SetActive(false);
        bool isGravityInverted = Physics.gravity.y < 0;
        Physics.gravity = -Physics.gravity;
        onGravityInvert.Invoke(isGravityInverted);
        StartCoroutine(InvokeGravityInvertRandomly());
    }

    private void DebugGravityInvert()
    {
        bool isGravityInverted = Physics.gravity.y < 0;
        Physics.gravity = -Physics.gravity;
        onGravityInvert.Invoke(isGravityInverted);
    }
}


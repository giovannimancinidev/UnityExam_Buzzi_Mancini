using System.Collections;
using UnityEngine;

public class GravityEventManager : MonoBehaviour
{
    [Header ("References")]
    public GravityInvertEvent onGravityInvert;
    public GameObject CanvasRef;

    [Header("Parameters")]
    public float minTime = 5f;
    public float maxTime = 10f;
    public static bool InputForEvent;

    private void Update()
    {
        if (InputForEvent)
        {
            InputForEvent = false;
            GravityInvert();
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

    public void GravityInvert()
    {
        bool isGravityInverted = Physics.gravity.y < 0;
        Physics.gravity = -Physics.gravity;
        onGravityInvert.Invoke(isGravityInverted);
    }
}


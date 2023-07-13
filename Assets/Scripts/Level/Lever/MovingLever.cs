using UnityEngine;
using System.Collections;

public class MovingLever : LeverBase
{
    [Header("Lever Parameters")]
    public Vector3 MoveAmount = new Vector3(0, 0, -1);
    public float MoveSpeed = 1f;
    public float Delay = 2f;
    public bool StayOpen;

    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private bool isReturning = false, isCoroutineRunning = false;

    void Start()
    {
        initialPosition = transform.localPosition;
        targetPosition = initialPosition + MoveAmount;
    }

    void Update()
    {
        if (isActive)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPosition, Time.deltaTime * MoveSpeed);
            if (Vector3.Distance(transform.localPosition, targetPosition) < 0.01f)
            {
                isActive = false;
                if (!StayOpen)
                {
                    if (!isCoroutineRunning)
                    {
                        StartCoroutine(StartReturn());
                    }
                }
            }
        }
        else if (isReturning)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, initialPosition, Time.deltaTime * MoveSpeed);
            if (Vector3.Distance(transform.localPosition, initialPosition) < 0.01f)
            {
                isReturning = false;
            }
        }
    }

    IEnumerator StartReturn()
    {
        isCoroutineRunning = true;
        yield return new WaitForSeconds(Delay);
        isReturning = true;
        isCoroutineRunning = false;
    }

    public override void Activate()
    {
        if (!isActive && !isReturning)
        {
            isActive = true;
        }
    }
}
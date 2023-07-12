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
        initialPosition = transform.position;
        targetPosition = initialPosition + MoveAmount;
    }

    void Update()
    {
        if (isActive)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * MoveSpeed);
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
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
            transform.position = Vector3.MoveTowards(transform.position, initialPosition, Time.deltaTime * MoveSpeed);
            if (Vector3.Distance(transform.position, initialPosition) < 0.01f)
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
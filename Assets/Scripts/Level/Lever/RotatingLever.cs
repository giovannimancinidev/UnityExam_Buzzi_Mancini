using UnityEngine;
using System.Collections;

public class RotatingLever : LeverBase
{
    public float rotationAmount = 90f;
    public float rotationSpeed = 90f;
    public float delay = 2f;  
    private Quaternion initialRotation;
    private Quaternion targetRotation;
    private bool isReturning = false;
    private bool isCoroutineRunning = false;

    void Start()
    {
        initialRotation = transform.rotation;
        targetRotation = initialRotation * Quaternion.Euler(new Vector3(rotationAmount, 0, 0));
    }

    void Update()
    {
        if (isActive)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.01f)
            {
                isActive = false;
                if (!isCoroutineRunning)
                {
                    StartCoroutine(StartReturn());
                }
            }
        }
        else if (isReturning)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, initialRotation, Time.deltaTime * rotationSpeed);
            if (Quaternion.Angle(transform.rotation, initialRotation) < 0.01f)
            {
                isReturning = false;
            }
        }
    }

    IEnumerator StartReturn()
    {
        isCoroutineRunning = true;
        yield return new WaitForSeconds(delay);
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
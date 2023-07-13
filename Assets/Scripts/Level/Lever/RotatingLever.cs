using UnityEngine;

public class RotatingLever : LeverBase
{
    [Header ("Lever Parameters")]
    public float rotationAmount = 90f;
    public float rotationSpeed = 90f;

    private Quaternion initialRotation;
    private Quaternion targetRotation;

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
                Quaternion temp = initialRotation;
                initialRotation = targetRotation;
                targetRotation = temp;
            }
        }
    }

    public override void Activate()
    {
        if (!isActive && Quaternion.Angle(transform.rotation, initialRotation) < 0.01f)
        {
            isActive = true;
        }
    }
}
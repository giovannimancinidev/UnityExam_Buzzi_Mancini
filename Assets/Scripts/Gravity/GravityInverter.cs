using UnityEngine;
using System.Collections;

public class GravityInverter : MonoBehaviour
{
    private Transform feetPosition;
    public float rotationSpeed = 1.8f;
    public float delayBeforeRotation = 0.3f;

    private GravityEventManager gravityMngr;
    private Rigidbody rb;
    private bool shouldRotate = false;
    public bool ShouldRotate
    {
        get { return shouldRotate; }
    }
    public static bool isGravityInverted = false;

    void Start()
    {
        feetPosition = GetComponent<Transform>();

        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = rb.transform.InverseTransformPoint(feetPosition.position);

        gravityMngr = FindObjectOfType<GravityEventManager>();
        gravityMngr.onGravityInvert.AddListener(HandleGravityInvert);
    }

    private void OnDisable()
    {
        gravityMngr.onGravityInvert.RemoveListener(HandleGravityInvert);
    }

    void HandleGravityInvert(bool isInverted)
    {
        isGravityInverted = isInverted;
        StartCoroutine(DelayRotation());
    }

    void Update()
    {
        if (shouldRotate)
        {
            RotateCharacter();
        }
    }

    IEnumerator DelayRotation()
    {
        shouldRotate = false;
        yield return new WaitForSeconds(delayBeforeRotation);
        shouldRotate = true;
    }

    void RotateCharacter()
    {
        float targetZRotation = isGravityInverted ? 180.1f : 0f;
        float zRotation = Mathf.LerpAngle(transform.eulerAngles.z, targetZRotation, rotationSpeed * Time.deltaTime);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, zRotation);

        if (Mathf.Abs(targetZRotation - transform.eulerAngles.z) < 0.1f)
        {
            shouldRotate = false;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonWeight : MonoBehaviour
{
    public float distanceDownPerMass = 1f / 3f;
    public float maxDownDistance = 1f;
    private Vector3 originalPosition;
    private HashSet<GameObject> objectsOnButton;

    void Start()
    {
        originalPosition = transform.position;
        objectsOnButton = new HashSet<GameObject>();
    }

    void FixedUpdate()
    {
        float totalMass = 0f;
        foreach (GameObject obj in objectsOnButton)
        {
            Rigidbody objRb = obj.GetComponent<Rigidbody>();
            if (objRb != null)
            {
                totalMass += objRb.mass;
            }
        }
        
        float downDistance = Mathf.Min(distanceDownPerMass * totalMass, maxDownDistance);
        Vector3 targetPosition = originalPosition - new Vector3(0, downDistance, 0);
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.fixedDeltaTime * 5);
    }

    void OnTriggerEnter(Collider other)
    {
        objectsOnButton.Add(other.gameObject);
    }

    void OnTriggerExit(Collider other)
    {
        objectsOnButton.Remove(other.gameObject);
    }
}
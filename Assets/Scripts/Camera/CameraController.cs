using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Settings")]
    public Transform Target;
    public float CameraSpeed;
    public float Zdistance;

    [Header("Confiner")]
    public float LeftLimit;
    public float RightLimit;
    public float TopLimit;
    public float BottomLimit;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Target != null)
        {
            float blendFactor = Time.deltaTime * CameraSpeed;
            Vector3 v = Vector3.Lerp(transform.position, Target.position, blendFactor);
            transform.position = new Vector3(Zdistance, Mathf.Clamp(v.y, BottomLimit, TopLimit), 
                                             Mathf.Clamp(v.z, LeftLimit, RightLimit));
        }
    }
}

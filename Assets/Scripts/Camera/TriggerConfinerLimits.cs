using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerConfinerLimits : MonoBehaviour
{
    [Header ("Settings")]
    public CameraController CameraRef;
    public float EnemyLeftLimit, EnemyRightLimit, EnemyTopLimit, EnemyBottomLimit;
    
    private float previousLeftLimit, previousRightLimit, previousTopLimit, previousBottomLimit;
    private bool fromRight;

    private void Start()
    {
        previousLeftLimit = CameraRef.LeftLimit;
        previousTopLimit = CameraRef.TopLimit;
        previousRightLimit = CameraRef.RightLimit;
        previousBottomLimit = CameraRef.BottomLimit;
    }

    private void OnTriggerEnter(Collider other)
    {
        //DETECT ON WHICH SIDE PLAYER ENTERS
        if (other.gameObject.CompareTag("Player") && other.gameObject.transform.position.z > gameObject.transform.position.z)
        {
            fromRight = true;
        }
        else
        {
            fromRight = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //UPDATE CONFINER VALUES IN ORDER TO NEW MAP AREA
        if (other.gameObject.CompareTag("Player") )
        {
            if ((fromRight && other.gameObject.transform.position.z < gameObject.transform.position.z) || (!fromRight&& other.gameObject.transform.position.z > gameObject.transform.position.z))
            {
                CameraRef.LeftLimit = CameraRef.LeftLimit == EnemyLeftLimit ? previousLeftLimit : EnemyLeftLimit;
                CameraRef.TopLimit = CameraRef.TopLimit == EnemyTopLimit ? previousTopLimit : EnemyTopLimit;
                CameraRef.RightLimit = CameraRef.RightLimit == EnemyRightLimit ? previousRightLimit : EnemyRightLimit;
                CameraRef.BottomLimit = CameraRef.BottomLimit == EnemyBottomLimit ? previousBottomLimit : EnemyBottomLimit;
            }
        }
    }
}

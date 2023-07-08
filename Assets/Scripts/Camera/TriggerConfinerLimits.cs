using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerConfinerLimits : MonoBehaviour
{
    public CameraController CameraRef;

    public float EnemyLeftLimit, EnemyRightLimit, EnemyTopLimit, EnemyBottomLimit;
    
    private float previousLeftLimit, previousRightLimit, previousTopLimit, previousBottomLimit;

    private void Start()
    {
        previousLeftLimit = CameraRef.LeftLimit;
        previousTopLimit = CameraRef.TopLimit;
        previousRightLimit = CameraRef.RightLimit;
        previousBottomLimit = CameraRef.BottomLimit;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CameraRef.LeftLimit = CameraRef.LeftLimit == EnemyLeftLimit ? previousLeftLimit : EnemyLeftLimit;
            CameraRef.TopLimit = CameraRef.TopLimit == EnemyTopLimit ? previousTopLimit : EnemyTopLimit;
            CameraRef.RightLimit = CameraRef.RightLimit == EnemyRightLimit ? previousRightLimit : EnemyRightLimit;
            CameraRef.BottomLimit = CameraRef.BottomLimit == EnemyBottomLimit ? previousBottomLimit : EnemyBottomLimit;
        }
    }
}

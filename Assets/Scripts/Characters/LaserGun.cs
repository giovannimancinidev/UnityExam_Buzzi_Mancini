using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LaserGun : MonoBehaviour
{
    [Header ("Settings")]
    public Transform LaserOrigin;
    public float gunRange = 50f;

    private LineRenderer laserLine;

    public RaycastHit Hit { get; private set; }

    void Awake()
    {
        laserLine = GetComponent<LineRenderer>();
    }

    void Update()
    {
        laserLine.SetPosition(0, LaserOrigin.position);
        RaycastHit hit;

        if (Physics.Raycast(LaserOrigin.position, LaserOrigin.forward, out hit, gunRange))
        {
            laserLine.SetPosition(1, hit.point);
        }
        else
        {
            laserLine.SetPosition(1, LaserOrigin.position + LaserOrigin.forward * gunRange);
        }

        Hit = hit;
    }
}
